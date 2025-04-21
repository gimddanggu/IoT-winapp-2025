namespace SyntaxWinApp04
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnMsg_Click(object sender, EventArgs e)
        {
            if (TxtName.Text =="" || TxtAge.Text == "")
            {
                MessageBox.Show("값을 채워주세요.");
                return;

            } else
            {
                LblResult.Text = "처리결과..";
                TxtResult.Text = "뭔가 처리가 될 것임";
            }
        }
    }
}
