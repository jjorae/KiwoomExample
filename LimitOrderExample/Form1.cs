using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace LimitOrderExample
{
    public partial class Form1 : Form
    {
        // 주문 넣을 때 이렇게 제한을 두면..큐에 넣었다가 진행할 수 있을듯..
        private System.Timers.Timer aTimer;
        private ConcurrentQueue<int> testQueue;

        private int maxCnt = 10;

        private int idx = 0;
        private int itemHistory = 0;

        public Form1()
        {
            InitializeComponent();

            aTimer = new System.Timers.Timer(1000); // 1초당
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Enabled = true;

            testQueue = new ConcurrentQueue<int>();
        }

        private void enqueue()
        {
            testQueue.Enqueue(itemHistory++);
            idx++;
        }

        private void dequeue()
        {
            int result;

            if (testQueue.TryDequeue(out result))
            {
                Console.WriteLine(result);
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Time {0}", e.SignalTime);

            if (idx > 0 && !testQueue.IsEmpty)
            {
                int loopCnt = maxCnt;

                if (testQueue.Count < maxCnt)
                {
                    loopCnt = testQueue.Count;
                }

                for (int i = 0; i < loopCnt; i++)
                {
                    dequeue();
                }

                if (testQueue.IsEmpty)
                {
                    idx = loopCnt;
                }
            }
            else
            {
                idx = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            enqueue();

            if (idx <= maxCnt)
            {
                dequeue();
            }
        }
    }
}
