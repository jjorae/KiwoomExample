using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwoomExample
{
    class Stock
    {
        public string stockNo { get; set; }
        public string stockName { get; set; }

        public Stock() { }
        public Stock(string stockNo, string stockName)
        {
            this.stockNo = stockNo;
            this.stockName = stockName;
        }

        public override string ToString()
        {
            return stockName + "(" + stockNo + ")";
        }
    }
}
