using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KiwoomExample
{
    public partial class Form1 : Form
    {
        private string SCREEN_NO_ACCOUNT_INFO = "1001";

        private string REALTIME_NEW = "0";
        private string REALTIME_ADD = "1";

        private BindingList<Holding> holdings;

        private LoginInfo loginInfo;

        public Form1()
        {
            InitializeComponent();

            holdings = new BindingList<Holding>();
            dataHolding.DataSource = holdings;
        }

        //***********************************************************************************************************************
        // Form 이벤트 선언부
        //***********************************************************************************************************************
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 로그아웃 기능이 없어져서 로그인만 처리
            Button clickedButton = (Button)sender;

            // 클릭 시 버튼 비활성화
            clickedButton.Enabled = false;

            // 로그인창 오픈
            if (kiwoomApi.CommConnect() == 0)
            {
                listBoxLog.Items.Add("로그인 창이 열렸습니다.");
            } else
            {
                listBoxLog.Items.Add("로그인 창을 열지 못했습니다.");

                // 로그인창 오픈 실패 시 다시 버튼 활성화
                clickedButton.Enabled = true;
            }
        }

        private void comboAccountList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox changedComboBox = (ComboBox)sender;
            if(!string.IsNullOrEmpty(changedComboBox.SelectedItem.ToString())) {
                requestTrAccountBalance();
            }
        }

        //***********************************************************************************************************************
        // 공통 함수 선언부
        //***********************************************************************************************************************
        private string convertMoneyFormat(string price)
        {
            return String.Format("{0:#,###0}", long.Parse(price));
        }

        private string convertPercentFormat(string percent)
        {
            return String.Format("{0:f2}", double.Parse(percent));
        }

        private string getSeletedAccountNo()
        {
            return comboAccountList.SelectedItem.ToString().Trim();
        }

        private void initAccountList()
        {
            int cnt = int.Parse(kiwoomApi.GetLoginInfo("ACCOUNT_CNT"));

            if (cnt > 0)
            {
                comboAccountList.Items.AddRange(kiwoomApi.GetLoginInfo("ACCLIST").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));

                comboAccountList.SelectedIndex = 0;
            }
        }

        private long getBuyFee(long totalPrice)
        {
            /**
             * 모의 매수 수수료 : 매수총액*0.35%
                실제 매수 수수료 : 매수총액*0.015%
             */
            double fee = 0;

            if (loginInfo.getServerGubun == 0)
            {
                // 모의투자
                fee += totalPrice * 0.35;
            } else
            {
                // 실투자
                fee += totalPrice * 0.015;
            }

            return long.Parse(Math.Ceiling(fee).ToString());
        }

        private long getSellFee(long totalPrice)
        {
            /**
                모의 매도 수수료 : 매도총액*0.35%
                모의 매도 세금: 매도총액*0.25%

                실제 매도 수수료 : 매도총액*0.015%
                실제 매도 세금: 매도총액*0.25%
             */
            double fee = 0;

            if (loginInfo.getServerGubun == 0)
            {
                // 모의투자
                fee += totalPrice * 0.35;
                fee += totalPrice * 0.25;
            }
            else
            {
                // 실투자
                fee += totalPrice * 0.015;
                fee += totalPrice * 0.25;
            }

            Console.WriteLine("fee : " + fee);

            return long.Parse(Math.Ceiling(fee).ToString());
        }

        //***********************************************************************************************************************
        // 키움 OpenApi 기본함수 선언부
        //***********************************************************************************************************************
        private void getLoginInfo()
        {
            loginInfo = new LoginInfo();
            loginInfo.accountCnt = int.Parse(kiwoomApi.GetLoginInfo("ACCOUNT_CNT").Trim());
            loginInfo.accList = kiwoomApi.GetLoginInfo("ACCLIST").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            loginInfo.userId = kiwoomApi.GetLoginInfo("USER_ID").Trim();
            loginInfo.userName = kiwoomApi.GetLoginInfo("USER_NAME").Trim();
            loginInfo.keyBsecgb = int.Parse(kiwoomApi.GetLoginInfo("KEY_BSECGB").Trim());
            loginInfo.firewSecgb = int.Parse(kiwoomApi.GetLoginInfo("FIREW_SECGB").Trim());
            loginInfo.getServerGubun = int.Parse(kiwoomApi.GetLoginInfo("GetServerGubun").Trim());
        }

        //***********************************************************************************************************************
        // 키움 OpenApi TR요청 선언부
        //***********************************************************************************************************************
        private void requestTrEstimatedAssets()
        {
            kiwoomApi.SetInputValue("계좌번호", getSeletedAccountNo());
            kiwoomApi.SetInputValue("비밀번호", "");
            kiwoomApi.SetInputValue("상장폐지조회구분", "0");

            // TR명 : 추정자산조회요청
            kiwoomApi.CommRqData("trEstimatedAssets", "OPW00003", 0, SCREEN_NO_ACCOUNT_INFO);
        }

        private void requestTrAccountBalance()
        {
            // opw00018 : 계좌평가잔고내역요청
            kiwoomApi.SetInputValue("계좌번호", getSeletedAccountNo());
            kiwoomApi.SetInputValue("비밀번호", "");
            kiwoomApi.SetInputValue("비밀번호입력매체구분", "00");
            kiwoomApi.SetInputValue("조회구분", "1");

            kiwoomApi.CommRqData("trAccountBalance", "opw00018", 0, SCREEN_NO_ACCOUNT_INFO);
        }

        private void requestRealtimeQuote(string scrNo, string codeList, string optType)
        {
            /*
             * 9001 : 종목코드,업종코드
             * 302 : 종목명
             * 10 : 현재가
             * 11 : 전일대비
             * 25 : 전일대비기호
             * 12 : 등락율
             * 13 : 누적거래량
             * 15 : 거래량
             * 20 : 체결시간
             */
            int ret = kiwoomApi.SetRealReg(scrNo, codeList, "9001;302;10;11;25;12;13;15;20", optType);

            if (ret < 0)
            {
                listBoxLog.Items.Add("[ERROR] 실시간시세요청 실패 - " + codeList + " (CODE : " + ret + ")");
            }
        }

        //***********************************************************************************************************************
        // 키움 OpenApi TR요청 결과 처리부
        //***********************************************************************************************************************
        private void trEstimatedAssets(AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            string estimatedAssets = kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "추정예탁자산").Trim();
            labelEstimatedAssetsVal.Text = convertMoneyFormat(estimatedAssets) + "원";
        }

        private void trAccountBalance(AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            totalPurchaseAmount.Text = String.Format("{0:#,###0}", long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "총매입금액")));
            totalEvaluationAmount.Text = String.Format("{0:#,###0}", long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "총평가금액")));
            totalValuationAmount.Text = String.Format("{0:#,###0}", long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "총평가손익금액")));
            totalYield.Text = String.Format("{0:f2}", double.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "총수익률(%)")));
            estimatedBalance.Text = String.Format("{0:#,###0}", long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "추정예탁자산")));

            int cnt = kiwoomApi.GetRepeatCnt(e.sTrCode, e.sRQName);

            holdings.Clear();

            for (int i = 0; i < cnt; i++)
            {
                string stockNo = kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "종목번호").Trim();
                string stockName = kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "종목명").Trim();
                long currentPrice = long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "현재가").Trim());
                int qty = int.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "보유수량").Trim());
                long buyPrice = long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "매입가").Trim());
                long totalBuyPrice = long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "매입금액").Trim());
                long profit = long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "평가손익").Trim());
                float profitRate = float.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "수익률(%)").Trim());

                holdings.Add(new Holding(stockNo, stockName, currentPrice, qty, buyPrice, totalBuyPrice, profit, profitRate, loginInfo.getServerGubun));
            }

            if (holdings.Count > 0)
            {
                string codeList = string.Join(";", holdings.Select(item => item.StockNo));

                // 실시간 시세 요청
                requestRealtimeQuote(SCREEN_NO_ACCOUNT_INFO, codeList, REALTIME_NEW);
            }
        }

        //***********************************************************************************************************************
        // 키움 OpenApi 이벤트 선언부
        //***********************************************************************************************************************
        private void kiwoomApi_OnReceiveMsg(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            Console.WriteLine("[DEBUG] 화면번호:" + e.sScrNo + ", 사용자구분명:" + e.sRQName + ", TR명:" + e.sTrCode + ", 메시지:" + e.sMsg);
        }

        private void kiwoomApi_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            listBoxLog.Items.Add("로그인 결과 (ErrCode:" + e.nErrCode + ")");

            // 로그인 성공
            if (e.nErrCode == 0)
            {
                // 계좌 비밀번호 입력창 띄움
                kiwoomApi.KOA_Functions("ShowAccountWindow", "");

                // 로그인 완료 후 처리 로직
                getLoginInfo();
                initAccountList();
                requestTrEstimatedAssets(); // 추정자산조회
            }
        }

        private void kiwoomApi_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            Console.WriteLine("RQName : " + e.sRQName);
            this.GetType().GetMethod(e.sRQName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).Invoke(this, new object[] { e });
        }

        private void kiwoomApi_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            // 보유 종목에 있을 경우 값 업데이트
            Holding holding = holdings.SingleOrDefault(item => item.StockNo.Contains(e.sRealKey));
            if(holding != null)
            {
                //Console.WriteLine(kiwoomApi.GetCommRealData(e.sRealKey, 10).Trim().Replace("-", ""));
                long currentPrice = long.Parse(kiwoomApi.GetCommRealData(e.sRealKey, 10).Trim().Replace("-", ""));
                //long calFee = getSellFee(long.Parse(holding.TotalBuyPrice, System.Globalization.NumberStyles.AllowThousands)) + getBuyFee(long.Parse(holding.TotalBuyPrice, System.Globalization.NumberStyles.AllowThousands));
                //long profit = (currentPrice - long.Parse(holding.BuyPrice, System.Globalization.NumberStyles.AllowThousands)) * long.Parse(holding.Qty, System.Globalization.NumberStyles.AllowThousands) - calFee;
                //Console.WriteLine("CurrentPrice : " + currentPrice + ", BuyPrice : " + long.Parse(holding.BuyPrice, System.Globalization.NumberStyles.AllowThousands) + ", QTY : " + long.Parse(holding.Qty, System.Globalization.NumberStyles.AllowThousands) + ", Fee : " + calFee + ", Profit : " + holding.Profit + ", calProfit : " + ((currentPrice - long.Parse(holding.BuyPrice, System.Globalization.NumberStyles.AllowThousands) * long.Parse(holding.Qty, System.Globalization.NumberStyles.AllowThousands)) - calFee));
                holding.CurrentPrice = String.Format("{0:#,###0}", currentPrice);
                //holding.Profit = String.Format("{0:#,###0}", profit);
                //holding.ProfitRate = String.Format("{0:f2}", long.Parse(holding.TotalBuyPrice, System.Globalization.NumberStyles.AllowThousands) / profit / 100);
            }
        }

        private void kiwoomApi_OnReceiveConditionVer(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {

        }

        private void kiwoomApi_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {

        }

        private void kiwoomApi_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {

        }

    }
}
