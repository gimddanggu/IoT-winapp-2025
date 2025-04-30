using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.CompilerServices;

namespace PaintBoard
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        enum ShapeMode
        {
            None,
            Circle,
            Rectangle,
            Triangle
        }

        enum ToolMode
        {
            Pen,
            Brush,
            Marker,
            Crayon,
            Eraser
        }

        int eraserSize = 20;    // �⺻ ���찳 ũ��
        int penSize = 2;
        Color penColor = Color.Black;
        ToolMode currentPenMode = ToolMode.Pen; // �⺻ ��� : ��
        ShapeMode currentShapeMode = ShapeMode.None;

        //Bitmap texture = new Bitmap("crayon_texture.png");
        //Brush crayonBrush = new TextureBrush(texture);

        Bitmap drawingCanvas;       // �׸���� ĵ����
        Bitmap previewCanvas;       // ���� �̸������ ĵ����
        Bitmap backgroundCanvas;    // ��� ĵ����
        Graphics g;
        Pen drawPen;    // ���������� �ʿ� x


        Stack<Bitmap> undoStack = new Stack<Bitmap>();  // �׸� �ǵ����� ����� ���� ��Ʈ�� ���� ����
        Stack<Bitmap> redoStack = new Stack<Bitmap>();  // �׸� �ǵ����� ����� ���� ��Ʈ�� ���� ����


        bool isDrawing = false;

        Color backgroundColor = Color.White;
        Point prevPoint;
        Point startPoint;

        private void RenderToWhiteBoard()
        {
            Bitmap final = new Bitmap(drawingCanvas.Width, drawingCanvas.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(final))
            {
                g.DrawImage(backgroundCanvas, 0, 0);
                g.DrawImage(drawingCanvas, 0, 0);
                g.DrawImage(previewCanvas, 0, 0);
            }

            WhiteBoard.Image = final;
        }
        private void EraseAt(int x, int y)
        {
            Rectangle eraseRect = new Rectangle(
                x - eraserSize / 2,
                y - eraserSize / 2,
                eraserSize,
                eraserSize
            );

            // LockBits 
            // ��Ʈ���� �ü���ܿ��� �����ϰ� �ֱ� ������ ���� �����Ϸ��� LockBits�� ����ؼ� �����ؾ߸� �Ѵ�.
            BitmapData bmpData = drawingCanvas.LockBits(
                new Rectangle(0, 0, drawingCanvas.Width, drawingCanvas.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);


            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;     // ù���� �ȼ��� ������ ???

                for (int i = 0; i < eraseRect.Height; i++)
                {
                    int PosY = eraseRect.Y + i;
                    if (PosY < 0 || PosY > drawingCanvas.Height) continue; // ��� �� ���� 

                    for (int j = 0; j < eraseRect.Width; j++)
                    {
                        int PosX = eraseRect.X + j;
                        if (PosX < 0 || PosX > drawingCanvas.Width) continue; // ��� �� ����

                        int offset = (PosY * bmpData.Stride) + (PosX * 4);       // stride ???

                        ptr[offset + 3] = 0;        // ���� ä�� (����)�� 0���� ����� (���� ����)

                    }
                }
            }

            drawingCanvas.UnlockBits(bmpData);
        }

        private Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y)
            );
        }

        private Point[] GetTrianglePoints(Point p1, Point p2)
        {
            Point top = new Point((p1.X + p2.X) / 2, Math.Min(p1.Y, p2.Y));         // ������ ��ǥ
            Point left = new Point(Math.Min(p1.X, p2.X), Math.Max(p1.Y, p2.Y));     // ���� �Ʒ� ��ǥ
            Point right = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));       // ������ �Ʒ� ��ǥ

            return new Point[] { top, left, right };

        }

        private Pen GetPenForCurrentMode()
        {
            switch (currentPenMode)
            {
                case ToolMode.Pen:
                    return new Pen(penColor, penSize)
                    {
                        StartCap = System.Drawing.Drawing2D.LineCap.Round,
                        EndCap = System.Drawing.Drawing2D.LineCap.Round
                    };

                case ToolMode.Brush:
                    return new Pen(Color.FromArgb(200, penColor), penSize * 3)
                    {
                        StartCap = System.Drawing.Drawing2D.LineCap.Round,
                        EndCap = System.Drawing.Drawing2D.LineCap.Round,
                        LineJoin = System.Drawing.Drawing2D.LineJoin.Round
                    };

                case ToolMode.Marker:
                    return new Pen(Color.FromArgb(80, penColor), penSize * 3)
                    {
                        StartCap = System.Drawing.Drawing2D.LineCap.Round,
                        EndCap = System.Drawing.Drawing2D.LineCap.Round
                    };

                //case ToolMode.Crayon:
                //    // ���� ó�� ����غ���

                //    Pen crayonPen = new Pen(crayonBrush, penSize * 2)
                //    {
                //        StartCap = System.Drawing.Drawing2D.LineCap.Round,
                //        EndCap = System.Drawing.Drawing2D.LineCap.Round
                //    };
                //    return crayonPen;

                default:
                    return new Pen(penColor, penSize);

            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            int w = WhiteBoard.Width;
            int h = WhiteBoard.Height;

            drawingCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            previewCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb);    // ���� �̸����⸦ ���� ĵ����
            backgroundCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb); // ��� ĵ����

            drawPen = new Pen(Color.Black, penSize); // ����?

            using (Graphics g = Graphics.FromImage(backgroundCanvas))
            {
                g.Clear(Color.White);
            }
            // �ʱ� ȭ�� ǥ��
            RenderToWhiteBoard();

            // ������ ������ ���� ���۸�
            this.DoubleBuffered = true;
        }

        private void WhiteBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentShapeMode == ShapeMode.None || currentPenMode == ToolMode.Eraser)
            {
                isDrawing = true;
                prevPoint = e.Location;

            }


            if (currentShapeMode == ShapeMode.Rectangle ||
                currentShapeMode == ShapeMode.Triangle ||
                currentShapeMode == ShapeMode.Circle)
            {
                isDrawing = true;
                startPoint = e.Location;
            }

            // ���� drawingCanvas �����ؼ� ����
            undoStack.Push(new Bitmap(drawingCanvas));

        }

        private void WhiteBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                if (currentPenMode == ToolMode.Eraser)
                {
                    EraseAt(e.X, e.Y);  // ���⼭ ���� ���찳 ȣ��

                }
                else if (currentShapeMode == ShapeMode.None)
                {
                    using (Graphics g = Graphics.FromImage(drawingCanvas))
                    using (Pen currentPen = GetPenForCurrentMode())
                    {
                        //currentPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        //currentPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.DrawLine(currentPen, prevPoint, e.Location);  // �̵��� �� �׸���
                        prevPoint = e.Location;     // ���� ��ǥ�� ���� ���������
                    }

                }
                else if (currentShapeMode == ShapeMode.Rectangle ||
                        currentShapeMode == ShapeMode.Circle ||
                        currentShapeMode == ShapeMode.Triangle)
                {
                    // �̸����� ĵ���� �ʱ�ȭ
                    previewCanvas = new Bitmap(drawingCanvas);

                    using (Graphics g = Graphics.FromImage(previewCanvas))
                    {
                        Pen previewPen = new Pen(Color.Gray, penSize)
                        {
                            DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
                        };

                        Point endPoint = e.Location;

                        // �׸� �׸���
                        if (currentShapeMode == ShapeMode.Rectangle)
                        {
                            Rectangle rect = GetRectangle(startPoint, endPoint);
                            g.DrawRectangle(previewPen, rect);
                        }
                        // ���� �׸���
                        if (currentShapeMode == ShapeMode.Triangle)
                        {
                            Point[] triangle = GetTrianglePoints(startPoint, endPoint);
                            g.DrawPolygon(previewPen, triangle);
                        }
                        // �� �׸���
                        if (currentShapeMode == ShapeMode.Circle)
                        {
                            Rectangle ellipseRect = GetRectangle(startPoint, endPoint);
                            g.DrawEllipse(previewPen, ellipseRect);
                        }
                    }
                    //WhiteBoard.Image = previewCanvas;
                    //WhiteBoard.Invalidate();
                }

                RenderToWhiteBoard();
            }
        }

        private void WhiteBoard_MouseUp(object sender, MouseEventArgs e)
        {

            if (!isDrawing) return;
            isDrawing = false;
            Point endPoint = e.Location;

            using (Graphics g = Graphics.FromImage(drawingCanvas))
            {
                if (currentShapeMode == ShapeMode.Rectangle)
                    g.DrawRectangle(drawPen, GetRectangle(startPoint, endPoint));
                else if (currentShapeMode == ShapeMode.Triangle)
                    g.DrawPolygon(drawPen, GetTrianglePoints(startPoint, endPoint));
                else if (currentShapeMode == ShapeMode.Circle)
                    g.DrawEllipse(drawPen, GetRectangle(startPoint, endPoint));

            }

            previewCanvas = new Bitmap(drawingCanvas.Width, drawingCanvas.Height, PixelFormat.Format32bppArgb);


            // undo ���� // ���콺�� �ѹ� ���� undo ���ÿ� ����
            //undoStack.Push(new Bitmap(canvas));

            RenderToWhiteBoard();
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]�����ư Ŭ��");
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                penColor = DlgColor.Color;
            }
        }

        private void BtnAllClear_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]��ü������ư Ŭ��");
            undoStack.Push(new Bitmap(drawingCanvas));
            //g.Clear(backgroundColor);
            //WhiteBoard.Invalidate();
            //using (Graphics g = Graphics.FromImage(drawingCanvas))
            //{
            //    g.Clear(backgroundColor);
            //}
            //RenderToWhiteBoard();

            // 1. �׸� �׸� ĵ���� �ʱ�ȭ
            using (Graphics g = Graphics.FromImage(drawingCanvas))
            {
                g.Clear(Color.Transparent); // �Ǵ� backgroundColor �ᵵ OK
            }

            // 2. ��浵 �ٽ� �ʱ�ȭ�ؾ� ȭ���� ������ �ٲ�
            using (Graphics g = Graphics.FromImage(backgroundCanvas))
            {
                g.Clear(backgroundColor);
            }

            RenderToWhiteBoard();  // 3. ���� ȭ�� �ٽ� �����ֱ�

        }

        private void BtnEraser_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]���찳��ư Ŭ��");
            currentPenMode = ToolMode.Eraser;
        }


        private void BtnPen_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]���ư Ŭ��");
            currentPenMode = ToolMode.Pen;
            currentShapeMode = ShapeMode.None;
        }

        private void BtnBackgroundColor_Click(object sender, EventArgs e)
        {
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("[DEBUG]������ư Ŭ��");
                backgroundColor = DlgColor.Color;
                //WhiteBoard.BackColor = backgroundColor;
                //WhiteBoard.Invalidate();            // ȭ�� ����

                using (Graphics g = Graphics.FromImage(backgroundCanvas))
                {
                    g.Clear(backgroundColor);
                }
                RenderToWhiteBoard();
            }
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                Console.WriteLine("[DEBUG]undo��ư Ŭ��");
                redoStack.Push(new Bitmap(drawingCanvas));
                drawingCanvas = undoStack.Pop();
                g = Graphics.FromImage(drawingCanvas);
                RenderToWhiteBoard();
            }
        }

        private void BtnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                Console.WriteLine("[DEBUG]redo��ư Ŭ��");
                undoStack.Push(new Bitmap(drawingCanvas));
                drawingCanvas = redoStack.Pop();
                g = Graphics.FromImage(drawingCanvas);
                RenderToWhiteBoard();
            }
        }

        private void BtnRectangle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]�׸��ư Ŭ��");
            currentShapeMode = ShapeMode.Rectangle;
        }

        private void BtnTriangle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]�����ư Ŭ��");
            currentShapeMode = ShapeMode.Triangle;
        }

        private void BtnCircle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]����ư Ŭ��");
            currentShapeMode = ShapeMode.Circle;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void TrbPenSize_ValueChanged(object sender, EventArgs e)
        {
            penSize = TrbPenSize.Value;
            Console.WriteLine($"[DEBUG] PenSize:{penSize}");

        }

        private void TrbEraserSize_ValueChanged(object sender, EventArgs e)
        {
            eraserSize = TrbEraserSize.Value;
            Console.WriteLine($"[DEBUG] EraserSize:{eraserSize}");
        }

        private void BtnBrush_Click(object sender, EventArgs e)
        {
            currentPenMode = ToolMode.Brush;
        }

        private void BtnMarker_Click(object sender, EventArgs e)
        {
            currentPenMode = ToolMode.Marker;
        }

        private void BtnCrayon_Click(object sender, EventArgs e)
        {
            currentPenMode = ToolMode.Crayon;
        }
    }
}
