namespace PaintBoard
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        Bitmap canvas;
        Graphics g;
        Pen drawPen = new Pen(Color.Black, 2);
        Pen eraserPen = new Pen(Color.White, 2);

        bool isDrawing = false;
        bool isEraseMode = false;
        Color backgroundColor = Color.White;
        Point prevPoint;



        private void FrmMain_Load(object sender, EventArgs e)
        {
            canvas = new Bitmap(WhiteBoard.Width, WhiteBoard.Height);
            g = Graphics.FromImage(canvas);
            g.Clear(Color.White);
            WhiteBoard.Image = canvas;
        }

        private void WhiteBoard_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            prevPoint = e.Location;
        }

        private void WhiteBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                Pen penToUse = isEraseMode ? eraserPen : drawPen;
                g.DrawLine(penToUse, prevPoint, e.Location);  // �̵��� �� �׸���
                prevPoint = e.Location;     // ���� ��ǥ�� ���� ���������
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
            g.Clear(backgroundColor);
            WhiteBoard.Invalidate();

        }

        private void BtnEraser_Click(object sender, EventArgs e)
        {
            isEraseMode = true;
        }


        private void BtnPen_Click(object sender, EventArgs e)
        {
            isEraseMode = false;
        }

        private void BtnBackgroundColor_Click(object sender, EventArgs e)
        {
            if (DlgColor.ShowDialog() == DialogResult.OK)
            {
                backgroundColor = DlgColor.Color;
                g.Clear(backgroundColor);           // ��� ���� ����
                eraserPen.Color = backgroundColor;  // ���찳 ���� ������ �����ϰ�
                WhiteBoard.Invalidate();            // ȭ�� ����
            }
        }
    }
}
