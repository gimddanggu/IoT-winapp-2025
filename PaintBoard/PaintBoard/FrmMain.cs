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
                g.DrawLine(penToUse, prevPoint, e.Location);  // 이동한 선 그리기
                prevPoint = e.Location;     // 현재 좌표를 다음 출발점으로
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
                g.Clear(backgroundColor);           // 배경 색상 변경
                eraserPen.Color = backgroundColor;  // 지우개 색상 배경색과 동일하게
                WhiteBoard.Invalidate();            // 화면 갱신
            }
        }
    }
}
