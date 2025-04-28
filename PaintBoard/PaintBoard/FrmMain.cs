using System.Drawing.Imaging;

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

        Bitmap canvas;
        Graphics g;
        Pen drawPen = new Pen(Color.Black, 2);
        int eraserSize = 20;    // 기본 지우개 크기
        int penSize = 2;

        Stack<Bitmap> undoStack = new Stack<Bitmap>();  // 그림 되돌리기 기능을 위한 비트맵 스택 생성
        Stack<Bitmap> redoStack = new Stack<Bitmap>();  // 그림 되돌리기 기능을 위한 비트맵 스택 생성


        bool isDrawing = false;

        Color backgroundColor = Color.White;
        Point prevPoint;

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
            BitmapData bmpData = canvas.LockBits(
                new Rectangle(0, 0, canvas.Width, canvas.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);


            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;     // 첫번재 픽셀의 포인터 ???

                for (int i = 0; i < eraseRect.Height; i++)
                {
                    int PosY = eraseRect.Y + i;
                    if (PosY < 0 || PosY > canvas.Height) continue; // 경계 밖 무시 

                    for (int j = 0; j < eraseRect.Width; j++)
                    {
                        int PosX = eraseRect.X + j;
                        if (PosX < 0 || PosX > canvas.Width) continue; // 경계 밖 무시

                        int offset = (PosY * bmpData.Stride) + (PosX * 4);       // stride ???

                        ptr[offset + 3] = 0;        // 알파 채널 (투명도)을 0으로 만든다 (완전 투명)

                    }
                }
            }

            canvas.UnlockBits(bmpData);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            WhiteBoard.BackColor = Color.White;
            canvas = new Bitmap(WhiteBoard.Width, WhiteBoard.Height, PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(canvas);
            g.Clear(Color.Transparent);
            WhiteBoard.Image = canvas;

        }

        private void WhiteBoard_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            prevPoint = e.Location;

            // 현재 drawingCanvas 복사해서 저장
            undoStack.Push(new Bitmap(canvas));
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
                    g.DrawLine(drawPen, prevPoint, e.Location);  // 이동한 선 그리기
                    prevPoint = e.Location;     // 현재 좌표를 다음 출발점으로
                }
                else if (currentMode == DrawMode.Rectangle) { }
                else if (currentMode == DrawMode.Circle) { }
                else if (currentMode == DrawMode.Triangle) { }

                WhiteBoard.Image = canvas;
                WhiteBoard.Invalidate();

            }
        }

        private void WhiteBoard_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                drawPen.Color = DlgColor.Color;
            }
        }

        private void BtnAllClear_Click(object sender, EventArgs e)
        {
            undoStack.Push(new Bitmap(canvas));
            g.Clear(backgroundColor);
            WhiteBoard.Invalidate();

        }

        private void BtnEraser_Click(object sender, EventArgs e)
        {
            currentMode = DrawMode.Eraser;
        }


        private void BtnPen_Click(object sender, EventArgs e)
        {
            currentMode = DrawMode.Pen;
        }

        private void BtnBackgroundColor_Click(object sender, EventArgs e)
        {
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                backgroundColor = DlgColor.Color;
                WhiteBoard.BackColor = backgroundColor;
                WhiteBoard.Invalidate();            // 화면 갱신
            }
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(new Bitmap(canvas));
                canvas = undoStack.Pop();
                g = Graphics.FromImage(canvas);
                WhiteBoard.Image = canvas;
                WhiteBoard.Invalidate();
            }
        }

        private void BtnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(new Bitmap(canvas));
                canvas = redoStack.Pop();
                g = Graphics.FromImage(canvas);
                WhiteBoard.Image = canvas;
                WhiteBoard.Invalidate();
            }
        }

        private void BtnRectangle_Click(object sender, EventArgs e)
        {
            currentMode = DrawMode.Rectangle;
        }

        private void BtnTriangle_Click(object sender, EventArgs e)
        {
            currentMode = DrawMode.Triangle;
        }

        private void BtnCircle_Click(object sender, EventArgs e)
        {
            currentMode = DrawMode.Circle;
        }
    }
}
