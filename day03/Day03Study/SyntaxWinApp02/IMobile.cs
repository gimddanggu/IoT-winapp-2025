using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxWinApp02
{
    // 인터페이스를 구현(상속의 일종)하는 클래스에서 메서드 재정의
    internal interface IMobile
    {
        void Call(); // 구현이 없음
        string GetList();
    }
}
