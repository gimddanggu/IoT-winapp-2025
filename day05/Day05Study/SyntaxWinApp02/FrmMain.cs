namespace SyntaxWinApp02
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        int add(int x, int y)
        {
            return x + y;
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            int result = add(10, 5);

            // 대리자 = 람다식
            Func<int, int, int> add2 = (x, y) => x + y;
            TxtLog.Text += result + "\r\n";
            TxtLog.Text += add2(10, 6) + "\r\n";

            // sayHello 함수 생성대신
            Action<string> sayHello = name => MessageBox.Show($"안녕, {name}", "인사",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            sayHello("테라!!!");

            // LINQ 사용 이전
            List<int> resList = new List<int>();
            List<int> numbers = [4, 10, 1, 5, 9, 8, 2, 3, 7];

            // 짝수만 추출해서 오름차순 정렬하는 로직
            foreach (int n in numbers)
            {
                if (n % 2 == 0) // 2로 나눠서 나머지 0이면 짝수
                {
                    resList.Add(n);
                }
            }

            TxtLog.Text += "전통 짝수리스트 > " + string.Join(" ", resList) + "\r\n";

            resList.Sort();

            TxtLog.Text += "전통 정렬리스트 > " + string.Join(" ", resList) + "\r\n";

            // 기본 LINQ 방식 > 3줄로 위의 전통방식을 처리
            numbers = [14, 20, 11, 15, 18, 19, 16, 13, 17];
            var resList2 = from n in numbers
                           where n % 2 == 0
                           orderby n
                           select n;

            TxtLog.Text += "링큐 정렬리스트 > " + string.Join(" ", resList2) + "\r\n";

            // LINQ Method Chaining 
            numbers = [24, 30, 21, 25, 28, 29, 26, 23, 27];
            var resList3 = numbers.Where(n => n % 2 == 0).OrderBy(n => n);
            TxtLog.Text += "링큐2 정렬리스트 > " + string.Join(" ", resList3) + "\r\n";


        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // UI 초기화
            TxtTest.PlaceholderText = "테스트입니다.";
            TxtTest.Size = new Size(350, 23);
            TxtTest.KeyPress += TxtTest_KeyPress; // Designer에서 작업하는 것과 동일
            TxtTest.Font = new Font("휴먼매직체", 12, FontStyle.Italic);

        }

        private void TxtTest_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {  // (char)Keys.Enter 과 동일      
                MessageBox.Show("엔터를 클릭했습니다.", "키프레스",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGeneric_Click(object sender, EventArgs e)
        {
            // 기본
            Print<int>(100);
            Print<float>(3.312343f);
            Print<string>("하이요");
            Print<bool>(false);

            // 생략가능
            Print(200);
            Print(432.21f);
            Print("잘가라");
            Print(true);

            // 제네릭 클래스 사용
            Box<int> intBox = new Box<int>();
            intBox.value = 300;
            intBox.Show();

            Box<string> stringBox = new Box<string>();
            stringBox.value = "김테라";
            stringBox.Show();
        }
        public void Print<T>(T data) { Console.WriteLine(data); }
    }

    // 제네릭 클래스
    public class Box<T> // where T : struct
    {
        public T value {  get; set; } // 속성

        public void Show()
        {
            Console.WriteLine($"Box의 값 : {value}" ,"Box값",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
