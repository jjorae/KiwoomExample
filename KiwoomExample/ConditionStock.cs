using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwoomExample
{
    // 보유종목
    class ConditionStock : INotifyPropertyChanged
    {
        // 종목코드,종목명,현재가,등락율,상태,편입횟수
        // 종목번호
        private string _stockNo;

        public string StockNo {
            get { return _stockNo; }
            set { 
                _stockNo = value.Trim().Replace("A", "");
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
                this.NotifyPropertyChanged("CurrentPrice");
            }
        }

        // 등락율
        private float _fluctuationRate;
        public string FluctuationRate
        {
            get { return String.Format("{0:f2}", _fluctuationRate); }
            set
            {
                _fluctuationRate = float.Parse(value);
                this.NotifyPropertyChanged("FluctuationRate");
            }
        }

        // 상태
        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                this.NotifyPropertyChanged("Status");
            }
        }

        // 편입횟수
        private int _transferCnt;

        public string TransferCnt
        {
            get { return string.Format("{0:#,###0}", _transferCnt); }
            set
            {
                _transferCnt = int.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("TransferCnt");
            }
        }

        public void upTransferCnt()
        {
            _transferCnt += 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ConditionStock(string stockNo, string stockName)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _status = "편입";
            _transferCnt = 1;
        }

        public ConditionStock(string stockNo, string stockName, long currentPrice, float fluctuationRate)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _currentPrice = currentPrice;
            _fluctuationRate = fluctuationRate;
            _status = "편입";
            _transferCnt = 1;
        }

        public ConditionStock(string stockNo, string stockName, long currentPrice, float fluctuationRate, string status, int transferCnt)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _currentPrice = currentPrice;
            _fluctuationRate = fluctuationRate;
            _status = status;
            _transferCnt = transferCnt;
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
