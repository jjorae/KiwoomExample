using KiwoomExample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeTheKing
{
    public partial class Form2 : Form
    {
        // 해당 폼에서는 전략을 설정 가능하도록 함
        // 우선 조건검색 선택
        // 저장하면 KiwoomApi에 일단 저장

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Strategy strategy = new Strategy();
            strategy.condition = comboBox2.SelectedItem.ToString();
            KiwoomApi.Instance.saveStrategy(strategy);

            this.Close();
        }
    }
}
