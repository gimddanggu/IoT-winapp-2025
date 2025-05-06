using Microsoft.VisualBasic.Logging;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            None,

            Pen,
            Brush,
            Marker,
            Crayon,
            Spray,
            Eraser
        }
        /* �������� */
        int eraserSize = 20;        // �⺻ ���찳 ũ��
        int penSize = 2;            // �⺻ �� ũ��
        Color previewColor;         // ���� �׸���� �̸����� ���� ������ ���󿡼� ȸ���� ������
        Color penColor = Color.Black;           // �⺻ �� ����
        ToolMode currentPenMode = ToolMode.Pen; // �⺻ ��� : ��
        ShapeMode currentShapeMode = ShapeMode.None;

        Bitmap texture;
        Brush crayonBrush;

        Bitmap drawingCanvas;       // �׸���� ĵ����
        Bitmap previewCanvas;       // ���� �̸������ ĵ����
        Bitmap backgroundCanvas;    // ��� ĵ����
        Graphics g;

        Pen previewPen;

        Image img;
        Random rnd = new Random();
        float prevAngle = 0f; // Ŭ���� ����� ��������� �� // �߰�

        float lastAngle = 0;

        Stack<Bitmap> undoStack = new Stack<Bitmap>();  // �׸� �ǵ����� ����� ���� ��Ʈ�� ���� ����
        Stack<Bitmap> redoStack = new Stack<Bitmap>();  // �׸� �ǵ����� ����� ���� ��Ʈ�� ���� ����


        bool isDrawing = false;

        Color backgroundColor = Color.White;
        Point prevPoint;
        Point startPoint;

        /* �Լ� */
        // ������ ���� ���� �̸����� ���� �� ��ȭ
        void UpdatePreviewPen(Color baseColor)
        {
            Color gray = Color.Gray;
            float ratio = 0.5f;
            Color blended = Color.FromArgb(
                (int)(baseColor.R * ratio + gray.R * (1 - ratio)),
                (int)(baseColor.G * ratio + gray.G * (1 - ratio)),
                (int)(baseColor.B * ratio + gray.B * (1 - ratio))
            );

            previewPen?.Dispose();
            previewPen = new Pen(blended, penSize)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
        }



        // ���/ �׸��� / �̸����� ĵ���� ��ħ
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

        // �������찳 ����(�簢��)
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

        // �簢�� ��ġ ����
        private Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y)
            );
        }

        // �ﰢ�� ��ġ ����
        private Point[] GetTrianglePoints(Point p1, Point p2)
        {
            Point top = new Point((p1.X + p2.X) / 2, Math.Min(p1.Y, p2.Y));         // ������ ��ǥ
            Point left = new Point(Math.Min(p1.X, p2.X), Math.Max(p1.Y, p2.Y));     // ���� �Ʒ� ��ǥ
            Point right = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));       // ������ �Ʒ� ��ǥ

            return new Point[] { top, left, right };

        }
        // �ؽ��� �̹��� ���ϰ� �����
        private static Bitmap MakeBitmapTransparent(Bitmap src, int alpha)
        {
            Bitmap output = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(output))
            {
                // ���� ��� ����
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, src.Width, src.Height);
                    g.SetClip(path);  // Ŭ���� ����
                }

                ColorMatrix colorMatrix = new ColorMatrix()
                {
                    Matrix33 = alpha / 255f
                };
                ImageAttributes attr = new ImageAttributes();
                attr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height),
                    0, 0, src.Width, src.Height,
                    GraphicsUnit.Pixel, attr);
            }
            return output;
        }

        // ���� �� ��� ����
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


                case ToolMode.Marker:
                    return new Pen(Color.FromArgb(20, penColor), penSize * 3)
                    {
                        StartCap = System.Drawing.Drawing2D.LineCap.Round,
                        EndCap = System.Drawing.Drawing2D.LineCap.Round
                    };


                default:
                    return new Pen(penColor, penSize);

            }
        }

        private Bitmap TintImage(Bitmap original, Color tint)
        {
            float boost = 4f;

            Bitmap tinted = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(tinted))
            {
                // �� ������: ColorMatrix Ȱ�� ����, ���⼱ ������ ���� ���
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
            new float[] {tint.R / 255f * boost, 0, 0, 0, 0},
            new float[] {0, tint.G / 255f * boost, 0, 0, 0},
            new float[] {0, 0, tint.B / 255f * boost, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
                });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height,
                    GraphicsUnit.Pixel, attributes);
            }
            return tinted;
        }

        

        /* �̺�Ʈ �Լ� */
        private void FrmMain_Load(object sender, EventArgs e)
        {
            int w = WhiteBoard.Width;
            int h = WhiteBoard.Height;

            drawingCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            previewCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb);    // ���� �̸����⸦ ���� ĵ����
            backgroundCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb); // ��� ĵ����


            UpdatePreviewPen(penColor); // previewPenColor �ʱ�ȭ

            // ��� �ʱ�ȭ
            using (Graphics g = Graphics.FromImage(backgroundCanvas))
            {
                g.Clear(Color.White);
            }


            texture = new Bitmap("brush_texture4.png"); // ���� ��ο� �ִ� �̹���
            img = TintImage(texture, penColor); // �� �����ؼ� ���� ���
            // �ʱ� ȭ�� ǥ��
            RenderToWhiteBoard();

            // ������ ������ ���� ���۸�
            this.DoubleBuffered = true;
        }

        // ���콺 �ٿ� �̺�Ʈ
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
        // ���콺 ���� �̺�Ʈ
        private void WhiteBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                using (Graphics g = Graphics.FromImage(drawingCanvas))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.CompositingMode = CompositingMode.SourceOver;

                    switch (currentPenMode)
                    {
                        case ToolMode.Crayon:

                            int size = TrbPenSize.Value; // Ʈ���� ������ ���� ����
                            int density = size * 3; // ���⿡ ���� �� ���� ���� (���ϴ� ��� ����)

                            using (Brush crayonBrush = new SolidBrush(penColor))

                                for (int i = 0; i < density; i++)
                                {
                                    int dx = rnd.Next(-size, size);
                                    int dy = rnd.Next(-size, size);
                                    g.FillEllipse(crayonBrush, e.X + dx, e.Y + dy, 2, 2);
                                }
                            break;

                        case ToolMode.Brush:
                            int width = img.Width;
                            int height = img.Height;

                            if (img == null)
                            {
                                Console.WriteLine("�귯�� �ؽ�ó �̹����� �ε����� �ʾҽ��ϴ�.");
                                break;
                            }


                            using (TextureBrush textureBrush = new TextureBrush(img))
                            {
                                textureBrush.WrapMode = WrapMode.Clamp;
                                float x = e.X - width / 2f;
                                float y = e.Y - height / 2f;

                                float dx = e.X - prevPoint.X;
                                float dy = e.Y - prevPoint.Y;
                                float angle = (float)(Math.Atan2(dy, dx) * (180 / Math.PI));

                                float centerX = width / 2f;
                                float centerY = height / 2f;

                                if (Math.Sqrt(dx * dx + dy * dy) > 2)  // �ʹ� ª�� �̵��� ����
                                {
                                    angle = lastAngle + (angle - lastAngle) * 0.15f; // �������� �ڿ�������
                                    lastAngle = angle;
                                }

                                textureBrush.ResetTransform();
                                textureBrush.TranslateTransform(x + centerX, y + centerY);
                                textureBrush.RotateTransform(lastAngle);
                                textureBrush.TranslateTransform(-centerX, -centerY);

                                g.FillEllipse(textureBrush, x, y, width, height);
                            }

                            break;

                        case ToolMode.Spray:
                            using (Brush sprayBrush = new SolidBrush(penColor))
                            {
                                int sprayRadius = TrbPenSize.Value; // Ʈ���ٷ� �ݰ� ����
                                int sprayDensity = sprayRadius * 2;
                                // ���� ���� (�е�)

                                for (int i = 0; i < sprayDensity; i++)
                                {

                                    if (rnd.NextDouble() > 0.3) continue; // 40% Ȯ���� �Ѹ�

                                    double angle = rnd.NextDouble() * 2 * Math.PI;
                                    double radius = Math.Sqrt(rnd.NextDouble()) * sprayRadius;
                                    int dx = (int)(Math.Cos(angle) * radius);
                                    int dy = (int)(Math.Sin(angle) * radius);

                                    g.FillEllipse(sprayBrush, e.X + dx, e.Y + dy, 2, 2); // �� �ϳ�
                                }
                            }
                            break;


                        case ToolMode.Eraser when currentShapeMode == ShapeMode.None:
                            EraseAt(e.X, e.Y);  // ���⼭ ���� ���찳 ȣ��
                            break;

                        default:
                            using (Pen currentPen = GetPenForCurrentMode())
                            {
                                g.DrawLine(currentPen, prevPoint, e.Location);  // �̵��� �� �׸���

                            }
                            break;
                    }
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
            }

            RenderToWhiteBoard();

        }
        // ���콺 �� �̺�Ʈ
        private void WhiteBoard_MouseUp(object sender, MouseEventArgs e)
        {

            if (!isDrawing) return;
            isDrawing = false;
            Point endPoint = e.Location;

            using (Graphics g = Graphics.FromImage(drawingCanvas))
            using (Pen currentPen = GetPenForCurrentMode())
            {

                // ���� ������ �׸� �� �̹� �׷��� ������ ������ ���ϴ� ������ �ִ� (�����ʿ�)
                //new Pen(penColor, penSize)

                if (penColor.IsEmpty)
                {
                    MessageBox.Show("�� ������ ����ֽ��ϴ�. �⺻������ �����մϴ�.");
                    penColor = Color.Black;
                }

                if (currentShapeMode == ShapeMode.Rectangle)
                    g.DrawRectangle(currentPen, GetRectangle(startPoint, endPoint));
                else if (currentShapeMode == ShapeMode.Triangle)
                    g.DrawPolygon(currentPen, GetTrianglePoints(startPoint, endPoint));
                else if (currentShapeMode == ShapeMode.Circle)
                    g.DrawEllipse(currentPen, GetRectangle(startPoint, endPoint));

            }

            previewCanvas = new Bitmap(drawingCanvas.Width, drawingCanvas.Height, PixelFormat.Format32bppArgb);



            RenderToWhiteBoard();
        }

        // ���� ��ư Ŭ��
        private void BtnColor_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]�����ư Ŭ��");
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                penColor = DlgColor.Color;
                UpdatePreviewPen(penColor);

                if (currentPenMode == ToolMode.Brush)
                    img = TintImage(texture, penColor);
            }
        }

        // ��ü ���찳 ��ư Ŭ��
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
            // (?) ���� �� ���ذ� �ȵǴϱ� �ٽ� ã�ƺ���
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

        // ���찳 ��ư Ŭ��
        private void BtnEraser_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]���찳��ư Ŭ��");
            currentPenMode = ToolMode.Eraser;
            currentShapeMode = ShapeMode.None;
        }


        // ��� ���� ��ư
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

        // �ǵ����� ��ư
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

        // ���� �׸��� ��ư
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

        // Ʈ���� �̺�Ʈ
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

        // �� ���� ���� ��ư
        private void BtnPen_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]���ư Ŭ��");
            currentPenMode = ToolMode.Pen;
            currentShapeMode = ShapeMode.None;
        }
        private void BtnBrush_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]�귯����ư Ŭ��");
            currentPenMode = ToolMode.Brush;
            currentShapeMode = ShapeMode.None;

            if (img == null)
            {
                Console.WriteLine("�̹����ε��ȵ�");
            }
            else Console.WriteLine("�̹����ε� ��");
        }

        private void BtnMarker_Click(object sender, EventArgs e)
        {
            currentPenMode = ToolMode.Marker;
            currentShapeMode = ShapeMode.None;
        }

        private void BtnCrayon_Click(object sender, EventArgs e)
        {
            currentPenMode = ToolMode.Crayon;
            currentShapeMode = ShapeMode.None;
        }

        private void BtnSpray_Click(object sender, EventArgs e)
        {
            currentPenMode = ToolMode.Spray;
            currentShapeMode = ShapeMode.None;
        }
    }
}
