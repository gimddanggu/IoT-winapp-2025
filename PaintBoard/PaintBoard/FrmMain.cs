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
            // 깜빡임 방지용 더블 버퍼링
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
        /* 전역변수 */
        int eraserSize = 20;        // 기본 지우개 크기
        int penSize = 5;            // 기본 펜 크기
        Color previewColor;         // 도형 그리기용 미리보기 색상 선택한 색상에서 회색빛 돌도록
        Color penColor = Color.Black;           // 기본 펜 색상
        ToolMode currentPenMode = ToolMode.Pen; // 기본 모드 : 펜
        ShapeMode currentShapeMode = ShapeMode.None;

        Bitmap texture;
        Brush crayonBrush;
        Pen currentPen;

        Bitmap drawingCanvas;       // 그리기용 캔버스
        Bitmap previewCanvas;       // 도형 미리보기용 캔버스
        Bitmap backgroundCanvas;    // 배경 캔버스
        Bitmap imageToInsert;
        Graphics g;

        Pen previewPen;

        Image img;
        Random rnd = new Random();
        float prevAngle = 0f; // 클래스 멤버로 선언해줘야 함 // 추가

        float lastAngle = 0;

        Stack<Bitmap> undoStack = new Stack<Bitmap>();  // 그림 되돌리기 기능을 위한 비트맵 스택 생성
        Stack<Bitmap> redoStack = new Stack<Bitmap>();  // 그림 되돌리기 기능을 위한 비트맵 스택 생성


        bool isDrawing = false;
        bool isFull = false;
        bool isBackground = false;
        bool isInsertingImage = false;
        bool isBgEraser = false;
        Color backgroundColor = Color.White;
        Point prevPoint;
        Point startPoint;
        Point insertStartPoint;

        /* 함수 */
        // 선택한 색상에 따라 미리보기 점선 색 변화
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



        // 배경/ 그리기 / 미리보기 캔버스 합침
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

        // 투명지우개 영영(사각형)
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
            // 비트맵은 운영체제단에서 관리하고 있기 때문에 직접 접근하려면 LockBits를 사용해서 접근해야만 한다.
            BitmapData bmpData = targetCanvas.LockBits(
                    new Rectangle(0, 0, targetCanvas.Width, targetCanvas.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format32bppArgb);


            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;     // 첫번재 픽셀의 포인터 ???

                for (int i = 0; i < eraseRect.Height; i++)
                {
                    int PosY = eraseRect.Y + i;
                    if (PosY < 0 || PosY > targetCanvas.Height) continue; // 경계 밖 무시 

                    for (int j = 0; j < eraseRect.Width; j++)
                    {
                        int PosX = eraseRect.X + j;
                        if (PosX < 0 || PosX > targetCanvas.Width) continue; // 경계 밖 무시

                        int offset = (PosY * bmpData.Stride) + (PosX * 4);       // stride ???

                        ptr[offset + 3] = 0;        // 알파 채널 (투명도)을 0으로 만든다 (완전 투명)

                    }
                }
            }

            targetCanvas.UnlockBits(bmpData);
        }

        // 사각형 위치 설정
        private Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y)
            );
        }

        // 삼각형 위치 설정
        private Point[] GetTrianglePoints(Point p1, Point p2)
        {
            Point top = new Point((p1.X + p2.X) / 2, Math.Min(p1.Y, p2.Y));         // 꼭짓점 좌표
            Point left = new Point(Math.Min(p1.X, p2.X), Math.Max(p1.Y, p2.Y));     // 왼쪽 아래 좌표
            Point right = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));       // 오른쪽 아래 좌표

            return new Point[] { top, left, right };

        }


        // 크래용 정규분포 함수
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
                // 색 입히기: ColorMatrix 활용 가능, 여기선 간단히 곱셈 방식
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
                Console.WriteLine("마커펜 생성됨");
                Color semiTransparent = Color.FromArgb(40, penColor);
                currentPen = new Pen(semiTransparent, penSize * 3)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                    LineJoin = LineJoin.Round
                };

            }

        }

        // 파일 저장
        private void SaveDrawingToFile()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG 파일|*.png|JPEG 파일|*.jpg|BMP 파일|*.bmp";
                sfd.Title = "이미지 저장";
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
                            g.Clear(Color.Transparent);  // 투명 배경 (JPG는 지원 안됨)
                            g.DrawImage(drawingCanvas, 0, 0);
                        }


                    }

                    merged.Save(sfd.FileName, ImageFormat.Png);
                    MessageBox.Show("저장 완료!", "알림");
                }
            }
        }

        // undo/redo 상태확인
        private void UpdateButtonsEnable()
        {
            BtnUndo.Enabled = undoStack.Count > 0;
            BtnRedo.Enabled = redoStack.Count > 0;
            if (undoStack.Count > 0)
                BtnEraser.Enabled = true;
        }

        /* 이벤트 함수 */
        private void FrmMain_Load(object sender, EventArgs e)
        {
            int w = WhiteBoard.Width;
            int h = WhiteBoard.Height;

            drawingCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            previewCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb);    // 도형 미리보기를 위한 캔버스
            backgroundCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb); // 배경 캔버스


            UpdatePreviewPen(penColor); // previewPenColor 초기화

            // 배경 초기화
            using (Graphics g = Graphics.FromImage(backgroundCanvas))
            {
                g.Clear(Color.White);
            }

            // 기본 펜 
            currentPen = new Pen(penColor, penSize);
            texture = new Bitmap("brush_texture4.png"); // 실행 경로에 있는 이미지
            img = TintImage(texture, penColor); // 색 덮입해서 실제 사용

            // 초기  redo/undo 버튼 비활성화
            BtnUndo.Enabled = false;
            BtnRedo.Enabled = false;
            BtnEraser.Enabled = false;

            // 초기 화면 표시
            RenderToWhiteBoard();

        }

        // 마우스 다운 이벤트
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

            // 현재 drawingCanvas 복사해서 저장
            if (!isInsertingImage)
                undoStack.Push(new Bitmap(drawingCanvas));
            UpdateButtonsEnable();
            
            

        }
        // 마우스 무브 이벤트
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

                        int size = TrbPenSize.Value * 3; // 트랙바 값으로 굵기 조절
                        int density = size * 3; // 굵기에 따라 점 개수 증가 (원하는 대로 조절)

                        using (Brush crayonBrush = new SolidBrush(penColor))

                            for (int i = 0; i < density; i++)
                            {
                                // 중심에 밀도 집중되도록 정규분포 사용
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
                            Console.WriteLine("브러시 텍스처 이미지가 로딩되지 않았습니다.");
                            break;
                        }


                        float dbx = e.X - prevPoint.X;
                        float dby = e.Y - prevPoint.Y;
                        float distance = (float)Math.Sqrt(dbx * dbx + dby * dby);
                        float bstep = width * 0.4f; // 도장 간격

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
                            int sprayRadius = TrbPenSize.Value * 2; // 트랙바로 반경 조절
                            int sprayDensity = sprayRadius * 2;
                            // 점의 개수 (밀도)

                            for (int i = 0; i < sprayDensity; i++)
                            {

                                if (rnd.NextDouble() > 0.3) continue; // 30% 확률만 뿌림

                                double angle = rnd.NextDouble() * 2 * Math.PI;
                                double radius = Math.Sqrt(rnd.NextDouble()) * sprayRadius;
                                int sdx = (int)(Math.Cos(angle) * radius);
                                int sdy = (int)(Math.Sin(angle) * radius);

                                g.FillEllipse(sprayBrush, e.X + sdx, e.Y + sdy, 2, 2); // 점 하나
                            }
                        }
                        break;


                    case ToolMode.Eraser when currentShapeMode == ShapeMode.None:
                        EraseAt(e.X, e.Y);  // 여기서 투명 지우개 호출
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
                            PointF p2 = new PointF(x + 0.8f, y + 0.8f); // 약간 움직여 선으로 만듦

                            g.DrawLine(currentPen, prevPoint, e.Location);

                        }

                        break;

                    case ToolMode.Pen when currentShapeMode == ShapeMode.None:
                        g.DrawLine(currentPen, prevPoint, e.Location);  // 이동한 선 그리기
                        break;



                        //default:
                        //    using (Pen currentPen = GetPenForCurrentMode())
                        //    {
                        //        g.DrawLine(currentPen, prevPoint, e.Location);  // 이동한 선 그리기

                        //    }
                        //    break;
                }
                prevPoint = e.Location;     // 현재 좌표를 다음 출발점으로

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
                // 미리보기 캔버스 초기화
                previewCanvas = new Bitmap(drawingCanvas);

                using (Graphics g = Graphics.FromImage(previewCanvas))
                {

                    Point endPoint = e.Location;

                    // 네모 그리기
                    if (currentShapeMode == ShapeMode.Rectangle)
                    {
                        Rectangle rect = GetRectangle(startPoint, endPoint);
                        g.DrawRectangle(previewPen, rect);
                    }
                    // 세모 그리기
                    if (currentShapeMode == ShapeMode.Triangle)
                    {
                        Point[] triangle = GetTrianglePoints(startPoint, endPoint);
                        g.DrawPolygon(previewPen, triangle);
                    }
                    // 원 그리기
                    if (currentShapeMode == ShapeMode.Circle)
                    {
                        Rectangle ellipseRect = GetRectangle(startPoint, endPoint);
                        g.DrawEllipse(previewPen, ellipseRect);
                    }
                }
            }


            RenderToWhiteBoard();

        }
        // 마우스 업 이벤트
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
            //    MessageBox.Show("펜 색상이 비어있습니다. 기본값으로 설정합니다.");
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

        // 색상 버튼 클릭
        private void BtnColor_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]색상버튼 클릭");
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                penColor = DlgColor.Color;
                UpdatePreviewPen(penColor);

                if (currentPenMode == ToolMode.Brush)
                    img = TintImage(texture, penColor);
            }
        }

        // 그림 전체 초기화, undo, redo도 초기화
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


            RenderToWhiteBoard();  // 3. 최종 화면 다시 보여주기
            UpdateButtonsEnable();
        }

        // 지우개 버튼 클릭
        private void BtnEraser_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]지우개버튼 클릭");
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

            RenderToWhiteBoard();  // 3. 최종 화면 다시 보여주기

        }

        private void BtnAllDrawClear_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]전체지우기버튼 클릭");
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
            // (?) 여긴 잘 이해가 안되니까 다시 찾아보기
            // 1. 그림 그린 캔버스 초기화
            using (Graphics g = Graphics.FromImage(drawingCanvas))
            {
                g.Clear(Color.Transparent); // 또는 backgroundColor 써도 OK
            }

            // 2. 배경도 다시 초기화해야 화면이 완전히 바뀜
            using (Graphics g = Graphics.FromImage(backgroundCanvas))
            {
                g.Clear(backgroundColor);
            }

            RenderToWhiteBoard();  // 3. 최종 화면 다시 보여주기

        
        }

        // 배경 선택 버튼
        private void BtnBackgroundColor_Click(object sender, EventArgs e)
        {
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("[DEBUG]배경색버튼 클릭");
                backgroundColor = DlgColor.Color;
                //WhiteBoard.BackColor = backgroundColor;
                //WhiteBoard.Invalidate();            // 화면 갱신

                using (Graphics g = Graphics.FromImage(backgroundCanvas))
                {
                    g.Clear(backgroundColor);
                }
                RenderToWhiteBoard();
            }
        }

        // 되돌리기 버튼
        private void BtnUndo_Click(object sender, EventArgs e)
        {
            Console.WriteLine(undoStack.Count.ToString());
            if (undoStack.Count > 0)
            {
                //BtnRedo.Enabled = true;
                Console.WriteLine("[DEBUG]undo버튼 클릭");
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

                Console.WriteLine("[DEBUG]redo버튼 클릭");
                undoStack.Push(new Bitmap(drawingCanvas));

                drawingCanvas = redoStack.Pop();
                g = Graphics.FromImage(drawingCanvas);
                RenderToWhiteBoard();
            }
            UpdateButtonsEnable();
            Console.WriteLine($"[DEBUG] UndoStack: {undoStack.Count}, RedoStack: {redoStack.Count}");

        }

        // 도형 그리기 버튼
        private void BtnRectangle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]네모버튼 클릭");
            currentShapeMode = ShapeMode.Rectangle;
            isFull = false;
        }

        private void BtnTriangle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]세모버튼 클릭");
            currentShapeMode = ShapeMode.Triangle;
            isFull = false;
        }

        private void BtnCircle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]원버튼 클릭");
            currentShapeMode = ShapeMode.Circle;
            isFull = false;
        }

        // 트렉바 이벤트
        private void TrbPenSize_ValueChanged(object sender, EventArgs e)
        {
            penSize = TrbPenSize.Value;
            LblPenSize.Text = "펜 크기: " + penSize.ToString();
            Console.WriteLine($"[DEBUG] PenSize:{penSize}");

        }

        private void TrbEraserSize_ValueChanged(object sender, EventArgs e)
        {
            eraserSize = TrbEraserSize.Value;
            LblEraserSize.Text = "지우개 크기: " + eraserSize.ToString();
            Console.WriteLine($"[DEBUG] EraserSize:{eraserSize}");
        }

        // 펜 종류 선택 버튼
        private void BtnPen_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]펜버튼 클릭");
            currentPenMode = ToolMode.Pen;
            currentShapeMode = ShapeMode.None;
        }
        private void BtnBrush_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]브러쉬버튼 클릭");
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
                ofd.Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imageToInsert = new Bitmap(ofd.FileName); // 임시 보관
                    isInsertingImage = true;
                    MessageBox.Show("이미지를 넣을 위치를 드래그로 지정하세요.");
                }
            }
        }

        
    }
}
