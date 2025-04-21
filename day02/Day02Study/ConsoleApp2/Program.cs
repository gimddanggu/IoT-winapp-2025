namespace ConsoleApp2
{
    internal class Program
    {
        // 3가지 주석 종류 - 한줄 주석, 여러줄 주석, xml 주석 

        /// <summary>
        ///  주석 - xml 주석,  회사에서 소스코드 자동 문서 생성시 사용
        ///
        /// </summary>
        /// <param name="args">입력파라미터</param>
        /// 

        static void Main(string[] args)
        {
            //주석 - 한 줄 주석
            Console.WriteLine("Hello, World!");
            /** 주석 - 여러 줄 주석
             * 
             */
        }
    }
}
