namespace SyntaxWinApp01
{
    public partial class FrmMain : Form
    {
        // �븮��(delegate) ����
        delegate void MyDelegate(string msg);
        // �̺�Ʈ�ڵ鷯 �븮�� ����
        delegate void MyEventHandler(object sender, EventArgs e);
        // �̺�Ʈ�� ����
        public event EventHandler somthingHappend;

        // �븮�ڿ��� ȣ���� �޼��� - �븮�ڿ� �Ķ���� ��ġ�ؾߵ�
        void SayHello(string msg)
        {
            MessageBox.Show($"�ȳ�{msg}", "�޽���",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        void SayGoodbye(string msg)
        {
            MessageBox.Show($"�߰�{msg}", "�޽���",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        public FrmMain()
        {
            InitializeComponent();
            TxtLog.Text += ("1. �� ������ ����\r\n");
            Console.WriteLine("1. �� ������ ����");

        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            // 1. ����ȣ��
            SayHello("HHEELLOO!!");
            SayGoodbye("HHEELLOO!!");


            // 2. �븮��(delegate) ȣ�� (���� ������ �ٽ�!!)
            MyDelegate del = SayHello; // �븮�ڿ� ȣ���� �޼��带 ������
            del += SayGoodbye; // �߰��� ���� (�븮�� ü�� �߰�)

            del -= SayHello; // �߰��� ���� (�븮�� ü�� ����)
            del("terra~!~!~~!~");

            // 3. ��ȯ�� ���� �븮�� - Action
            Action<string> act = SayHello;
            act("cass");

            // 4. ��ȯ��  �ִ� �븮�� - Func
            Func<int, int, int> add = (a, b) => a + b;
            int result = add(7, 8);
            Console.WriteLine(result);

            if (somthingHappend != null)
            {
                somthingHappend(this, new EventArgs());
            }
        }

        private void FrmMain_Load(object sender, EventArgs e) {
            TxtLog.Text += ("2. ���ε� �̺�Ʈ ����\r\n");
            Console.WriteLine("2. ���ε� �̺�Ʈ ����");
        } 
        private void FrmMain_Activated(object sender, EventArgs e) { 
            TxtLog.Text += ("3. ����Ƽ����Ʈ �̺�Ʈ ����\r\n");
            Console.WriteLine("3. ����Ƽ����Ʈ �̺�Ʈ ����");
        }
        private void FrmMain_Shown(object sender, EventArgs e) {
            TxtLog.Text += ("4. ���� �̺�Ʈ ����\r\n");
            Console.WriteLine("4. ���� �̺�Ʈ ����");
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            Console.WriteLine("5. ��Ŭ��¡ �̺�Ʈ ����");
        }
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) {
            Console.WriteLine("6. ��Ŭ����� �̺�Ʈ ����");
        }

    }
}
