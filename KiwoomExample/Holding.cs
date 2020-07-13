using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwoomExample
{
    // 보유종목
    class Holding : INotifyPropertyChanged
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
                this.NotifyPropertyChanged("ProfitRate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Holding(string stockNo, string stockName, long currentPrice, int qty, long buyPrice, long totalBuyPrice, int serverGubun)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _currentPrice = currentPrice;
            _qty = qty;
            _buyPrice = buyPrice;
            _totalBuyPrice = totalBuyPrice;
            _serverGubun = serverGubun;
        }

        public Holding(string stockNo, string stockName, long currentPrice, int qty, long buyPrice, long totalBuyPrice, long profit, float profitRate, int serverGubun)
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
