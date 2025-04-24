namespace SyntaxWinApp01
{
    public partial class FrmMain : Form
    {
        // 대리자(delegate) 선언
        delegate void MyDelegate(string msg);
        // 이벤트핸들러 대리자 선언
        delegate void MyEventHandler(object sender, EventArgs e);
        // 이벤트를 선언
        public event EventHandler somthingHappend;

        // 대리자에서 호출할 메서드 - 대리자와 파라미터 일치해야됨
        void SayHello(string msg)
        {
            MessageBox.Show($"안녕{msg}", "메시지",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        void SayGoodbye(string msg)
        {
            MessageBox.Show($"잘가{msg}", "메시지",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        public FrmMain()
        {
            InitializeComponent();
            TxtLog.Text += ("1. 폼 생성자 실행\r\n");
            Console.WriteLine("1. 폼 생성자 실행");

        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            // 1. 직접호출
            SayHello("HHEELLOO!!");
            SayGoodbye("HHEELLOO!!");


            // 2. 대리자(delegate) 호출 (윈앱 개발의 핵심!!)
            MyDelegate del = SayHello; // 대리자에 호출할 메서드를 묶어줌
            del += SayGoodbye; // 추가도 가능 (대리자 체인 추가)

            del -= SayHello; // 추가도 가능 (대리자 체인 삭제)
            del("terra~!~!~~!~");

            // 3. 반환값 없는 대리자 - Action
            Action<string> act = SayHello;
            act("cass");

            // 4. 반환값  있는 대리자 - Func
            Func<int, int, int> add = (a, b) => a + b;
            int result = add(7, 8);
            Console.WriteLine(result);

            if (somthingHappend != null)
            {
                somthingHappend(this, new EventArgs());
            }
        }

        private void FrmMain_Load(object sender, EventArgs e) {
            TxtLog.Text += ("2. 폼로드 이벤트 실행\r\n");
            Console.WriteLine("2. 폼로드 이벤트 실행");
        } 
        private void FrmMain_Activated(object sender, EventArgs e) { 
            TxtLog.Text += ("3. 폼액티베이트 이벤트 실행\r\n");
            Console.WriteLine("3. 폼액티베이트 이벤트 실행");
        }
        private void FrmMain_Shown(object sender, EventArgs e) {
            TxtLog.Text += ("4. 폼숀 이벤트 실행\r\n");
            Console.WriteLine("4. 폼숀 이벤트 실행");
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            Console.WriteLine("5. 폼클로징 이벤트 실행");
        }
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) {
            Console.WriteLine("6. 폼클로즈드 이벤트 실행");
        }

    }
}
