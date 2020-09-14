using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourCommaTrader
{
    // 보유종목
    public class OrderStock : INotifyPropertyChanged
    {
        // 주문번호,종목코드,주문상태,종목명,주문수량,주문가격,미체결수량
        // 종목번호
        private string _orderNo;

        public string OrderNo
        {
            get { return _orderNo; }
            set
            {
                _orderNo = value;
                this.NotifyPropertyChanged("OrderNo");
            }
        }

        // 종목번호
        private string _stockNo;

        public string StockNo {
            get { return _stockNo; }
            set { 
                _stockNo = value.Trim().Replace("A", "");
                this.NotifyPropertyChanged("StockNo");
            }
        }
        
        // 주문상태
        private string _orderStatus;

        public string OrderStatus
        {
            get { return _orderStatus; }
            set
            {
                _orderStatus = value;
                this.NotifyPropertyChanged("OrderStatus");
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

        // 주문수량
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

        // 주문가격
        private long _orderPrice;
        public string OrderPrice
        {
            get { return string.Format("{0:#,###0}", _orderPrice); }
            set
            {
                _orderPrice = long.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("OrderPrice");
            }
        }

        // 미체결수량
        private int _unclosedQty;

        public string UnclosedQty
        {
            get { return string.Format("{0:#,###0}", _unclosedQty); }
            set
            {
                _unclosedQty = int.Parse(value, System.Globalization.NumberStyles.AllowThousands);
                this.NotifyPropertyChanged("UnclosedQty");
            }
        }

        // 주문구분
        private string _orderType;

        public string OrderType
        {
            get { return _orderType; }
            set
            {
                _orderType = value;
                this.NotifyPropertyChanged("OrderType");
            }
        }

        // 주문/체결시간
        private string _orderTime;

        public string OrderTime
        {
            get { return _orderTime; }
            set
            {
                _orderTime = value;
                this.NotifyPropertyChanged("OrderTime");
            }
        }

        private int _afterOrder;

        public int AfterOrder
        {
            get { return Convert.ToInt32(((DateTime.Now - DateTime.ParseExact(_orderTime, "HHmmss", null)).TotalSeconds)); }
        }

        private bool _canceled = false;

        public bool Canceled
        {
            get { return _canceled; }
            set { _canceled = value; }
        }

        private string _orgOrderNo;

        public string OrgOrderNo
        {
            get { return _orgOrderNo; }
            set
            {
                _orgOrderNo = value;
                this.NotifyPropertyChanged("OrgOrderNo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OrderStock(string orderNo, string stockNo, string orderStatus, string stockName, int qty, long orderPrice, int unclosedQty, string orderType)
        {
            _orderNo = orderNo;
            _stockNo = stockNo;
            _orderStatus = orderStatus;
            _stockName = stockName;
            _qty = qty;
            _orderPrice = orderPrice;
            _unclosedQty = unclosedQty;
            _orderType = orderType;
        }

        public OrderStock(string orderNo, string stockNo, string orderStatus, string stockName, int qty, long orderPrice, int unclosedQty, string orderType, string orderTime)
        {
            _orderNo = orderNo;
            _stockNo = stockNo;
            _orderStatus = orderStatus;
            _stockName = stockName;
            _qty = qty;
            _orderPrice = orderPrice;
            _unclosedQty = unclosedQty;
            _orderType = orderType;
            _orderTime = orderTime;
        }

        public OrderStock(string orderNo, string stockNo, string orderStatus, string stockName, int qty, long orderPrice, int unclosedQty, string orderType, string orderTime, string orgOrderNo)
        {
            _orderNo = orderNo;
            _stockNo = stockNo;
            _orderStatus = orderStatus;
            _stockName = stockName;
            _qty = qty;
            _orderPrice = orderPrice;
            _unclosedQty = unclosedQty;
            _orderType = orderType;
            _orderTime = orderTime;
            _orgOrderNo = orgOrderNo;
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
