using KiwoomExample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FourCommaTrader
{
    public enum LogMode
    {
        SYSTEM, TRADE
    }

    public partial class Form1 : Form
    {
        private string SCREEN_NO_ACCOUNT_INFO = "1001";
        private string SCREEN_NO_ACCOUNT_INFO_REFRESH = "1002";
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

        private int ORDER_TIMEOUT = 600;
        /*
         [필요한 항목]
         한 종목당 매수 금액(1회*3)
         손절?
         동작 시간 설정?
         종목 금액이랑 한 종목당 주문 금액 비교해서..
         가진 돈이랑 주문 금액 비교..
         */
        private int oneTimeAmount = 10000;
        private int availableAmount = 0;
        private double lossCut = 10.0;


        private BindingList<HoldStock> holdings;
        private BindingList<OrderStock> orders;
        private BindingList<DetectionStock> conditionStocks;

        private LoginInfo loginInfo;

        public Form1()
        {
            InitializeComponent();

            holdings = new BindingList<HoldStock>();
            orders = new BindingList<OrderStock>();
            conditionStocks = new BindingList<DetectionStock>();
            
            setHoldingDataGridView();
            setOrderDataGridView();
            setDetectConditionDataGridView();
            //Console.WriteLine("" + DateTime.ParseExact("092311", "HHmmss", null) + " " + ((DateTime.Now - DateTime.ParseExact("092311", "HHmmss", null)).TotalSeconds / 60 /60));
        }

        private void setHoldingDataGridView()
        {
            dataHolding.AutoGenerateColumns = false;

            dataHolding.Columns.Add(getColumn("StockNo", "종목코드"));
            dataHolding.Columns.Add(getColumn("StockName", "종목명"));
            dataHolding.Columns.Add(getColumn("CurrentPrice", "현재가"));
            //dataHolding.Columns.Add(getColumn("TargetLine", "수익선"));
            //dataHolding.Columns.Add(getColumn("MiddleLine", "중심선"));
            //dataHolding.Columns.Add(getColumn("FirstBuyPrice", "1차매수"));
            //dataHolding.Columns.Add(getColumn("SecondBuyPrice", "2차매수"));
            //dataHolding.Columns.Add(getColumn("ThirdBuyPrice", "3차매수"));
            dataHolding.Columns.Add(getColumn("BuyPrice", "매입가"));
            dataHolding.Columns.Add(getColumn("Qty", "보유수량"));
            dataHolding.Columns.Add(getColumn("BuyCnt", "매수횟수")); 
            dataHolding.Columns.Add(getColumn("Profit", "평가손익"));
            dataHolding.Columns.Add(getColumn("ProfitRate", "수익률"));
            dataHolding.DataSource = holdings;
        }

        private void setOrderDataGridView()
        {
            dataOrder.AutoGenerateColumns = false;

            dataOrder.Columns.Add(getColumn("OrderNo", "주문번호"));
            dataOrder.Columns.Add(getColumn("StockNo", "종목코드"));
            dataOrder.Columns.Add(getColumn("StockName", "종목명"));
            dataOrder.Columns.Add(getColumn("OrderStatus", "주문상태"));
            dataOrder.Columns.Add(getColumn("Qty", "주문수량"));
            dataOrder.Columns.Add(getColumn("OrderPrice", "주문가격"));
            dataOrder.Columns.Add(getColumn("UnclosedQty", "미체결수량"));
            dataOrder.Columns.Add(getColumn("OrderType", "주문구분"));
            dataOrder.DataSource = orders;
        }

        private void setDetectConditionDataGridView()
        {
            dataDetectCondition.AutoGenerateColumns = false;

            dataDetectCondition.Columns.Add(getColumn("StockNo", "종목코드"));
            dataDetectCondition.Columns.Add(getColumn("StockName", "종목명"));
            dataDetectCondition.Columns.Add(getColumn("TransferPrice", "편입가"));
            dataDetectCondition.Columns.Add(getColumn("CurrentPrice", "현재가"));
            //dataDetectCondition.Columns.Add(getColumn("TargetPrice", "1차매수"));
            dataDetectCondition.Columns.Add(getColumn("FluctuationRate", "등락율"));
            dataDetectCondition.Columns.Add(getColumn("TransferCnt", "편입횟수"));
            dataDetectCondition.Columns.Add(getColumn("Status", "상태"));
            dataDetectCondition.DataSource = conditionStocks;
        }

        private DataGridViewTextBoxColumn getColumn(string dataPropertyName, string headerText)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = dataPropertyName;
            column.HeaderText = headerText;

            return column;
        }

        private void comboConditionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox target = (ComboBox)sender;
            log(LogMode.SYSTEM, "조건 검색 선택 (" + target.SelectedItem + ")");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Button target = (Button)sender;
            target.Enabled = false;

            if(comboConditionList.SelectedIndex < 0 || comboAccount.SelectedIndex < 0)
            {
                MessageBox.Show("조건식과 계좌번호를 선택하세요.");
                target.Enabled = true;
                return;
            }

            comboConditionList.Enabled = false;
            comboAccount.Enabled = false;
            buttonStop.Enabled = true;

            // 조건 검색 및 매매 시작
            detectCondition();
        }

        private void detectCondition()
        {
            conditionStocks.Clear();

            string[] condition = comboConditionList.SelectedItem.ToString().Split('^');

            int conditionNum = 0;

            try
            {
                conditionNum = int.Parse(condition[0].TrimStart('0'));
            }
            catch (FormatException ex)
            {
                log(LogMode.SYSTEM, "기본 조건식 사용 " + ex.Message);
            }

            if (kiwoomApi.SendCondition(SCREEN_NO_CONDITION, condition[1], conditionNum, 1) == 0)
            {
                log(LogMode.SYSTEM, "조건검색 요청 실패");
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Button target = (Button)sender;
            target.Enabled = false;

            if (kiwoomApi.CommConnect() == 0)
            {
                log(LogMode.SYSTEM, "로그인 창이 열렸습니다.");
            }
            else
            {
                log(LogMode.SYSTEM, "로그인 창을 열지 못했습니다.");

                // 로그인창 오픈 실패 시 다시 버튼 활성화
                target.Enabled = true;
            }
        }

        private void comboAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox target = (ComboBox)sender;

            if (target.SelectedIndex < 0)
            {
                labelEstimatedBalance.Text = "0";
                return;
            }

            requestTrEstimatedAssets();
            requestTrAccountBalance();
            requestTrAccountEvaluationStatus();
        }

        private void log(LogMode mode, string msg)
        {
            TextBox textBox;

            if (mode == LogMode.SYSTEM)
            {
                textBox = textBoxSystemLog;
            }
            else
            {
                textBox = textBoxTradingLog;
            }

            textBox.AppendText("[" + DateTime.Now + "] " + msg + "\r\n");
        }

        private void enableComponent()
        {
            comboConditionList.Enabled = true;
            comboAccount.Enabled = true;
            buttonStart.Enabled = true;
        }

        private string convertMoneyFormat(string price)
        {
            return String.Format("{0:#,###0}", int.Parse(price));
        }

        private void loadAccountList()
        {
            int cnt = int.Parse(kiwoomApi.GetLoginInfo("ACCOUNT_CNT"));

            if (cnt > 0)
            {
                comboAccount.Items.AddRange(kiwoomApi.GetLoginInfo("ACCLIST").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

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

        private void loadConditions()
        {
            if (kiwoomApi.GetConditionLoad() == 0)
            {
                log(LogMode.SYSTEM, "[ERROR] 조건검색 로딩 실패");
            }
        }

        private void stopSendCondition(string scrNo, string conditionName, int conditionIdx)
        {
            kiwoomApi.SendConditionStop(scrNo, conditionName, conditionIdx);
        }

        private string getSeletedAccountNo()
        {
            return comboAccount.SelectedItem.ToString().Trim();
        }

        private void requestTrEstimatedAssets()
        {
            kiwoomApi.SetInputValue("계좌번호", getSeletedAccountNo());
            kiwoomApi.SetInputValue("비밀번호", "");
            kiwoomApi.SetInputValue("상장폐지조회구분", "0");

            // TR명 : 추정자산조회요청
            kiwoomApi.CommRqData("trEstimatedAssets", "OPW00003", 0, SCREEN_NO_ACCOUNT_INFO);
        }

        private bool order(int orderType, string code, int qty, int price, string hoga)
        {
            if(orderType == ORDER_TYPE_BUY && price * qty > availableAmount)
            {
                log(LogMode.TRADE, "[ERROR] " + (hoga == ORDER_HOGA_LIIMIT ? "지정가" : "시장가") + " " + (orderType == ORDER_TYPE_BUY ? "매수" : "매도") + " 주문 실패(주문가능 수량 부족) - " + kiwoomApi.GetMasterCodeName(code) + "(" + code + ") " + price + "원 " + qty + "주");
                return false;
            }

            int result = kiwoomApi.SendOrder("ORDER", SCREEN_NO_ORDER, getSeletedAccountNo(), orderType, code, qty, price, hoga, "");

            if (result < 0)
            {
                log(LogMode.TRADE, "[ERROR] " + (hoga == ORDER_HOGA_LIIMIT ? "지정가" : "시장가") + " " + (orderType == ORDER_TYPE_BUY ? "매수" : "매도") + " 주문 실패 - " + kiwoomApi.GetMasterCodeName(code) + "(" + code + ") " + price + "원 " + qty + "주");
                return false;
            }

            log(LogMode.TRADE, (hoga == ORDER_HOGA_LIIMIT ? "지정가" : "시장가") + " " + (orderType == ORDER_TYPE_BUY ? "매수" : "매도") + " 주문 완료 - " + kiwoomApi.GetMasterCodeName(code) + "(" + code + ") " + price + "원 " + qty + "주");
            return true;
        }

        private void cancelOrder(int orderType, string code, string orgOrderNo)
        {
            //주문취소(종목코드, 주문구분.Equals("매수") ? 3 : 4, 주문번호);
            int result = kiwoomApi.SendOrder("ORDER", SCREEN_NO_ORDER, getSeletedAccountNo(), orderType, code, 0, 0, "", orgOrderNo);

            if (result < 0)
            {
                log(LogMode.TRADE, "[ERROR] 주문 취소 실패 " + result);
            }
        }

        private void kiwoomApi_OnReceiveMsg(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            log(LogMode.SYSTEM, "[OnReceiveMsg] 화면번호:" + e.sScrNo + ", 사용자구분명:" + e.sRQName + ", TR명:" + e.sTrCode + ", 메시지:" + e.sMsg);
        }

        private void kiwoomApi_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            log(LogMode.SYSTEM, "로그인 결과 (ErrCode:" + e.nErrCode + ")");

            // 로그인 성공
            if (e.nErrCode == 0)
            {
                // 계좌 비밀번호 입력창 띄움
                kiwoomApi.KOA_Functions("ShowAccountWindow", "");

                // 로그인 완료 후 처리 로직
                enableComponent();
                loadAccountList();
                loadConditions();
                getLoginInfo();
            }
        }

        private void kiwoomApi_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
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

        private void kiwoomApi_OnReceiveConditionVer(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            // 조건식 목록
            Console.WriteLine("[DEBUG] OnReceiveConditionVer " + e.lRet + " " + e.sMsg);
            if (e.lRet != 1) return;

            comboConditionList.Items.AddRange(kiwoomApi.GetConditionNameList().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
        }

        private void kiwoomApi_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            // 조건검색 요청으로 검색된 종목코드 리스트를 전달하는 이벤트. ';'로 구분
            Console.WriteLine("[DEBUG] OnReceiveTrCondition " + e.sScrNo + " " + e.strCodeList);
            int cnt = e.strCodeList.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (cnt > 0 && cnt <= 100)
            {
                // 실시간 시세 요청
                /*foreach(string code in e.strCodeList.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    conditionStocks.Add(new DetectionStock(code, kiwoomApi.GetMasterCodeName(code)));
                    requestRealtimeQuote(SCREEN_NO_CONDITION, code, conditionStocks.Count > 0 ? REALTIME_ADD : REALTIME_NEW);
                }*/
                
                // 한번에 100 종목을 조회할 수 있는 코드
                //axKHOpenAPI1.CommKwRqData(e.strCodeList.Remove(e.strCodeList.Length - 1), 0, cnt, 0, "조건검색종목", 화면번호_조건검색);
            }
        }

        private void kiwoomApi_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            //Console.WriteLine("[DEBUG] OnReceiveRealData - e.sRealKey : " + e.sRealKey + ", e.sRealType : " + e.sRealType + ", e.sRealData : " + e.sRealData);
            // 보유 종목에 있을 경우 값 업데이트
            string market = kiwoomApi.GetCommRealData(e.sRealKey, 290).Trim();

            if (market.Equals("2"))
            {
                if(orders.Count > 0)
                {
                    foreach(OrderStock order in orders)
                    {
                        if(order.AfterOrder > ORDER_TIMEOUT)
                        {
                            log(LogMode.TRADE, "주문번호 " + order.OrderNo + "의 주문이 오래되어 해당 주문을 취소합니다. (" + order.AfterOrder + "초)");
                            cancelOrder(order.OrderType.Equals("매수") ? 3 : 4, order.StockNo, order.OrderNo);
                        }
                    }
                }

                HoldStock holding = holdings.SingleOrDefault(item => item.StockNo.Contains(e.sRealKey));
                if (holding != null)
                {
                    long currentPrice = long.Parse(Regex.Replace(kiwoomApi.GetCommRealData(e.sRealKey, 10).Trim(), @"[^0-9]", ""));
                    holding.CurrentPrice = String.Format("{0:#,###0}", currentPrice);

                    long dayHighPrice = long.Parse(Regex.Replace(kiwoomApi.GetCommRealData(e.sRealKey, 17).Trim(), @"[^0-9]", ""));
                    holding.DayHighPrice = String.Format("{0:#,###0}", dayHighPrice);
                    long dayLowPrice = long.Parse(Regex.Replace(kiwoomApi.GetCommRealData(e.sRealKey, 18).Trim(), @"[^0-9]", ""));
                    holding.DayLowPrice = String.Format("{0:#,###0}", dayLowPrice);

                    /*if (float.Parse(holding.ProfitRate) < -1)
                    {
                        Console.WriteLine("손절 " + holding.StockName + " " + holding.ProfitRate);
                        order(ORDER_TYPE_SELL, holding.StockNo, int.Parse(holding.Qty, System.Globalization.NumberStyles.AllowThousands), 0, ORDER_HOGA_MARKET); // 시장가 손절
                    }
                    else if (float.Parse(holding.ProfitRate) > 1)
                    {
                        Console.WriteLine("익절 " + holding.StockName + " " + holding.ProfitRate);
                        order(ORDER_TYPE_SELL, holding.StockNo, int.Parse(holding.Qty, System.Globalization.NumberStyles.AllowThousands), int.Parse(currentPrice.ToString()), ORDER_HOGA_LIIMIT); // 시장가 손절
                    }*/

                    if (!holding.Ordered.Equals("주문") && long.Parse(holding.TargetLine, System.Globalization.NumberStyles.AllowThousands) > 0 && long.Parse(holding.TargetLine, System.Globalization.NumberStyles.AllowThousands) < currentPrice)
                    {
                        log(LogMode.TRADE, holding.StockName + "(" + holding.StockNo + ") 종목이 익절가에 도달하여 익절 합니다. (" + float.Parse(holding.ProfitRate) + "%)");
                        
                        if(order(ORDER_TYPE_SELL, holding.StockNo, int.Parse(holding.Qty, System.Globalization.NumberStyles.AllowThousands), int.Parse(currentPrice.ToString()), ORDER_HOGA_LIIMIT))
                        {
                            holding.Ordered = "주문";
                        }
                    } else if (long.Parse(holding.SecondBuyPrice, System.Globalization.NumberStyles.AllowThousands) > currentPrice && holding.BuyCnt == 1)
                    {
                        // 2차 매수가 주문
                        log(LogMode.TRADE, holding.StockName + "(" + holding.StockNo + ") 종목이 2차 매수가에 도달하여 주문 합니다. (" + holding.CurrentPrice + "원)");
                        if(order(ORDER_TYPE_BUY, holding.StockNo, (oneTimeAmount / int.Parse(currentPrice.ToString())), int.Parse(currentPrice.ToString()), ORDER_HOGA_LIIMIT))
                        {
                            holding.BuyCnt = holding.BuyCnt + 1;
                        }
                    } else if (long.Parse(holding.ThirdBuyPrice, System.Globalization.NumberStyles.AllowThousands) > currentPrice && holding.BuyCnt == 2)
                    {
                        // 3차 매수가 주문. 기본 2배 물량
                        log(LogMode.TRADE, holding.StockName + "(" + holding.StockNo + ") 종목이 3차 매수가에 도달하여 주문 합니다. (" + holding.CurrentPrice + "원)");
                        if(order(ORDER_TYPE_BUY, holding.StockNo, (oneTimeAmount / int.Parse(currentPrice.ToString())) * 2, int.Parse(currentPrice.ToString()), ORDER_HOGA_LIIMIT))
                        {
                            holding.BuyCnt = holding.BuyCnt + 1;
                        }
                    } else if(holding.BuyCnt >= 2 && float.Parse(holding.ProfitRate) < -2)
                    {
                        log(LogMode.TRADE, holding.StockName + "(" + holding.StockNo + ") 종목이 손절가에 도달하여 손절 합니다. (" + float.Parse(holding.ProfitRate) + "%)");
                        order(ORDER_TYPE_SELL, holding.StockNo, int.Parse(holding.Qty, System.Globalization.NumberStyles.AllowThousands), 0, ORDER_HOGA_MARKET);
                    }
                }

                DetectionStock conditionStock = conditionStocks.SingleOrDefault(item => item.StockNo.Contains(e.sRealKey));
                if (conditionStock != null)
                {
                    long currentPrice = long.Parse(Regex.Replace(kiwoomApi.GetCommRealData(e.sRealKey, 10).Trim(), @"[^0-9]", ""));
                    conditionStock.CurrentPrice = String.Format("{0:#,###0}", currentPrice);

                    long dayHighPrice = long.Parse(Regex.Replace(kiwoomApi.GetCommRealData(e.sRealKey, 17).Trim(), @"[^0-9]", ""));
                    conditionStock.DayHighPrice = String.Format("{0:#,###0}", dayHighPrice);
                    long dayLowPrice = long.Parse(Regex.Replace(kiwoomApi.GetCommRealData(e.sRealKey, 18).Trim(), @"[^0-9]", ""));
                    conditionStock.DayLowPrice = String.Format("{0:#,###0}", dayLowPrice);

                    if (string.IsNullOrEmpty(conditionStock.TransferPrice) || long.Parse(conditionStock.TransferPrice, System.Globalization.NumberStyles.AllowThousands) < 1)
                    {
                        conditionStock.TransferPrice = conditionStock.CurrentPrice;
                    }

                    float fluctuationRate = float.Parse(kiwoomApi.GetCommRealData(e.sRealKey, 12).Trim());
                    conditionStock.FluctuationRate = String.Format("{0:f2}", fluctuationRate);

                    // 1차 매수가에 도달하면 매수 -> 1차 매수 도달 시 매수 대기했다가 돌파하면 매수하는 방식은?
                    if (currentPrice < oneTimeAmount && !conditionStock.Ordered.Equals("주문") && long.Parse(conditionStock.TargetPrice, System.Globalization.NumberStyles.AllowThousands) > 0 && long.Parse(conditionStock.TargetPrice, System.Globalization.NumberStyles.AllowThousands) > currentPrice && long.Parse(conditionStock.TargetPrice, System.Globalization.NumberStyles.AllowThousands) < long.Parse(conditionStock.TransferPrice, System.Globalization.NumberStyles.AllowThousands))
                    {
                        log(LogMode.TRADE, conditionStock.StockName + "(" + conditionStock.StockNo + ")의 1차 매수가에 도달하여 매수 합니다. (1차 매수가 : " + conditionStock.TargetPrice + ", 진입횟수 : " + conditionStock.TransferCnt + ")");

                        if(order(ORDER_TYPE_BUY, conditionStock.StockNo, (oneTimeAmount / int.Parse(currentPrice.ToString())), int.Parse(currentPrice.ToString()), ORDER_HOGA_LIIMIT))
                        {
                            conditionStock.Ordered = "주문";
                        }
                    }
                }
            }
        }

        private void kiwoomApi_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            // 실시간 조건검색 요청으로 종목 편입 확인 (type - I : 종목편입, D : 종목이탈)
            if (e.strType.Equals("I"))
            {
                Console.WriteLine("조건검색 실시간 편입 [" + e.sTrCode + "]");

                DetectionStock conditionStock = conditionStocks.SingleOrDefault(item => item.StockNo.Equals(e.sTrCode));

                if (conditionStock != null)
                {
                    conditionStock.Status = "편입";
                    conditionStock.upTransferCnt();
                }
                else
                {
                    conditionStocks.Add(new DetectionStock(e.sTrCode, kiwoomApi.GetMasterCodeName(e.sTrCode)));
                    requestRealtimeQuote(SCREEN_NO_CONDITION, e.sTrCode, conditionStocks.Count > 0 ? REALTIME_ADD : REALTIME_NEW);
                }

                // 실시간으로 여러 종목을 가져올 경우는 없는가? 확인이 필요할듯..

            }
            else if (e.strType.Equals("D"))
            {
                Console.WriteLine("조건검색 실시간 이탈 [" + e.sTrCode + "]");

                DetectionStock conditionStock = conditionStocks.SingleOrDefault(item => item.StockNo.Equals(e.sTrCode));

                if (conditionStock != null)
                {
                    conditionStock.Status = "이탈";
                    //removeRealtimeQuote(SCREEN_NO_CONDITION, e.sTrCode);
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

            // 선택한 계좌일 경우에만 추가
            if (!kiwoomApi.GetChejanData(9201).Trim().Equals(getSeletedAccountNo())) return;

            if (e.sGubun.Equals("0"))
            {
                // 체결인 경우
                updateOrders();
            }
            else
            {
                // 잔고인 경우에 실시간 잔고에 수량 등 반영
                // 종목코드, 보유수량, 매입단가
                updateHoldings();
                //refreshTrAccountBalance();
            }
        }

        private void updateHoldings()
        {
            string stockNo = kiwoomApi.GetChejanData(9001).Trim().Replace("A", "");
            int qty = int.Parse(kiwoomApi.GetChejanData(930));
            long buyPrice = long.Parse(kiwoomApi.GetChejanData(931));
            long totalBuyPrice = long.Parse(kiwoomApi.GetChejanData(932));

            HoldStock holding = holdings.SingleOrDefault(item => item.StockNo.Contains(stockNo));
            if (holding != null)
            {
                if (qty == 0)
                {
                    holdings.Remove(holding);
                    requestTrEstimatedAssets();
                    requestTrAccountEvaluationStatus();
                }
                else
                {
                    holding.Qty = String.Format("{0:#,###0}", qty);
                    holding.BuyPrice = String.Format("{0:#,###0}", buyPrice);
                    holding.TotalBuyPrice = String.Format("{0:#,###0}", totalBuyPrice);
                }
            }
            else
            {
                string stockName = kiwoomApi.GetChejanData(302).Trim();
                long currentPrice = long.Parse(kiwoomApi.GetChejanData(10));
                holdings.Add(new HoldStock(stockNo, stockName, currentPrice, qty, buyPrice, totalBuyPrice, loginInfo.getServerGubun));

                requestRealtimeQuote(SCREEN_NO_ACCOUNT_INFO, stockNo, REALTIME_ADD);
            }
        }

        private void updateOrders()
        {
            // 접수 : 주문번호,종목코드,주문상태,종목명,주문수량,주문가격,미체결수량,주문시간,화면번호
            // 체결 : 주문번호,종목코드,주문상태,종목명,주문수량,주문가격,미체결수량,체결시간,체결번호,체결가,체결량,화면번호
            string orderNo = kiwoomApi.GetChejanData(9203).Trim();
            string stockNo = kiwoomApi.GetChejanData(9001).Trim().Replace("A", "");
            string orderStatus = kiwoomApi.GetChejanData(913);
            string stockName = kiwoomApi.GetChejanData(302).Trim();
            int qty = int.Parse(kiwoomApi.GetChejanData(900));
            long orderPrice = long.Parse(kiwoomApi.GetChejanData(901));
            int unclosedQty = int.Parse(kiwoomApi.GetChejanData(902));
            string orderType = kiwoomApi.GetChejanData(905);
            string orderTime = kiwoomApi.GetChejanData(908); //주문/체결시간:095744

            if (orderStatus.Equals("접수"))
            {
                // 접수
                orders.Add(new OrderStock(orderNo, stockNo, orderStatus, stockName, qty, orderPrice, unclosedQty, orderType, orderTime));
            }
            else
            {
                // 체결
                OrderStock order = orders.SingleOrDefault(item => item.OrderNo.Contains(orderNo));
                if (order != null)
                {
                    if (unclosedQty == 0)
                    {
                        orders.Remove(order);
                    }
                    else if (orderType.Contains("취소"))
                    {
                        Console.WriteLine("주문번호 " + orderNo + " " + orderType);
                        orders.Remove(order);
                    }
                    else
                    {
                        order.OrderStatus = orderStatus;
                        order.UnclosedQty = String.Format("{0:#,###0}", unclosedQty);
                    }
                }
            }
        }

        private void trEstimatedAssets(AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            string estimatedAssets = kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "추정예탁자산").Trim();
            labelEstimatedBalance.Text = convertMoneyFormat(estimatedAssets);
        }

        private void trAccountEvaluationStatus(AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            // D+2추정예수금만 사용
            availableAmount = int.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "D+2추정예수금"));
            log(LogMode.SYSTEM, "주문가능금액 : " + availableAmount);
        }

        private void trAccountBalance(AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            /*totalPurchaseAmount.Text = String.Format("{0:#,###0}", long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "총매입금액")));
            totalEvaluationAmount.Text = String.Format("{0:#,###0}", long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "총평가금액")));
            totalValuationAmount.Text = String.Format("{0:#,###0}", long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "총평가손익금액")));
            totalYield.Text = String.Format("{0:f2}", double.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "총수익률(%)")));
            estimatedBalance.Text = String.Format("{0:#,###0}", long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, 0, "추정예탁자산")));*/

            if (SCREEN_NO_ACCOUNT_INFO.Equals(e.sScrNo))
            {
                int cnt = kiwoomApi.GetRepeatCnt(e.sTrCode, e.sRQName);

                holdings.Clear();

                for (int i = 0; i < cnt; i++)
                {
                    string stockNo = kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "종목번호").Trim().Replace("A", "");
                    string stockName = kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "종목명").Trim();
                    long currentPrice = long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "현재가").Trim());
                    int qty = int.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "보유수량").Trim());
                    long buyPrice = long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "매입가").Trim());
                    long totalBuyPrice = long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "매입금액").Trim());
                    long profit = long.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "평가손익").Trim());
                    float profitRate = float.Parse(kiwoomApi.GetCommData(e.sTrCode, e.sRQName, i, "수익률(%)").Trim());

                    int buyCnt = qty > 0 ? (int)Math.Ceiling((double)((int)totalBuyPrice) / oneTimeAmount):0;
                    if (buyCnt > 3) buyCnt = 3;

                    holdings.Add(new HoldStock(stockNo, stockName, currentPrice, qty, buyPrice, totalBuyPrice, profit, profitRate, loginInfo.getServerGubun, buyCnt));
                }

                if (holdings.Count > 0)
                {
                    string codeList = string.Join(";", holdings.Select(item => item.StockNo));

                    // 실시간 시세 요청
                    requestRealtimeQuote(SCREEN_NO_ACCOUNT_INFO, codeList, REALTIME_NEW);
                }
            }
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

        private void requestTrAccountEvaluationStatus()
        {
            // OPW00004 : 계좌평가현황요청
            kiwoomApi.SetInputValue("계좌번호", getSeletedAccountNo());
            kiwoomApi.SetInputValue("비밀번호", "");
            kiwoomApi.SetInputValue("비밀번호입력매체구분", "00");
            kiwoomApi.SetInputValue("조회구분", "1");

            kiwoomApi.CommRqData("trAccountEvaluationStatus", "OPW00004", 0, SCREEN_NO_ACCOUNT_INFO);
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
             * 16 : 시가
             * 17 : 고가
             * 18 : 저가
             * 20 : 체결시간
             */
            int ret = kiwoomApi.SetRealReg(scrNo, codeList, "9001;302;10;11;25;12;13;15;16;17;18;20;920", optType);

            if (ret < 0)
            {
                log(LogMode.SYSTEM, "[ERROR] 실시간시세요청 실패 - " + codeList + " (CODE : " + ret + ")");
            }
        }

        private void removeRealtimeQuote(string scrNo, string codeList)
        {
            kiwoomApi.SetRealRemove(scrNo, codeList);
        }

        private void requestTrBasicInformation(string code, string scrNo)
        {
            kiwoomApi.SetInputValue("종목코드", code);

            kiwoomApi.CommRqData("trBasicInformation", "opt10001", 0, scrNo);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            string[] condition = comboConditionList.SelectedItem.ToString().Split('^');

            int conditionNum = 0;

            try
            {
                conditionNum = int.Parse(condition[0].TrimStart('0'));
            }
            catch (FormatException ex)
            {
                log(LogMode.SYSTEM, "conditionNum 기본값 사용 " + ex.Message);
            }

            stopSendCondition(SCREEN_NO_CONDITION, condition[1], conditionNum);

            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            comboConditionList.Enabled = true;
            comboAccount.Enabled = true;
        }

        private void buttonSaveLog_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, textBoxTradingLog.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, textBoxSystemLog.Text);
            }
        }
    }
}
