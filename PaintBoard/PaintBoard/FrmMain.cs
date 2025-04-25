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
        Pen currentPen = new Pen(Color.Black, 2);
        bool isDrawing = false;
        Point prevPoint;



        private void FrmMain_Load(object sender, EventArgs e)
        {
            canvas = new Bitmap(WhiteBoard.Width, WhiteBoard.Height);
            g = Graphics.FromImage(canvas);
            g.Clear(Color.White);
            WhiteBoard.Image = canvas;
        }
    }
}
