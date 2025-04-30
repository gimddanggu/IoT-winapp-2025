using System.Diagnostics.Eventing.Reader;
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
        enum DrawMode
        {
            Pen,
            Eraser,
            Circle,
            Rectangle,
            Triangle
        }

        DrawMode currentMode = DrawMode.Pen; // 기본 모드 : 펜

        Bitmap drawingCanvas;       // 그리기용 캔버스
        Bitmap previewCanvas;       // 도형 미리보기용 캔버스
        Bitmap backgroundCanvas;    // 배경 캔버스
        Graphics g;
        Pen drawPen = new Pen(Color.Black, 2);
        int eraserSize = 20;    // 기본 지우개 크기
        int penSize = 2;


        Stack<Bitmap> undoStack = new Stack<Bitmap>();  // 그림 되돌리기 기능을 위한 비트맵 스택 생성
        Stack<Bitmap> redoStack = new Stack<Bitmap>();  // 그림 되돌리기 기능을 위한 비트맵 스택 생성


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
            // 비트맵은 운영체제단에서 관리하고 있기 때문에 직접 접근하려면 LockBits를 사용해서 접근해야만 한다.
            BitmapData bmpData = drawingCanvas.LockBits(
                new Rectangle(0, 0, drawingCanvas.Width, drawingCanvas.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);


            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;     // 첫번재 픽셀의 포인터 ???

                for (int i = 0; i < eraseRect.Height; i++)
                {
                    int PosY = eraseRect.Y + i;
                    if (PosY < 0 || PosY > drawingCanvas.Height) continue; // 경계 밖 무시 

                    for (int j = 0; j < eraseRect.Width; j++)
                    {
                        int PosX = eraseRect.X + j;
                        if (PosX < 0 || PosX > drawingCanvas.Width) continue; // 경계 밖 무시

                        int offset = (PosY * bmpData.Stride) + (PosX * 4);       // stride ???

                        ptr[offset + 3] = 0;        // 알파 채널 (투명도)을 0으로 만든다 (완전 투명)

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
            Point top = new Point((p1.X + p2.X) / 2, Math.Min(p1.Y, p2.Y));         // 꼭짓점 좌표
            Point left = new Point(Math.Min(p1.X, p2.X), Math.Max(p1.Y, p2.Y));     // 왼쪽 아래 좌표
            Point right = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));       // 오른쪽 아래 좌표

            return new Point[] { top, left, right };

        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            int w = WhiteBoard.Width;
            int h = WhiteBoard.Height;

            drawingCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            previewCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb);    // 도형 미리보기를 위한 캔버스
            backgroundCanvas = new Bitmap(w, h, PixelFormat.Format32bppArgb); // 배경 캔버스

            using (Graphics g= Graphics.FromImage(backgroundCanvas))
            {
                g.Clear(Color.White);
            }
            // 초기 화면 표시
            RenderToWhiteBoard();

            // 깜빡임 방지용 더블 버퍼링
            this.DoubleBuffered = true;
        }

        private void WhiteBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentMode == DrawMode.Pen || currentMode == DrawMode.Eraser)
            {
                isDrawing = true;
                prevPoint = e.Location;
 
            }


            if (currentMode == DrawMode.Rectangle ||
                currentMode == DrawMode.Triangle ||
                currentMode == DrawMode.Circle)
            {
                isDrawing = true;
                startPoint = e.Location;
            }

            // 현재 drawingCanvas 복사해서 저장
            undoStack.Push(new Bitmap(drawingCanvas));

        }

        private void WhiteBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                if (currentMode == DrawMode.Eraser)
                {
                    EraseAt(e.X, e.Y);  // 여기서 투명 지우개 호출

                }
                else if (currentMode == DrawMode.Pen)
                {
                    using (Graphics g = Graphics.FromImage(drawingCanvas))
                    {
                        g.DrawLine(drawPen, prevPoint, e.Location);  // 이동한 선 그리기
                        prevPoint = e.Location;     // 현재 좌표를 다음 출발점으로
                    }
                    
                }
                else if (currentMode == DrawMode.Rectangle ||
                        currentMode == DrawMode.Circle ||
                        currentMode == DrawMode.Triangle)
                { 
                    // 미리보기 캔버스 초기화
                    previewCanvas = new Bitmap(drawingCanvas);

                    using (Graphics g = Graphics.FromImage(previewCanvas))
                    {
                        Pen previewPen = new Pen(Color.Gray, penSize)
                        {
                            DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
                        };

                        Point endPoint = e.Location;

                        // 네모 그리기
                        if (currentMode == DrawMode.Rectangle)
                        {
                            Rectangle rect = GetRectangle(startPoint, endPoint);
                            g.DrawRectangle(previewPen, rect);
                        }
                        // 세모 그리기
                        if (currentMode == DrawMode.Triangle)
                        {
                            Point[] triangle = GetTrianglePoints(startPoint, endPoint);
                            g.DrawPolygon(previewPen, triangle);
                        }
                        // 원 그리기
                        if (currentMode == DrawMode.Circle)
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

            using(Graphics g = Graphics.FromImage(drawingCanvas))
            {
                if (currentMode == DrawMode.Rectangle)
                    g.DrawRectangle(drawPen, GetRectangle(startPoint, endPoint));
                else if (currentMode == DrawMode.Triangle)
                    g.DrawPolygon(drawPen, GetTrianglePoints(startPoint, endPoint));
                else if (currentMode == DrawMode.Circle)
                    g.DrawEllipse(drawPen, GetRectangle(startPoint, endPoint));

            }

            previewCanvas = new Bitmap(drawingCanvas.Width, drawingCanvas.Height, PixelFormat.Format32bppArgb);  


            // undo 저장 // 마우스를 한번 때면 undo 스택에 저장
            //undoStack.Push(new Bitmap(canvas));

            RenderToWhiteBoard();
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]색상버튼 클릭");
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                drawPen.Color = DlgColor.Color;
            }
        }

        private void BtnAllClear_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]전체지우기버튼 클릭");
            undoStack.Push(new Bitmap(drawingCanvas));
            //g.Clear(backgroundColor);
            //WhiteBoard.Invalidate();
            //using (Graphics g = Graphics.FromImage(drawingCanvas))
            //{
            //    g.Clear(backgroundColor);
            //}
            //RenderToWhiteBoard();

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

        private void BtnEraser_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]지우개버튼 클릭");
            currentMode = DrawMode.Eraser;
        }


        private void BtnPen_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]펜버튼 클릭");
            currentMode = DrawMode.Pen;
        }

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

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                Console.WriteLine("[DEBUG]undo버튼 클릭");
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
                Console.WriteLine("[DEBUG]redo버튼 클릭");
                undoStack.Push(new Bitmap(drawingCanvas));
                drawingCanvas = redoStack.Pop();
                g = Graphics.FromImage(drawingCanvas);
                RenderToWhiteBoard();
            }
        }

        private void BtnRectangle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]네모버튼 클릭");
            currentMode = DrawMode.Rectangle;
        }

        private void BtnTriangle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]세모버튼 클릭");
            currentMode = DrawMode.Triangle;
        }

        private void BtnCircle_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[DEBUG]원버튼 클릭");
            currentMode = DrawMode.Circle;
        }
    }
}
