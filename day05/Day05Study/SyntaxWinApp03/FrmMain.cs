using System.CodeDom;
using System.Security.Cryptography;

namespace SyntaxWinApp03
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
        // async �񵿱�� �����ϴ� �޼����� ����
        // async, await�� �׻� ���� ���
        private async void BtnStart_Click(object sender, EventArgs e)
        {
            // * : UI �����尡 ����
            LblCurrState.Text = "������� : ����";        // UI ó��
            BtnStart.Text = "������";                     // UI ó��
            BtnStart.Enabled = false; // ������ ��        // UI ó��


            long MaxVal = 200;
            long total = 0;
            PrgProcess.Minimum = 0;                        // *
            PrgProcess.Maximum = 100;                      // *

            // await�� �񵿱� ���
            await Task.Run(() => {
                for (int i = 0; i < MaxVal; i++)
                {
                    total += i % 3;

                    int progress = (int)((i * 100) / MaxVal) +1;
                    Console.WriteLine(progress.ToString());
                    // Task.Run �� ���� UIó�� ������
                    this.Invoke(new Action(() => 
                    {
                        TxtLog.Text += i.ToString() + "\r\n";
                        TxtLog.SelectionStart = TxtLog.Text.Length;
                        TxtLog.ScrollToCaret(); // ��ũ���� ���� ������
                        PrgProcess.Value = progress;        // *
                    }));
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
            });

            LblCurrState.Text = "������� : ����";    // *
            BtnStart.Text = "����";                   // *
            BtnStart.Enabled = true; // ������ ��     // *

        }

        
    }
}
