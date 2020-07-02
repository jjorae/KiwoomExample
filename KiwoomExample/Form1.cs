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
        private string SCREEN_NO_ORDER = "2001";

        private string REALTIME_NEW = "0";
        private string REALTIME_ADD = "1";

        private int ORDER_TYPE_BUY = 1;
        private int ORDER_TYPE_SELL = 2;
        private string ORDER_HOGA_MARKET = "03";
        private string ORDER_HOGA_LIIMIT = "00";

        private BindingList<Holding> holdings;
        private BindingList<Order> orders;
        private List<Stock> stocks;

        private LoginInfo loginInfo;

        public Form1()
        {
            InitializeComponent();

            holdings = new BindingList<Holding>();
            orders = new BindingList<Order>();
            stocks = new List<Stock>();
            dataHolding.DataSource = holdings;
            dataOrder.DataSource = orders;
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

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                searchStock(textBox.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            searchStock(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(numericUpDown1.Value < 1)
            {
                MessageBox.Show("수량을 입력하세요.");
                return;
            } else if(numericUpDown2.Value < 1)
            {
                MessageBox.Show("수량을 입력하세요.");
                return;
            }

            Stock selectedStock = (Stock)comboBox1.SelectedItem;
            order(ORDER_TYPE_BUY, selectedStock.stockNo, int.Parse(numericUpDown1.Value.ToString()), int.Parse(numericUpDown2.Value.ToString()), radioButton1.Checked ? ORDER_HOGA_LIIMIT : ORDER_HOGA_MARKET);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 1)
            {
                MessageBox.Show("수량을 입력하세요.");
                return;
            }
            else if (numericUpDown2.Value < 1)
            {
                MessageBox.Show("수량을 입력하세요.");
                return;
            }

            Stock selectedStock = (Stock)comboBox1.SelectedItem;
            order(ORDER_TYPE_SELL, selectedStock.stockNo, int.Parse(numericUpDown1.Value.ToString()), int.Parse(numericUpDown2.Value.ToString()), radioButton1.Checked ? ORDER_HOGA_LIIMIT : ORDER_HOGA_MARKET);
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

        private void getAllStocks()
        {
            // 모든 종목 코드 가져와서 (null : 전체, 0 : 장내, 10 : 코스닥...등등)
            string stockCodes = kiwoomApi.GetCodeListByMarket("0") + kiwoomApi.GetCodeListByMarket("10");
            foreach (string code in stockCodes.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                Stock stock = new Stock(code, kiwoomApi.GetMasterCodeName(code));
                
                stocks.Add(stock);
            }
        }

        private void searchStock(string searchStr)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("검색하실 종목명이나 종목코드를 입력하세요.");
                return;
            }

            comboBox1.DataSource = stocks.FindAll(item => item.stockName.Contains(searchStr) || item.stockNo.Contains(searchStr));
        }

        private void updateHoldings()
        {
            string stockNo = kiwoomApi.GetChejanData(9001).Trim();
            int qty = int.Parse(kiwoomApi.GetChejanData(930));
            long buyPrice = long.Parse(kiwoomApi.GetChejanData(931));
            long totalBuyPrice = long.Parse(kiwoomApi.GetChejanData(932));

            Holding holding = holdings.SingleOrDefault(item => item.StockNo.Contains(stockNo));
            if (holding != null)
            {
                if(qty == 0)
                {
                    holdings.Remove(holding);
                } else
                {
                    holding.Qty = String.Format("{0:#,###0}", qty);
                    holding.BuyPrice = String.Format("{0:#,###0}", buyPrice);
                    holding.TotalBuyPrice = String.Format("{0:#,###0}", totalBuyPrice);
                }
            } else
            {
                //holdings.Add(new Holding())
            }
        }

        private void updateOrders()
        {
            // 접수 : 주문번호,종목코드,주문상태,종목명,주문수량,주문가격,미체결수량,주문시간,화면번호
            // 체결 : 주문번호,종목코드,주문상태,종목명,주문수량,주문가격,미체결수량,체결시간,체결번호,체결가,체결량,화면번호
            string orderNo = kiwoomApi.GetChejanData(9203).Trim();
            string stockNo = kiwoomApi.GetChejanData(9001).Trim();
            string orderStatus = kiwoomApi.GetChejanData(913);
            string stockName = kiwoomApi.GetChejanData(302).Trim();
            int qty = int.Parse(kiwoomApi.GetChejanData(900));
            long orderPrice = long.Parse(kiwoomApi.GetChejanData(901));
            int unclosedQty = int.Parse(kiwoomApi.GetChejanData(902));

            if(orderStatus.Equals("접수"))
            {
                // 접수
                orders.Add(new Order(orderNo, stockNo, orderStatus, stockName, qty, orderPrice, unclosedQty));
            } else
            {
                // 체결
                Order order = orders.SingleOrDefault(item => item.OrderNo.Contains(orderNo));
                if(order != null)
                {
                    if(unclosedQty == 0)
                    {
                        orders.Remove(order);
                    } else
                    {
                        order.OrderStatus = orderStatus;
                        order.UnclosedQty = String.Format("{0:#,###0}", unclosedQty);
                    }
                }
            }
        }

        private string getChejanData(string fidList)
        {
            string result = "";

            if (fidList.Contains("9201"))
            {
                result += " 계좌번호:" + kiwoomApi.GetChejanData(9201);
            }

            if (fidList.Contains("9203"))
            {
                result += " 주문번호:" + kiwoomApi.GetChejanData(9203);
            }

            if (fidList.Contains("9001"))
            {
                result += " 종목코드:" + kiwoomApi.GetChejanData(9001).Trim();
            }

            if (fidList.Contains("913"))
            {
                // 주문이 들어가면 : 주문, 체결되면 : 체결, 취소 확인 필요.
                result += " 주문상태:" + kiwoomApi.GetChejanData(913);
            }

            if (fidList.Contains("302"))
            {
                result += " 종목명:" + kiwoomApi.GetChejanData(302);
            }

            if (fidList.Contains("900"))
            {
                result += " 주문수량:" + kiwoomApi.GetChejanData(900);
            }

            if (fidList.Contains("901"))
            {
                result += " 주문가격:" + kiwoomApi.GetChejanData(901);
            }

            if (fidList.Contains("902"))
            {
                result += " 미체결수량:" + kiwoomApi.GetChejanData(902);
            }

            if (fidList.Contains("903"))
            {
                result += " 체결누계금액:" + kiwoomApi.GetChejanData(903);
            }

            if (fidList.Contains("904"))
            {
                result += " 원주문번호:" + kiwoomApi.GetChejanData(904);
            }

            if (fidList.Contains("905"))
            {
                result += " 주문구분:" + kiwoomApi.GetChejanData(905);
            }

            if (fidList.Contains("906"))
            {
                result += " 매매구분:" + kiwoomApi.GetChejanData(906);
            }

            if (fidList.Contains("907"))
            {
                result += " 매도수구분:" + kiwoomApi.GetChejanData(907);
            }

            if (fidList.Contains("908"))
            {
                result += " 주문/체결시간:" + kiwoomApi.GetChejanData(908);
            }

            if (fidList.Contains("909"))
            {
                result += " 체결번호:" + kiwoomApi.GetChejanData(909);
            }

            if (fidList.Contains("910"))
            {
                result += " 체결가:" + kiwoomApi.GetChejanData(910);
            }

            if (fidList.Contains("911"))
            {
                result += " 체결량:" + kiwoomApi.GetChejanData(911);
            }

            if (fidList.Contains("10"))
            {
                result += " 현재가:" + kiwoomApi.GetChejanData(10);
            }

            if (fidList.Contains("27"))
            {
                result += " (최우선)매도호가:" + kiwoomApi.GetChejanData(27);
            }

            if (fidList.Contains("28"))
            {
                result += " (최우선)매수호가:" + kiwoomApi.GetChejanData(28);
            }

            if (fidList.Contains("914"))
            {
                result += " 단위체결가:" + kiwoomApi.GetChejanData(914);
            }

            if (fidList.Contains("915"))
            {
                result += " 단위체결량:" + kiwoomApi.GetChejanData(915);
            }

            if (fidList.Contains("919"))
            {
                result += " 거부사유:" + kiwoomApi.GetChejanData(919);
            }

            if (fidList.Contains("920"))
            {
                result += " 화면번호:" + kiwoomApi.GetChejanData(920);
            }

            if (fidList.Contains("917"))
            {
                result += " 신용구분:" + kiwoomApi.GetChejanData(917);
            }

            if (fidList.Contains("916"))
            {
                result += " 대출일:" + kiwoomApi.GetChejanData(916);
            }

            if (fidList.Contains("930"))
            {
                result += " 보유수량:" + kiwoomApi.GetChejanData(930);
            }

            if (fidList.Contains("931"))
            {
                result += " 매입단가:" + kiwoomApi.GetChejanData(931);
            }

            if (fidList.Contains("932"))
            {
                result += " 총매입가:" + kiwoomApi.GetChejanData(932);
            }

            if (fidList.Contains("933"))
            {
                result += " 주문가능수량:" + kiwoomApi.GetChejanData(933);
            }

            if (fidList.Contains("945"))
            {
                result += " 당일순매수수량:" + kiwoomApi.GetChejanData(945);
            }

            if (fidList.Contains("946"))
            {
                result += " 매도/매수구분:" + kiwoomApi.GetChejanData(946);
            }

            if (fidList.Contains("950"))
            {
                result += " 당일총매도손일:" + kiwoomApi.GetChejanData(950);
            }

            if (fidList.Contains("951"))
            {
                result += " 예수금:" + kiwoomApi.GetChejanData(951);
            }

            if (fidList.Contains("307"))
            {
                result += " 기준가:" + kiwoomApi.GetChejanData(307);
            }

            if (fidList.Contains("8019"))
            {
                result += " 손익율:" + kiwoomApi.GetChejanData(8019);
            }

            if (fidList.Contains("957"))
            {
                result += " 신용금액:" + kiwoomApi.GetChejanData(957);
            }

            if (fidList.Contains("958"))
            {
                result += " 신용이자:" + kiwoomApi.GetChejanData(958);
            }

            if (fidList.Contains("918"))
            {
                result += " 만기일:" + kiwoomApi.GetChejanData(918);
            }

            if (fidList.Contains("990"))
            {
                result += " 당일실현손익(유가):" + kiwoomApi.GetChejanData(990);
            }

            if (fidList.Contains("991"))
            {
                result += " 당일실현손익률(유가):" + kiwoomApi.GetChejanData(991);
            }

            if (fidList.Contains("992"))
            {
                result += " 당일실현손익(신용):" + kiwoomApi.GetChejanData(992);
            }

            if (fidList.Contains("993"))
            {
                result += " 당일실현손익률(신용):" + kiwoomApi.GetChejanData(993);
            }

            if (fidList.Contains("397"))
            {
                result += " 파생상품거래단위:" + kiwoomApi.GetChejanData(397);
            }

            if (fidList.Contains("305"))
            {
                result += " 상한가:" + kiwoomApi.GetChejanData(305);
            }

            if (fidList.Contains("306"))
            {
                result += " 하한가:" + kiwoomApi.GetChejanData(306);
            }

            return result;
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

        private void order(int orderType, string code, int qty, int price, string hoga)
        {
            int result = kiwoomApi.SendOrder("ORDER", SCREEN_NO_ORDER, getSeletedAccountNo(), orderType, code, qty, price, hoga, "");

            if (result < 0)
            {
                Console.WriteLine("[ERROR] 주문 실패 " + result);
            }
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
            //labelEstimatedAssetsVal.Text = convertMoneyFormat(estimatedAssets) + "원";
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
                getAllStocks();
                getLoginInfo();
                initAccountList();
                requestTrEstimatedAssets(); // 추정자산조회
            }
        }

        private void kiwoomApi_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            Console.WriteLine("RQName : " + e.sRQName);

            try
            {
                this.GetType().GetMethod(e.sRQName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).Invoke(this, new object[] { e });
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine("[ERROR] " + ex.Message);
            }
        }

        private void kiwoomApi_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            // 보유 종목에 있을 경우 값 업데이트
            Holding holding = holdings.SingleOrDefault(item => item.StockNo.Contains(e.sRealKey));
            if(holding != null)
            {
                long currentPrice = long.Parse(kiwoomApi.GetCommRealData(e.sRealKey, 10).Trim().Replace("-", ""));
                holding.CurrentPrice = String.Format("{0:#,###0}", currentPrice);
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

        private void kiwoomApi_OnReceiveChejanData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            Console.WriteLine("[DEBUG] OnReceiveChejanData - " + (e.sGubun.Equals("0") ? "[체결]" : "[잔고]") + getChejanData(e.sFIdList));
            /*
            [DEBUG] OnReceiveChejanData - [체결] 계좌번호:8139380411 주문번호:0060353 종목코드:A021050 주문상태:접수 종목명:서원 주문수량:10 주문가격:4125 미체결수량:10 체결누계금액:0 원주문번호:0000000 주문구분:+매수 매매구분:보통 매도수구분:2 주문/체결시간:095744 체결번호: 체결가: 체결량: 현재가:+4130 (최우선)매도호가:+4130 (최우선)매수호가: 4125 단위체결가: 단위체결량: 거부사유:0 화면번호:2001
            [DEBUG] OnReceiveChejanData - [체결] 계좌번호:8139380411 주문번호:0060353 종목코드:A021050 주문상태:체결 종목명:서원 주문수량:10 주문가격:4125 미체결수량:0 체결누계금액:41250 원주문번호:0000000 주문구분:+매수 매매구분:보통 매도수구분:2 주문/체결시간:095747 체결번호:204322 체결가:4125 체결량:10 현재가: 4125 (최우선)매도호가: 4125 (최우선)매수호가:-4120 단위체결가:4125 단위체결량:10 거부사유:0 화면번호:2001
            [DEBUG] OnReceiveChejanData - [잔고] 계좌번호:8139380411 종목코드:A021050 종목명:서원 주문수량: 현재가: 4125 (최우선)매도호가: 4125 (최우선)매수호가:-4120 화면번호: 신용구분:00 대출일:00000000 보유수량:25 매입단가:4150 ?祺탔蹈?103750 주문가능수량:25 당일순매수수량:20 매도/매수구분:2 당일총매도손일:0 예수금:0 기준가:4125 손익율:0.00 신용금액:0 신용이자:0 만기일:00000000 당일실현손익(유가):0 당일실현손익률(유가):0.00 당일실현손익(신용):0 당일실현손익률(신용):0.00 상한가:+5360 하한가:-2890
             */

            if(e.sGubun.Equals("0"))
            {
                // 체결인 경우
                updateOrders();
            }
            else
            {
                // 잔고인 경우에 실시간 잔고에 수량 등 반영
                // 종목코드, 보유수량, 매입단가
                updateHoldings();
            }
        }
    }
}
