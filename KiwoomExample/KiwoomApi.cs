using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxKHOpenAPILib;

namespace KiwoomExample
{
    // 해당 클래스에서 비즈니스 로직이 모두 수행되고..Form에서는 UI만 가질 수 있게 하면 여러 프로그램에 사용 가능할듯.
    public sealed class KiwoomApi : Control
    {
        private bool isRunning = false;
        private Strategy strategy = null;

        private string SCREEN_NO_ACCOUNT_INFO = "1001";
        private string SCREEN_NO_ORDER = "2001";
        private string SCREEN_NO_SEARCH_RESULT = "3001";
        private string SCREEN_NO_CONDITION = "4001";

        private string REALTIME_NEW = "0";
        private string REALTIME_ADD = "1";

        private int ORDER_TYPE_BUY = 1;
        private int ORDER_TYPE_SELL = 2;
        private int ORDER_TYPE_BUY_CANCEL = 3;
        private int ORDER_TYPE_SELL_CANCEL = 3;
        private string ORDER_HOGA_MARKET = "03";
        private string ORDER_HOGA_LIIMIT = "00";

        private static KiwoomApi instance = null;
        private AxKHOpenAPI openApi = null;
        private LoginInfo loginInfo = null;

        private BindingList<Holding> holdings = null;
        private List<Order> orders = null;
        private List<ConditionStock> conditionStocks = null;
        private List<Stock> stocks = null;
        public string[] conditions = null;

        public delegate void LoginSuccessEventHandler(bool flag);
        public event LoginSuccessEventHandler LoginSuccessEvent;

        public delegate void AccountBalanceEventHandler(long totalValuationAmount, long estimatedBalance);
        public event AccountBalanceEventHandler AccountBalanceEvent;

        private KiwoomApi()
        {
            this.openApi = new AxKHOpenAPI();
            ((System.ComponentModel.ISupportInitialize)(this.openApi)).BeginInit();
            this.SuspendLayout();

            this.initEvent();

            this.Controls.Add(this.openApi);

            ((System.ComponentModel.ISupportInitialize)(this.openApi)).EndInit();
            this.ResumeLayout(false);

            holdings = new BindingList<Holding>();
            orders = new List<Order>();
            conditionStocks = new List<ConditionStock>();
            stocks = new List<Stock>();
        }

        public static KiwoomApi Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new KiwoomApi();
                }

                return instance;
            }
        }

        //---------------------------------------------------------- Request  ----------------------------------------------------------
        public void requestTrAccountBalance(string account)
        {
            // opw00018 : 계좌평가잔고내역요청
            openApi.SetInputValue("계좌번호", account);
            openApi.SetInputValue("비밀번호", "");
            openApi.SetInputValue("비밀번호입력매체구분", "00");
            openApi.SetInputValue("조회구분", "1");

            openApi.CommRqData("trAccountBalance", "opw00018", 0, SCREEN_NO_ACCOUNT_INFO);
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
            int ret = openApi.SetRealReg(scrNo, codeList, "9001;302;10;11;25;12;13;15;20;920", optType);

            if (ret < 0)
            {
                Console.WriteLine("[ERROR] 실시간시세요청 실패 - " + codeList + " (CODE : " + ret + ")");
            }
        }

        private void removeRealtimeQuote(string scrNo, string codeList)
        {
            openApi.SetRealRemove(scrNo, codeList);
        }

        private void requestTrBasicInformation(string code, string scrNo)
        {
            openApi.SetInputValue("종목코드", code);

            openApi.CommRqData("trBasicInformation", "opt10001", 0, scrNo);
        }

        private void loadConditions()
        {
            if (openApi.GetConditionLoad() == 0)
            {
                Console.WriteLine("[ERROR] 조건검색 로딩 실패");
            }
        }

        private void requestCondition()
        {
            conditionStocks.Clear();

            string[] conditionArr = strategy.condition.Split('^');

            int conditionNum = 0;

            try
            {
                conditionNum = int.Parse(conditionArr[0].TrimStart('0'));
            }
            catch (FormatException ex)
            {
                Console.WriteLine("conditionNum 기본값 사용 " + ex.Message);
            }

            if (openApi.SendCondition(SCREEN_NO_CONDITION, conditionArr[1], conditionNum, 1) == 0)
            {
                Console.WriteLine("조건검색 요청 실패");
            }
        }
        //---------------------------------------------------------- Request  ----------------------------------------------------------

        //----------------------------------------------------------- Event  -----------------------------------------------------------
        private void initEvent()
        {
            this.openApi.OnReceiveMsg += new _DKHOpenAPIEvents_OnReceiveMsgEventHandler(this.OnReceiveMsgEventHandler);
            this.openApi.OnEventConnect += new _DKHOpenAPIEvents_OnEventConnectEventHandler(this.OnEventConnectEventHandler);
            this.openApi.OnReceiveTrData += new _DKHOpenAPIEvents_OnReceiveTrDataEventHandler(this.OnReceiveTrDataEventHandler);
            this.openApi.OnReceiveRealData += new _DKHOpenAPIEvents_OnReceiveRealDataEventHandler(this.OnReceiveRealDataEventHandler);
            this.openApi.OnReceiveConditionVer += new _DKHOpenAPIEvents_OnReceiveConditionVerEventHandler(this.OnReceiveConditionVerEventHandler);
        }

        private void OnReceiveMsgEventHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            Console.WriteLine("[DEBUG] 화면번호:" + e.sScrNo + ", 사용자구분명:" + e.sRQName + ", TR명:" + e.sTrCode + ", 메시지:" + e.sMsg);
        }

        private void OnEventConnectEventHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode == 0)
            {
                Console.WriteLine("로그인 완료");
                
                // 계좌 비밀번호 입력창 띄움
                openApi.KOA_Functions("ShowAccountWindow", "");

                this.LoginSuccessEvent(true);
                this.loadConditions();
            }
        }

        private void OnReceiveTrDataEventHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            Console.WriteLine("RQName : " + e.sRQName);

            try
            {
                this.GetType().GetMethod(e.sRQName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).Invoke(this, new object[] { e });
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("[ERROR] " + ex.Message);
            }
        }

        private void OnReceiveRealDataEventHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            //Console.WriteLine("[DEBUG] OnReceiveRealData - e.sRealKey : " + e.sRealKey + ", e.sRealType : " + e.sRealType + ", e.sRealData : " + e.sRealData);
            // 보유 종목에 있을 경우 값 업데이트
            string market = openApi.GetCommRealData(e.sRealKey, 290).Trim();

            if (market.Equals("2")) // 장 중
            {
                Holding holding = holdings.SingleOrDefault(item => item.StockNo.Contains(e.sRealKey));
                if (holding != null)
                {
                    long currentPrice = long.Parse(Regex.Replace(openApi.GetCommRealData(e.sRealKey, 10).Trim(), @"[^0-9]", ""));
                    holding.CurrentPrice = String.Format("{0:#,###0}", currentPrice);
                }

                ConditionStock conditionStock = conditionStocks.SingleOrDefault(item => item.StockNo.Contains(e.sRealKey));
                if (conditionStock != null)
                {
                    long currentPrice = long.Parse(Regex.Replace(openApi.GetCommRealData(e.sRealKey, 10).Trim(), @"[^0-9]", ""));
                    conditionStock.CurrentPrice = String.Format("{0:#,###0}", currentPrice);

                    float fluctuationRate = float.Parse(openApi.GetCommRealData(e.sRealKey, 12).Trim());
                    conditionStock.FluctuationRate = String.Format("{0:f2}", fluctuationRate);

                    // 주문 목록에 없고, 보유종목에 없으면 매수
                    // 주문이 들어가기 전에 한번 더 요청이 오면? 안되겠군..
                    // 조건 검색에 편입되는 순간 확인하고..큐에 넣고 큐를 뒤져야하나?
                    // 편입, 이탈이 반복되는 경우는 어떻게 하지?
                    if (holding != null)
                    {

                    }
                }
            }
        }

        private void OnReceiveConditionVerEventHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            // 조건식 목록
            Console.WriteLine("[DEBUG] OnReceiveConditionVer " + e.lRet + " " + e.sMsg);
            if (e.lRet != 1) return;

            //comboBox2.Items.AddRange(kiwoomApi.GetConditionNameList().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            this.conditions = openApi.GetConditionNameList().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private void kiwoomApi_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            // 조건검색 요청으로 검색된 종목코드 리스트를 전달하는 이벤트. ';'로 구분
            Console.WriteLine("[DEBUG] OnReceiveTrCondition " + e.sScrNo + " " + e.strCodeList);
            int cnt = e.strCodeList.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (cnt > 0)
            {
                // 한번에 100 종목을 조회할 수 있는 코드
                //axKHOpenAPI1.CommKwRqData(e.strCodeList.Remove(e.strCodeList.Length - 1), 0, cnt, 0, "조건검색종목", 화면번호_조건검색);
            }
        }

        private void kiwoomApi_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            // 실시간 조건검색 요청으로 종목 편입 확인 (type - I : 종목편입, D : 종목이탈)
            if (e.strType.Equals("I"))
            {
                Console.WriteLine("조건검색 실시간 편입 [" + e.sTrCode + "]");

                ConditionStock conditionStock = conditionStocks.SingleOrDefault(item => item.StockNo.Equals(e.sTrCode));

                if (conditionStock != null)
                {
                    conditionStock.Status = "편입";
                    conditionStock.upTransferCnt();
                }
                else
                {
                    //conditionStocks.Add(new ConditionStock(e.sTrCode, kiwoomApi.GetMasterCodeName(e.sTrCode)));
                    requestRealtimeQuote(SCREEN_NO_CONDITION, e.sTrCode, conditionStocks.Count > 0 ? REALTIME_ADD : REALTIME_NEW);
                }

                // 실시간으로 여러 종목을 가져올 경우는 없는가? 확인이 필요할듯..

            }
            else if (e.strType.Equals("D"))
            {
                Console.WriteLine("조건검색 실시간 이탈 [" + e.sTrCode + "]");

                ConditionStock conditionStock = conditionStocks.SingleOrDefault(item => item.StockNo.Equals(e.sTrCode));

                if (conditionStock != null)
                {
                    conditionStock.Status = "이탈";
                    removeRealtimeQuote(SCREEN_NO_CONDITION, e.sTrCode);
                }
            }
        }

        private void kiwoomApi_OnReceiveChejanData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            //Console.WriteLine("[DEBUG] OnReceiveChejanData - " + (e.sGubun.Equals("0") ? "[체결]" : "[잔고]") + getChejanData(e.sFIdList));
            /*
            [DEBUG] OnReceiveChejanData - [체결] 계좌번호:8139380411 주문번호:0060353 종목코드:A021050 주문상태:접수 종목명:서원 주문수량:10 주문가격:4125 미체결수량:10 체결누계금액:0 원주문번호:0000000 주문구분:+매수 매매구분:보통 매도수구분:2 주문/체결시간:095744 체결번호: 체결가: 체결량: 현재가:+4130 (최우선)매도호가:+4130 (최우선)매수호가: 4125 단위체결가: 단위체결량: 거부사유:0 화면번호:2001
            [DEBUG] OnReceiveChejanData - [체결] 계좌번호:8139380411 주문번호:0060353 종목코드:A021050 주문상태:체결 종목명:서원 주문수량:10 주문가격:4125 미체결수량:0 체결누계금액:41250 원주문번호:0000000 주문구분:+매수 매매구분:보통 매도수구분:2 주문/체결시간:095747 체결번호:204322 체결가:4125 체결량:10 현재가: 4125 (최우선)매도호가: 4125 (최우선)매수호가:-4120 단위체결가:4125 단위체결량:10 거부사유:0 화면번호:2001
            [DEBUG] OnReceiveChejanData - [잔고] 계좌번호:8139380411 종목코드:A021050 종목명:서원 주문수량: 현재가: 4125 (최우선)매도호가: 4125 (최우선)매수호가:-4120 화면번호: 신용구분:00 대출일:00000000 보유수량:25 매입단가:4150 ?祺탔蹈?103750 주문가능수량:25 당일순매수수량:20 매도/매수구분:2 당일총매도손일:0 예수금:0 기준가:4125 손익율:0.00 신용금액:0 신용이자:0 만기일:00000000 당일실현손익(유가):0 당일실현손익률(유가):0.00 당일실현손익(신용):0 당일실현손익률(신용):0.00 상한가:+5360 하한가:-2890
             */

            if (e.sGubun.Equals("0"))
            {
                // 체결인 경우
                //updateOrders();
            }
            else
            {
                // 잔고인 경우에 실시간 잔고에 수량 등 반영
                // 종목코드, 보유수량, 매입단가
                //updateHoldings();
            }
        }
        //----------------------------------------------------------- Event  -----------------------------------------------------------

        //---------------------------------------------- 키움 OpenApi TR 요청 결과 처리부 ----------------------------------------------
        private void trAccountBalance(AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            long totalValuationAmount = long.Parse(openApi.GetCommData(e.sTrCode, e.sRQName, 0, "총평가손익금액"));
            long estimatedBalance = long.Parse(openApi.GetCommData(e.sTrCode, e.sRQName, 0, "추정예탁자산"));

            this.AccountBalanceEvent(totalValuationAmount, estimatedBalance);

            int cnt = openApi.GetRepeatCnt(e.sTrCode, e.sRQName);

            this.holdings.Clear();
            
            for (int i = 0; i < cnt; i++)
            {
                string stockNo = openApi.GetCommData(e.sTrCode, e.sRQName, i, "종목번호").Trim().Replace("A", "");
                string stockName = openApi.GetCommData(e.sTrCode, e.sRQName, i, "종목명").Trim();
                long currentPrice = long.Parse(openApi.GetCommData(e.sTrCode, e.sRQName, i, "현재가").Trim());
                int qty = int.Parse(openApi.GetCommData(e.sTrCode, e.sRQName, i, "보유수량").Trim());
                long buyPrice = long.Parse(openApi.GetCommData(e.sTrCode, e.sRQName, i, "매입가").Trim());
                long totalBuyPrice = long.Parse(openApi.GetCommData(e.sTrCode, e.sRQName, i, "매입금액").Trim());
                long profit = long.Parse(openApi.GetCommData(e.sTrCode, e.sRQName, i, "평가손익").Trim());
                float profitRate = float.Parse(openApi.GetCommData(e.sTrCode, e.sRQName, i, "수익률(%)").Trim());

                this.holdings.Add(new Holding(stockNo, stockName, currentPrice, qty, buyPrice, totalBuyPrice, profit, profitRate, loginInfo.getServerGubun));
            }

            if (this.holdings.Count > 0)
            {
                string codeList = string.Join(";", holdings.Select(item => item.StockNo));

                // 실시간 시세 요청
                requestRealtimeQuote(SCREEN_NO_ACCOUNT_INFO, codeList, REALTIME_NEW);
            }
        }
        //---------------------------------------------- 키움 OpenApi TR 요청 결과 처리부 ----------------------------------------------

        //---------------------------------------------------------- Function ----------------------------------------------------------
        public bool openLoginDialog()
        {
            if(openApi.CommConnect() == 0)
            {
                Console.WriteLine("로그인창 열림");

                return true;
            }

            Console.WriteLine("로그인창 안열림");

            return false;
        }

        public string accountCnt()
        {
            return this.openApi.GetLoginInfo("ACCOUNT_CNT");
        }

        public LoginInfo LoginInfo
        {
            get
            {
                if (loginInfo == null)
                {
                    loginInfo = new LoginInfo();
                    loginInfo.accountCnt = int.Parse(this.openApi.GetLoginInfo("ACCOUNT_CNT").Trim());
                    loginInfo.accList = this.openApi.GetLoginInfo("ACCLIST").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    loginInfo.userId = this.openApi.GetLoginInfo("USER_ID").Trim();
                    loginInfo.userName = this.openApi.GetLoginInfo("USER_NAME").Trim();
                    loginInfo.keyBsecgb = int.Parse(this.openApi.GetLoginInfo("KEY_BSECGB").Trim());
                    loginInfo.firewSecgb = int.Parse(this.openApi.GetLoginInfo("FIREW_SECGB").Trim());
                    loginInfo.getServerGubun = int.Parse(this.openApi.GetLoginInfo("GetServerGubun").Trim());
                }

                return loginInfo;
            }
        }

        public string[] initAccountList()
        {
            int cnt = int.Parse(openApi.GetLoginInfo("ACCOUNT_CNT"));

            if (cnt > 0)
            {
                return openApi.GetLoginInfo("ACCLIST").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            }

            return null;
        }

        public BindingList<Holding> Holdings
        {
            get
            {
                return this.holdings;
            }
        }

        public bool Running
        {
            get
            {
                return this.isRunning;
            }
        }

        public bool runTrading()
        {
            if(this.isRunning)
            {
                MessageBox.Show("이미 매매가 시작되었습니다.");
                return false;
            } else if(strategy == null)
            {
                MessageBox.Show("전략이 설정되지 않았습니다.");
                return false;
            }

            this.isRunning = true;

            requestCondition();

            return true;
        }

        public void saveStrategy(Strategy strategy)
        {
            this.strategy = strategy;
            Console.WriteLine("전략이 저장되었습니다.");
        }
        //---------------------------------------------------------- Function ----------------------------------------------------------
    }
}
