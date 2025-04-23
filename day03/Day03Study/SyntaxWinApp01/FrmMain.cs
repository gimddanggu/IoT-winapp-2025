namespace SyntaxWinApp01
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            // 기본 생성자
            Person hugo = new Person();
            hugo.Name = TxtName.Text.Trim();
            hugo.Age = int.Parse(TxtAge.Text.Trim());
            
            hugo.Gender = char.Parse(TxtGender.Text.Trim());
            hugo.Phone = TxtPhone.Text.Trim();

            // 매개변수 생성자
            Person ashely = new Person("애슐리", 42, 'F', "010-9839-7777");

            TxtResult.Text = hugo.ToString();
            hugo.GetUp();
            hugo.GoToSchool();
            
            // static일 경우는 객체를 생성하지 않음
            Person.GetNumber();
        }
    }
}
