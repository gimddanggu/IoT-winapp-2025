using Microsoft.VisualBasic.Logging;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Net;
using System.Runtime.CompilerServices;

namespace PaintBoard
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            // ������ ������ ���� ���۸�
            this.DoubleBuffered = true;
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
        int penSize = 5;            // �⺻ �� ũ��
        Color previewColor;         // ���� �׸���� �̸����� ���� ������ ���󿡼� ȸ���� ������
        Color penColor = Color.Black;           // �⺻ �� ����
        ToolMode currentPenMode = ToolMode.Pen; // �⺻ ��� : ��
        ShapeMode currentShapeMode = ShapeMode.None;

        Bitmap texture;
        Brush crayonBrush;
        Pen currentPen;

        Bitmap drawingCanvas;       // �׸���� ĵ����
        Bitmap previewCanvas;       // ���� �̸������ ĵ����
        Bitmap backgroundCanvas;    // ��� ĵ����
        Bitmap imageToInsert;
        Graphics g;

        Pen previewPen;

        Image img;
        Random rnd = new Random();
        float prevAngle = 0f; // Ŭ���� ����� ��������� �� // �߰�

        float lastAngle = 0;

        Stack<Bitmap> undoStack = new Stack<Bitmap>();  // �׸� �ǵ����� ����� ���� ��Ʈ�� ���� ����
        Stack<Bitmap> redoStack = new Stack<Bitmap>();  // �׸� �ǵ����� ����� ���� ��Ʈ�� ���� ����


        bool isDrawing = false;
        bool isFull = false;
        bool isBackground = false;
        bool isInsertingImage = false;
        bool isBgEraser = false;
        Color backgroundColor = Color.White;
        Point prevPoint;
        Point startPoint;
        Point insertStartPoint;

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

            Bitmap targetCanvas = isBgEraser ? backgroundCanvas : drawingCanvas;
            // LockBits 
            // ��Ʈ���� �ü���ܿ��� �����ϰ� �ֱ� ������ ���� �����Ϸ��� LockBits�� ����ؼ� �����ؾ߸� �Ѵ�.
            BitmapData bmpData = targetCanvas.LockBits(
                    new Rectangle(0, 0, targetCanvas.Width, targetCanvas.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format32bppArgb);


            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;     // ù���� �ȼ��� ������ ???

                for (int i = 0; i < eraseRect.Height; i++)
                {
                    int PosY = eraseRect.Y + i;
                    if (PosY < 0 || PosY > targetCanvas.Height) continue; // ��� �� ���� 

                    for (int j = 0; j < eraseRect.Width; j++)
                    {
                        int PosX = eraseRect.X + j;
                        if (PosX < 0 || PosX > targetCanvas.Width) continue; // ��� �� ����

                        int offset = (PosY * bmpData.Stride) + (PosX * 4);       // stride ???

                        ptr[offset + 3] = 0;        // ���� ä�� (����)�� 0���� ����� (���� ����)

                    }
                }
            }

            targetCanvas.UnlockBits(bmpData);
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


        // ũ���� ���Ժ��� �Լ�
        public static double NextGaussian(Random rnd, double mean, double stddev)
        {
            // Box-Muller transform
            double u1 = 1.0 - rnd.NextDouble();
            double u2 = 1.0 - rnd.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);
            return mean + stddev * randStdNormal;
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

        private void MakePen()
        {
            if (currentPenMode == ToolMode.Pen)
            {
                currentPen = new Pen(penColor, penSize)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                };
            }
            else if (currentPenMode == ToolMode.Marker)
            {
                Console.WriteLine("��Ŀ�� ������");
                Color semiTransparent = Color.FromArgb(40, penColor);
                currentPen = new Pen(semiTransparent, penSize * 3)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                    LineJoin = LineJoin.Round
                };

            }

        }

        // ���� ����
        private void SaveDrawingToFile()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG ����|*.png|JPEG ����|*.jpg|BMP ����|*.bmp";
                sfd.Title = "�̹��� ����";
                sfd.FileName = "my_drawing.png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap merged = new Bitmap(drawingCanvas.Width, drawingCanvas.Height, PixelFormat.Format32bppArgb);

                    using (Graphics g = Graphics.FromImage(merged))
                    {
                        if (isBackground)
                        {
                            g.DrawImage(backgroundCanvas, 0, 0);
                            g.DrawImage(drawingCanvas, 0, 0);
                        }
                        else
                        {
                            g.Clear(Color.Transparent);  // ���� ��� (JPG�� ���� �ȵ�)
                            g.DrawImage(drawingCanvas, 0, 0);
                        }


                    }

                    merged.Save(sfd.FileName, ImageFormat.Png);
                    MessageBox.Show("���� �Ϸ�!", "�˸�");
                }
            }
        }

        // undo/redo ����Ȯ��
        private void UpdateButtonsEnable()
        {
            BtnUndo.Enabled = undoStack.Count > 0;
            BtnRedo.Enabled = redoStack.Count > 0;
            if (undoStack.Count > 0)
                BtnEraser.Enabled = true;
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

            // �⺻ �� 
            currentPen = new Pen(penColor, penSize);
            texture = new Bitmap("brush_texture4.png"); // ���� ��ο� �ִ� �̹���
            img = TintImage(texture, penColor); // �� �����ؼ� ���� ���

            // �ʱ�  redo/undo ��ư ��Ȱ��ȭ
            BtnUndo.Enabled = false;
            BtnRedo.Enabled = false;
            BtnEraser.Enabled = false;

            // �ʱ� ȭ�� ǥ��
            RenderToWhiteBoard();

        }

        // ���콺 �ٿ� �̺�Ʈ
        private void WhiteBoard_MouseDown(object sender, MouseEventArgs e)
        {
            MakePen();

            if (isInsertingImage)
            {
                insertStartPoint = e.Location;
                undoStack.Push(new Bitmap(backgroundCanvas));
            }

                
            if (currentShapeMode == ShapeMode.None || currentPenMode == ToolMode.Eraser)
            {
                isDrawing = true;
                prevPoint = e.Location;

            }

            else if (currentShapeMode == ShapeMode.None && currentPenMode != ToolMode.None)
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
            if (!isInsertingImage)
                undoStack.Push(new Bitmap(drawingCanvas));
            UpdateButtonsEnable();
            
            

        }
        // ���콺 ���� �̺�Ʈ
        private void WhiteBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            using (Graphics g = Graphics.FromImage(drawingCanvas))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //g.CompositingMode = CompositingMode.SourceOver;

                switch (currentPenMode)
                {
                    case ToolMode.Crayon:

                        int size = TrbPenSize.Value * 3; // Ʈ���� ������ ���� ����
                        int density = size * 3; // ���⿡ ���� �� ���� ���� (���ϴ� ��� ����)

                        using (Brush crayonBrush = new SolidBrush(penColor))

                            for (int i = 0; i < density; i++)
                            {
                                // �߽ɿ� �е� ���ߵǵ��� ���Ժ��� ���
                                double cdx = NextGaussian(rnd, 0, size / 3.0);
                                double cdy = NextGaussian(rnd, 0, size / 3.0);

                                g.FillEllipse(crayonBrush, (float)(e.X + cdx), (float)(e.Y + cdy), 2, 2);
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


                        float dbx = e.X - prevPoint.X;
                        float dby = e.Y - prevPoint.Y;
                        float distance = (float)Math.Sqrt(dbx * dbx + dby * dby);
                        float bstep = width * 0.4f; // ���� ����

                        float baseAngle = (float)(Math.Atan2(dby, dbx) * (180 / Math.PI));
                        if (distance > 2)
                        {
                            baseAngle = lastAngle + (baseAngle - lastAngle) * 0.15f;
                            lastAngle = baseAngle;
                        }

                        float centerX = img.Width / 2f;
                        float centerY = img.Height / 2f;

                        for (float d = 0; d <= distance; d += bstep)
                        {
                            float t = d / distance;
                            float x = prevPoint.X + dbx * t - width / 2f;
                            float y = prevPoint.Y + dby * t - height / 2f;

                            using (TextureBrush textureBrush = new TextureBrush(img))
                            {
                                textureBrush.WrapMode = WrapMode.Clamp;
                                textureBrush.ResetTransform();
                                textureBrush.TranslateTransform(x + centerX, y + centerY);
                                textureBrush.RotateTransform(baseAngle);
                                textureBrush.TranslateTransform(-centerX, -centerY);

                                g.FillEllipse(textureBrush, x, y, width, height);
                            }
                        }


                        break;

                    case ToolMode.Spray:
                        using (Brush sprayBrush = new SolidBrush(penColor))
                        {
                            int sprayRadius = TrbPenSize.Value * 2; // Ʈ���ٷ� �ݰ� ����
                            int sprayDensity = sprayRadius * 2;
                            // ���� ���� (�е�)

                            for (int i = 0; i < sprayDensity; i++)
                            {

                                if (rnd.NextDouble() > 0.3) continue; // 30% Ȯ���� �Ѹ�

                                double angle = rnd.NextDouble() * 2 * Math.PI;
                                double radius = Math.Sqrt(rnd.NextDouble()) * sprayRadius;
                                int sdx = (int)(Math.Cos(angle) * radius);
                                int sdy = (int)(Math.Sin(angle) * radius);

                                g.FillEllipse(sprayBrush, e.X + sdx, e.Y + sdy, 2, 2); // �� �ϳ�
                            }
                        }
                        break;


                    case ToolMode.Eraser when currentShapeMode == ShapeMode.None:
                        EraseAt(e.X, e.Y);  // ���⼭ ���� ���찳 ȣ��
                        break;

                    case ToolMode.Marker when currentShapeMode == ShapeMode.None:
                        float dx = e.X - prevPoint.X;
                        float dy = e.Y - prevPoint.Y;
                        float dist = MathF.Sqrt(dx * dx + dy * dy);

                        float step = 1.5f;
                        for (float i = 0; i < dist; i += step)
                        {
                            float t = i / dist;
                            float x = prevPoint.X + dx * t;
                            float y = prevPoint.Y + dy * t;
                            PointF p1 = new PointF(x, y);
                            PointF p2 = new PointF(x + 0.8f, y + 0.8f); // �ణ ������ ������ ����

                            g.DrawLine(currentPen, prevPoint, e.Location);

                        }

                        break;

                    case ToolMode.Pen when currentShapeMode == ShapeMode.None:
                        g.DrawLine(currentPen, prevPoint, e.Location);  // �̵��� �� �׸���
                        break;



                        //default:
                        //    using (Pen currentPen = GetPenForCurrentMode())
                        //    {
                        //        g.DrawLine(currentPen, prevPoint, e.Location);  // �̵��� �� �׸���

                        //    }
                        //    break;
                }
                prevPoint = e.Location;     // ���� ��ǥ�� ���� ���������

            }
            if (isInsertingImage)
            {
                previewCanvas = new Bitmap(drawingCanvas);
                Point endPoint = e.Location;
                using (Graphics g = Graphics.FromImage(previewCanvas))
                {
                    Rectangle rect = GetRectangle(insertStartPoint, endPoint);
                    g.DrawRectangle(previewPen, rect);
                }
            }

            if (currentShapeMode == ShapeMode.Rectangle ||
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

            if (isInsertingImage && imageToInsert != null)
            {
                Point insertEndPoint = e.Location;

                Rectangle insertRect = new Rectangle(
                    Math.Min(insertStartPoint.X, insertEndPoint.X),
                    Math.Min(insertStartPoint.Y, insertEndPoint.Y),
                    Math.Abs(insertStartPoint.X - insertEndPoint.X),
                    Math.Abs(insertStartPoint.Y - insertEndPoint.Y)
                );

                using (Graphics g = Graphics.FromImage(backgroundCanvas))
                {
                    g.DrawImage(imageToInsert, insertRect);
                }

                isInsertingImage = false;
                imageToInsert.Dispose();
                imageToInsert = null;
                previewCanvas = new Bitmap(drawingCanvas.Width, drawingCanvas.Height, PixelFormat.Format32bppArgb);

                RenderToWhiteBoard();
                return;


            }

            //if (penColor.IsEmpty)
            //{
            //    MessageBox.Show("�� ������ ����ֽ��ϴ�. �⺻������ �����մϴ�.");
            //    penColor = Color.Black;
            //}
            using (Graphics g = Graphics.FromImage(drawingCanvas))
            {
                if (isFull)
                {
                    using (SolidBrush brush = new SolidBrush(penColor))
                    {
                        if (currentShapeMode == ShapeMode.Rectangle)
                            g.FillRectangle(brush, GetRectangle(startPoint, endPoint));
                        else if (currentShapeMode == ShapeMode.Triangle)
                            g.FillPolygon(brush, GetTrianglePoints(startPoint, endPoint));
                        else if (currentShapeMode == ShapeMode.Circle)
                            g.FillEllipse(brush, GetRectangle(startPoint, endPoint));
                    }
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

        // �׸� ��ü �ʱ�ȭ, undo, redo�� �ʱ�ȭ
        private void BtnAllClear_Click(object sender, EventArgs e)
        {
            undoStack.Clear();
            redoStack.Clear();

            using (Graphics g = Graphics.FromImage(backgroundCanvas))
            {
                g.Clear(Color.White);
            }
            backgroundColor = Color.White;

            using (Graphics g = Graphics.FromImage(drawingCanvas))
            {
                g.Clear(Color.Transparent); 
            }


            RenderToWhiteBoard();  // 3. ���� ȭ�� �ٽ� �����ֱ�
            UpdateButtonsEnable();
        }

        // ���찳 ��ư Ŭ��
        private void BtnEraser_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]���찳��ư Ŭ��");
            currentPenMode = ToolMode.Eraser;
            currentShapeMode = ShapeMode.None;
            isBgEraser = false;

        }

        private void BtnBgEraser_Click(object sender, EventArgs e)
        {
            currentPenMode = ToolMode.Eraser;
            currentShapeMode = ShapeMode.None;
            isBgEraser = true;
        }

        private void BtnAllBgClear_Click(object sender, EventArgs e)
        {
            undoStack.Clear();
            redoStack.Clear();
            undoStack.Push(new Bitmap(drawingCanvas));
            using (Graphics g = Graphics.FromImage(backgroundCanvas))
            {
                g.Clear(Color.White);
            }
            backgroundColor = Color.White;

            RenderToWhiteBoard();  // 3. ���� ȭ�� �ٽ� �����ֱ�

        }

        private void BtnAllDrawClear_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]��ü������ư Ŭ��");
            undoStack.Clear();
            redoStack.Clear();
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
            Console.WriteLine(undoStack.Count.ToString());
            if (undoStack.Count > 0)
            {
                //BtnRedo.Enabled = true;
                Console.WriteLine("[DEBUG]undo��ư Ŭ��");
                redoStack.Push(new Bitmap(drawingCanvas));
                drawingCanvas = undoStack.Pop();
                g = Graphics.FromImage(drawingCanvas);
                RenderToWhiteBoard();
            }
            UpdateButtonsEnable();
            Console.WriteLine($"[DEBUG] UndoStack: {undoStack.Count}, RedoStack: {redoStack.Count}");

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
            UpdateButtonsEnable();
            Console.WriteLine($"[DEBUG] UndoStack: {undoStack.Count}, RedoStack: {redoStack.Count}");

        }

        // ���� �׸��� ��ư
        private void BtnRectangle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]�׸��ư Ŭ��");
            currentShapeMode = ShapeMode.Rectangle;
            isFull = false;
        }

        private void BtnTriangle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]�����ư Ŭ��");
            currentShapeMode = ShapeMode.Triangle;
            isFull = false;
        }

        private void BtnCircle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]����ư Ŭ��");
            currentShapeMode = ShapeMode.Circle;
            isFull = false;
        }

        // Ʈ���� �̺�Ʈ
        private void TrbPenSize_ValueChanged(object sender, EventArgs e)
        {
            penSize = TrbPenSize.Value;
            LblPenSize.Text = "�� ũ��: " + penSize.ToString();
            Console.WriteLine($"[DEBUG] PenSize:{penSize}");

        }

        private void TrbEraserSize_ValueChanged(object sender, EventArgs e)
        {
            eraserSize = TrbEraserSize.Value;
            LblEraserSize.Text = "���찳 ũ��: " + eraserSize.ToString();
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


        private void BtnRectangleFull_Click(object sender, EventArgs e)
        {
            currentShapeMode = ShapeMode.Rectangle;
            isFull = true;
        }

        private void BtnTriangleFull_Click(object sender, EventArgs e)
        {
            currentShapeMode = ShapeMode.Triangle;
            isFull = true;
        }

        private void BtnCircleFull_Click(object sender, EventArgs e)
        {
            currentShapeMode = ShapeMode.Circle;
            isFull = true;
        }

        private void BtnNoBgSave_Click(object sender, EventArgs e)
        {
            SaveDrawingToFile();
            isBackground = false;
        }

        private void BtnSaveImg_Click(object sender, EventArgs e)
        {
            SaveDrawingToFile();
            isBackground = true;
        }
        private void BtnLoadImg_Click(object sender, EventArgs e)
        {
            currentShapeMode = ShapeMode.None;
            currentPenMode = ToolMode.None;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "�̹��� ����|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imageToInsert = new Bitmap(ofd.FileName); // �ӽ� ����
                    isInsertingImage = true;
                    MessageBox.Show("�̹����� ���� ��ġ�� �巡�׷� �����ϼ���.");
                }
            }
        }

        
    }
}
