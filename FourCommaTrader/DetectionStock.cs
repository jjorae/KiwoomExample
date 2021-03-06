﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourCommaTrader
{
    // 보유종목
    public class DetectionStock : INotifyPropertyChanged
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

        // 편입가
        private long _transferPrice;
        public string TransferPrice
        {
            get { return string.Format("{0:#,###0}", _transferPrice); }
            set
            {
                _transferPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("TransferPrice");
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

        // 고가
        private long _dayHighPrice;
        public string DayHighPrice
        {
            get { return string.Format("{0:#,###0}", _dayHighPrice); }
            set
            {
                _dayHighPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("DayHighPrice");
                this.NotifyPropertyChanged("TargetPrice");
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
                this.NotifyPropertyChanged("TargetPrice");
            }
        }

        // 매수가
        private double _targetPrice;
        public string TargetPrice
        {
            get {
                _targetPrice = _dayLowPrice + (_dayHighPrice - _dayLowPrice) * 0.667;
                return string.Format("{0:#,###0}", _targetPrice);
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
                this.NotifyPropertyChanged("TransferCnt");
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

        private long _newTransferPrice;
        public string NewTransferPrice
        {
            get { return string.Format("{0:#,###0}", _newTransferPrice); }
            set
            {
                _newTransferPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("NewTransferPrice");
            }
        }

        // 주문여부
        private string _ordered;

        public string Ordered
        {
            get { return _ordered; }
            set
            {
                _ordered = value;
                this.NotifyPropertyChanged("Ordered");
            }
        }

        private string _detectionTime;

        public string DetectionTime
        {
            get { return _detectionTime; }
            set
            {
                _detectionTime = value;
                this.NotifyPropertyChanged("DetectionTime");
            }
        }

        private int _afterDetection;

        public int AfterDetection
        {
            get { return Convert.ToInt32(((DateTime.Now - DateTime.ParseExact(_detectionTime, "HHmmss", null)).TotalSeconds)); }
        }

        // 최대 주문 초과 여부
        private bool _isOvered;

        public bool IsOvered
        {
            get { return _isOvered; }
            set
            {
                _isOvered = value;
                this.NotifyPropertyChanged("IsOvered");
            }
        }

        public void upTransferCnt()
        {
            _transferCnt += 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DetectionStock(string stockNo, string stockName)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _status = "편입";
            _transferCnt = 1;
            _ordered = "대기";
            _detectionTime = DateTime.Now.ToString("HHmmss");
        }

        public DetectionStock(string stockNo, string stockName, bool isOvered)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _status = "편입";
            _transferCnt = 1;
            _ordered = "대기";
            _detectionTime = DateTime.Now.ToString("HHmmss");
            _isOvered = isOvered;
        }

        public DetectionStock(string stockNo, string stockName, long currentPrice, float fluctuationRate)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _currentPrice = currentPrice;
            _fluctuationRate = fluctuationRate;
            _status = "편입";
            _transferCnt = 1;
            _ordered = "대기";
            _detectionTime = DateTime.Now.ToString("HHmmss");
        }

        public DetectionStock(string stockNo, string stockName, long currentPrice, float fluctuationRate, string status, int transferCnt)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _currentPrice = currentPrice;
            _fluctuationRate = fluctuationRate;
            _status = status;
            _transferCnt = transferCnt;
            _ordered = "대기";
            _detectionTime = DateTime.Now.ToString("HHmmss");
        }

        public DetectionStock(string stockNo, string stockName, long transferPrice, long currentPrice, float fluctuationRate, string status, int transferCnt)
        {
            _stockNo = stockNo;
            _stockName = stockName;
            _transferPrice = transferPrice;
            _currentPrice = currentPrice;
            _fluctuationRate = fluctuationRate;
            _status = status;
            _transferCnt = transferCnt;
            _ordered = "대기";
            _detectionTime = DateTime.Now.ToString("HHmmss");
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
