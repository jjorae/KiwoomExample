using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourCommaTrader
{
    // 보유종목
    public class HoldStock : INotifyPropertyChanged
    {
        private int _serverGubun;

        // 종목번호
        private string _stockNo;

        public string StockNo {
            get { return _stockNo; }
            set { 
                _stockNo = value.Trim().Replace("A","");
                this.NotifyPropertyChanged("StockNo");
            }
        }

        // 종목명
        private string _stockName;
        public string StockName
        {
            get { return _stockName; }
            set
            {
                _stockName = value;
                this.NotifyPropertyChanged("StockName");
            }
        }
        // 현재가
        private long _currentPrice;
        public string CurrentPrice
        {
            get { return string.Format("{0:#,###0}", _currentPrice); }
            set
            {
                _currentPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                Profit = string.Format("{0:#,###0}", (_currentPrice - _buyPrice) * _qty - getFee());
                this.NotifyPropertyChanged("CurrentPrice");
            }
        }

        // 고가
        private long _dayHighPrice;
        public string DayHighPrice
        {
            get { return string.Format("{0:#,###0}", _dayHighPrice); }
            set
            {
                _dayHighPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("DayHighPrice");
                updateChangedValue();
            }
        }

        // 저가
        private long _dayLowPrice;
        public string DayLowPrice
        {
            get { return string.Format("{0:#,###0}", _dayLowPrice); }
            set
            {
                _dayLowPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("DayLowPrice");
                updateChangedValue();
            }
        }

        private void updateChangedValue()
        {
            this.NotifyPropertyChanged("TargetLine");
            this.NotifyPropertyChanged("FirstBuyPrice");
            this.NotifyPropertyChanged("MiddleLine");
            this.NotifyPropertyChanged("SecondBuyPrice");
            this.NotifyPropertyChanged("ThirdBuyPrice");
        }

        // 매수 횟수
        private int _buyCnt;
        public int BuyCnt
        {
            get
            {
                return _buyCnt;
            }
            set
            {
                _buyCnt = value;
                this.NotifyPropertyChanged("BuyCnt");
            }
        }

        // 수익선
        private double _targetLine;
        public string TargetLine
        {
            get
            {
                if(_buyCnt == 2)
                {
                    _targetLine = _firstBuyPrice;
                } else if (_buyCnt == 3)
                {
                    _targetLine = _middleLine;
                }
                else
                {
                    _targetLine = (_dayLowPrice + (_dayHighPrice - _dayLowPrice) * 0.667) * 1.0199;
                }
                
                return string.Format("{0:#,###0}", _targetLine);
            }
        }

        // 1차 매수가
        private double _firstBuyPrice;
        public string FirstBuyPrice
        {
            get
            {
                _firstBuyPrice = _dayLowPrice + (_dayHighPrice - _dayLowPrice) * 0.667;
                return string.Format("{0:#,###0}", _firstBuyPrice);
            }
        }

        // 미들
        private double _middleLine;
        public string MiddleLine
        {
            get
            {
                _middleLine = _dayLowPrice + (_dayHighPrice - _dayLowPrice) * 0.583;
                return string.Format("{0:#,###0}", _middleLine);
            }
        }

        // 2차 매수가
        private double _secondBuyPrice;
        public string SecondBuyPrice
        {
            get
            {
                _secondBuyPrice = _dayLowPrice + (_dayHighPrice - _dayLowPrice) * 0.5;
                return string.Format("{0:#,###0}", _secondBuyPrice);
            }
        }

        // 3차 매수가
        private double _thirdBuyPrice;
        public string ThirdBuyPrice
        {
            get
            {
                _thirdBuyPrice = _dayLowPrice + (_dayHighPrice - _dayLowPrice) * 0.333;
                return string.Format("{0:#,###0}", _thirdBuyPrice);
            }
        }

        // 보유수량
        private int _qty;
        public string Qty
        {
            get { return string.Format("{0:#,###0}", _qty); }
            set
            {
                _qty = int.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("Qty");
            }
        }
        // 매입가
        private long _buyPrice;
        public string BuyPrice
        {
            get { return string.Format("{0:#,###0}", _buyPrice); }
            set
            {
                _buyPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("BuyPrice");
            }
        }
        // 매입금액
        private long _totalBuyPrice;
        public string TotalBuyPrice
        {
            get { return string.Format("{0:#,###0}", _totalBuyPrice); }
            set
            {
                _totalBuyPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("TotalBuyPrice");
            }
        }

        // 평가손익
        private long _profit;
        public string Profit
        {
            get { return string.Format("{0:#,###0}", _profit); }
            set
            {
                _profit = long.Parse(value, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowLeadingSign);
                //Console.WriteLine("Profit " + _profit + " Total " + _totalBuyPrice + " rate " + (float.Parse("" + _profit) / float.Parse("" + _totalBuyPrice) * 100.0));
                ProfitRate = String.Format("{0:f2}", (float.Parse("" + _profit) / float.Parse("" + _totalBuyPrice) * 100.0));
                this.NotifyPropertyChanged("Profit");
            }
        }

        // 수익률
        private float _profitRate;
        public string ProfitRate
        {
            get { return String.Format("{0:f2}", _profitRate); }
            set
            {
                _profitRate = float.Parse(value);

                if(_profitRate > 0 && _profitRate > _maxProfitRate)
                {
                    _maxProfitRate = _profitRate;
                }

                this.NotifyPropertyChanged("ProfitRate");
                this.NotifyPropertyChanged("MaxProfitRate");
            }
        }

        // 최고 수익률
        private float _maxProfitRate;
        public string MaxProfitRate
        {
            get { return String.Format("{0:f2}", _profitRate); }
            set
            {
                _profitRate = float.Parse(value);
                this.NotifyPropertyChanged("ProfitRate");
            }
        }

        // 주문여부
        private bool _ordered;

        public string Ordered
        {
            get { return _ordered ? "주문" : "대기"; }
            set
            {
                _ordered = value.Equals("주문") ? true : false;
                this.NotifyPropertyChanged("Ordered");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public HoldStock(string stockNo, string stockName, long currentPrice, int qty, long buyPrice, long totalBuyPrice, int serverGubun)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _currentPrice = currentPrice;
            _qty = qty;
            _buyPrice = buyPrice;
            _totalBuyPrice = totalBuyPrice;
            _serverGubun = serverGubun;
            _buyCnt = 1;
        }

        public HoldStock(string stockNo, string stockName, long currentPrice, int qty, long buyPrice, long totalBuyPrice, long profit, float profitRate, int serverGubun)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _currentPrice = currentPrice;
            _qty = qty;
            _buyPrice = buyPrice;
            _totalBuyPrice = totalBuyPrice;
            _profit = profit;
            _profitRate = profitRate;
            _serverGubun = serverGubun;
            _buyCnt = 1;
        }

        public HoldStock(string stockNo, string stockName, long currentPrice, int qty, long buyPrice, long totalBuyPrice, long profit, float profitRate, int serverGubun, int buyCnt)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _currentPrice = currentPrice;
            _qty = qty;
            _buyPrice = buyPrice;
            _totalBuyPrice = totalBuyPrice;
            _profit = profit;
            _profitRate = profitRate;
            _serverGubun = serverGubun;
            _buyCnt = buyCnt;
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private long getFee()
        {
            return getBuyFee(_totalBuyPrice) + getSellFee(_currentPrice * _qty);
        }

        private long getBuyFee(long totalPrice)
        {
            /**
             * 모의 매수 수수료 : 매수총액*0.35%
                실제 매수 수수료 : 매수총액*0.015%
             */
            double fee = 0;

            if (_serverGubun == 0)
            {
                // 모의투자
                fee += totalPrice * 0.0035;
            }
            else
            {
                // 실투자
                fee += totalPrice * 0.00015;
            }

            //Console.WriteLine("Total : " + totalPrice + "Fee : " + fee);

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

            if (_serverGubun == 0)
            {
                // 모의투자
                fee += totalPrice * 0.0035;
                fee += totalPrice * 0.0025;
            }
            else
            {
                // 실투자
                fee += totalPrice * 0.00015;
                fee += totalPrice * 0.0025;
            }

            //Console.WriteLine("Total : " + totalPrice + "Fee : " + fee);

            return long.Parse(Math.Ceiling(fee).ToString());
        }
    }
}
