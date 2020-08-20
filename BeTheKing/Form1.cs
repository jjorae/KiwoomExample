using KiwoomExample;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace BeTheKing
{
    public partial class Form1 : Form
    {
        /*
        해당 폼에서는 로그인 상태, 추정 자산, 당일 수익, 보유 종목(수동 시장가 매도 가능), 매매 로그, 동작중인 전략(On,Off 가능?)만 보여줌
        - 로그인 상태 : 로그인 전에는 로그인 전입니다. 로그인 후에는 실투자, 모의투자 정보 출력. 로그인 전에는 모든 UI 잠금
        - 로그인 후에는 추정 자산, 당일 수익이 바로 보이며 매매가 이루어질때마다 새로 고침이 되도록 처리
        - 보유 종목 : 보유 종목에 대해서는 실시간 시세가 반영되고, 수수료 및 세금을 반영한 실제 수익도 반영됨. 시장가 매도가 가능하도록 함

        - 동작중인 전략 상단에는 전략 추가가 가능하도록 버튼 제공
        - 등록되어 있는 전략을 리스트로 보여주고 현재 동작 중인지 확인 가능하도록 함
        - 전략에는 조건식 선택 포함. 조건식 없이 다른 정보를 조합하는 부분은 추후 구현
        - 매수/매도 전략 설정
        - 기본 설정 : 전략이 동작하는 시간 등 설정. 전략의 상태는 대기,예약,실행,종료 이며 최초 생성 시 대기로 생성.
        */

        public Form1()
        {
            InitializeComponent();

            KiwoomApi.Instance.LoginSuccessEvent += changeLoginStatus;
            KiwoomApi.Instance.AccountBalanceEvent += AccountBalance;
            KiwoomApi.Instance.openLoginDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void changeLoginStatus(Boolean flag)
        {
            if (flag)
            {
                addSystemLog("로그인되었습니다.");

                if(KiwoomApi.Instance.LoginInfo.getServerGubun == 0)
                {
                    toolStripStatusLabel2.Text = "실서버";
                } else
                {
                    toolStripStatusLabel2.Text = "모의서버";
                }

                toolStripStatusLabel1.Text = "로그인되었습니다.";
                button1.Enabled = true;
                button2.Enabled = true;

                addSystemLog("계좌번호를 가져옵니다.");
                comboAccountList.Items.AddRange(KiwoomApi.Instance.initAccountList());
                comboAccountList.SelectedIndex = 0;
                //KiwoomApi.Instance.requestTrAccountBalance();

                dataGridView1.DataSource = KiwoomApi.Instance.Holdings;
            }
            else
            {
                addSystemLog("로그인에 실패했습니다.");
                toolStripStatusLabel1.Text = "로그인 전입니다.";
            }
        }

        private void AccountBalance(long totalValuationAmount, long estimatedBalance)
        {
            addSystemLog("계좌(" + comboAccountList.SelectedItem.ToString() + ") 정보를 업데이트 합니다.");
            this.totalValuationAmount.Text = String.Format("{0:#,###0}", totalValuationAmount);
            this.estimatedBalance.Text = String.Format("{0:#,###0}", estimatedBalance);
        }

        private void addSystemLog(string log)
        {
            textBox2.AppendText("[" + DateTime.Now + "] " + log + "\r\n");
        }

        private void addTradeLog(string log)
        {
            textBox1.AppendText("[" + DateTime.Now + "] " + log + "\r\n");
        }

        private void comboAccountList_SelectedIndexChanged(object sender, EventArgs e)
        {
            addSystemLog("계좌(" + comboAccountList.SelectedItem.ToString() + ") 정보를 요청 합니다.");
            KiwoomApi.Instance.requestTrAccountBalance(comboAccountList.SelectedItem.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KiwoomApi.Instance.runTrading();
        }
    }
}
