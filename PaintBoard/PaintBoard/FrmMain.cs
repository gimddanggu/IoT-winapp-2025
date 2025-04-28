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

        DrawMode currentMode = DrawMode.Pen; // �⺻ ��� : ��

        Bitmap canvas;
        Graphics g;
        Pen drawPen = new Pen(Color.Black, 2);
        int eraserSize = 20;    // �⺻ ���찳 ũ��
        int penSize = 2;

        Stack<Bitmap> undoStack = new Stack<Bitmap>();  // �׸� �ǵ����� ����� ���� ��Ʈ�� ���� ����
        Stack<Bitmap> redoStack = new Stack<Bitmap>();  // �׸� �ǵ����� ����� ���� ��Ʈ�� ���� ����


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
            // ��Ʈ���� �ü���ܿ��� �����ϰ� �ֱ� ������ ���� �����Ϸ��� LockBits�� ����ؼ� �����ؾ߸� �Ѵ�.
            BitmapData bmpData = canvas.LockBits(
                new Rectangle(0, 0, canvas.Width, canvas.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);


            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;     // ù���� �ȼ��� ������ ???

                for (int i = 0; i < eraseRect.Height; i++)
                {
                    int PosY = eraseRect.Y + i;
                    if (PosY < 0 || PosY > canvas.Height) continue; // ��� �� ���� 

                    for (int j = 0; j < eraseRect.Width; j++)
                    {
                        int PosX = eraseRect.X + j;
                        if (PosX < 0 || PosX > canvas.Width) continue; // ��� �� ����

                        int offset = (PosY * bmpData.Stride) + (PosX * 4);       // stride ???

                        ptr[offset + 3] = 0;        // ���� ä�� (����)�� 0���� ����� (���� ����)

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

            // ���� drawingCanvas �����ؼ� ����
            undoStack.Push(new Bitmap(canvas));
        }

        private void WhiteBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                if (currentMode == DrawMode.Eraser)
                {
                    EraseAt(e.X, e.Y);  // ���⼭ ���� ���찳 ȣ��
                }
                else if (currentMode == DrawMode.Pen)
                {
                    g.DrawLine(drawPen, prevPoint, e.Location);  // �̵��� �� �׸���
                    prevPoint = e.Location;     // ���� ��ǥ�� ���� ���������
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
                WhiteBoard.Invalidate();            // ȭ�� ����
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
