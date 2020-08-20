namespace FourCommaTrader
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonLogin = new System.Windows.Forms.Button();
            this.comboConditionList = new System.Windows.Forms.ComboBox();
            this.dataDetectCondition = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.dataOrder = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataHolding = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxSystemLog = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxTradingLog = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.labelEstimatedBalance = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.kiwoomApi = new AxKHOpenAPILib.AxKHOpenAPI();
            this.comboAccount = new System.Windows.Forms.ComboBox();
            this.buttonStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataDetectCondition)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataOrder)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataHolding)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kiwoomApi)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(1001, 12);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(124, 39);
            this.buttonLogin.TabIndex = 0;
            this.buttonLogin.Text = "로그인";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // comboConditionList
            // 
            this.comboConditionList.Enabled = false;
            this.comboConditionList.FormattingEnabled = true;
            this.comboConditionList.Location = new System.Drawing.Point(17, 18);
            this.comboConditionList.Margin = new System.Windows.Forms.Padding(4);
            this.comboConditionList.Name = "comboConditionList";
            this.comboConditionList.Size = new System.Drawing.Size(230, 26);
            this.comboConditionList.TabIndex = 1;
            this.comboConditionList.Text = "조건식 선택";
            this.comboConditionList.SelectedIndexChanged += new System.EventHandler(this.comboConditionList_SelectedIndexChanged);
            // 
            // dataDetectCondition
            // 
            this.dataDetectCondition.AllowUserToAddRows = false;
            this.dataDetectCondition.AllowUserToDeleteRows = false;
            this.dataDetectCondition.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataDetectCondition.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataDetectCondition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataDetectCondition.Location = new System.Drawing.Point(6, 27);
            this.dataDetectCondition.MultiSelect = false;
            this.dataDetectCondition.Name = "dataDetectCondition";
            this.dataDetectCondition.ReadOnly = true;
            this.dataDetectCondition.RowHeadersVisible = false;
            this.dataDetectCondition.RowHeadersWidth = 62;
            this.dataDetectCondition.RowTemplate.Height = 30;
            this.dataDetectCondition.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataDetectCondition.Size = new System.Drawing.Size(1097, 148);
            this.dataDetectCondition.TabIndex = 15;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dataDetectCondition);
            this.groupBox5.Location = new System.Drawing.Point(17, 56);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1109, 182);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "감지 종목";
            // 
            // buttonStart
            // 
            this.buttonStart.Enabled = false;
            this.buttonStart.Location = new System.Drawing.Point(257, 12);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(96, 39);
            this.buttonStart.TabIndex = 19;
            this.buttonStart.Text = "시작";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // dataOrder
            // 
            this.dataOrder.AllowUserToAddRows = false;
            this.dataOrder.AllowUserToDeleteRows = false;
            this.dataOrder.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataOrder.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataOrder.Location = new System.Drawing.Point(6, 27);
            this.dataOrder.MultiSelect = false;
            this.dataOrder.Name = "dataOrder";
            this.dataOrder.ReadOnly = true;
            this.dataOrder.RowHeadersVisible = false;
            this.dataOrder.RowHeadersWidth = 62;
            this.dataOrder.RowTemplate.Height = 30;
            this.dataOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataOrder.Size = new System.Drawing.Size(1097, 148);
            this.dataOrder.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataOrder);
            this.groupBox1.Location = new System.Drawing.Point(17, 243);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1109, 182);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "주문 상태";
            // 
            // dataHolding
            // 
            this.dataHolding.AllowUserToAddRows = false;
            this.dataHolding.AllowUserToDeleteRows = false;
            this.dataHolding.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataHolding.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataHolding.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataHolding.Location = new System.Drawing.Point(6, 27);
            this.dataHolding.MultiSelect = false;
            this.dataHolding.Name = "dataHolding";
            this.dataHolding.ReadOnly = true;
            this.dataHolding.RowHeadersVisible = false;
            this.dataHolding.RowHeadersWidth = 62;
            this.dataHolding.RowTemplate.Height = 30;
            this.dataHolding.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataHolding.Size = new System.Drawing.Size(1097, 148);
            this.dataHolding.TabIndex = 15;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataHolding);
            this.groupBox2.Location = new System.Drawing.Point(17, 430);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1109, 182);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "잔고";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(16, 618);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1104, 206);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxSystemLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1096, 174);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "시스템로그";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxSystemLog
            // 
            this.textBoxSystemLog.Location = new System.Drawing.Point(6, 6);
            this.textBoxSystemLog.Multiline = true;
            this.textBoxSystemLog.Name = "textBoxSystemLog";
            this.textBoxSystemLog.ReadOnly = true;
            this.textBoxSystemLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSystemLog.Size = new System.Drawing.Size(1080, 152);
            this.textBoxSystemLog.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxTradingLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1096, 174);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "매매로그";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxTradingLog
            // 
            this.textBoxTradingLog.Location = new System.Drawing.Point(6, 6);
            this.textBoxTradingLog.Multiline = true;
            this.textBoxTradingLog.Name = "textBoxTradingLog";
            this.textBoxTradingLog.ReadOnly = true;
            this.textBoxTradingLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTradingLog.Size = new System.Drawing.Size(1080, 152);
            this.textBoxTradingLog.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(934, 22);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(26, 18);
            this.label13.TabIndex = 24;
            this.label13.Text = "원";
            // 
            // labelEstimatedBalance
            // 
            this.labelEstimatedBalance.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEstimatedBalance.Location = new System.Drawing.Point(791, 22);
            this.labelEstimatedBalance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEstimatedBalance.Name = "labelEstimatedBalance";
            this.labelEstimatedBalance.Size = new System.Drawing.Size(136, 18);
            this.labelEstimatedBalance.TabIndex = 23;
            this.labelEstimatedBalance.Text = "0";
            this.labelEstimatedBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(673, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 18);
            this.label5.TabIndex = 22;
            this.label5.Text = "추정예탁자산";
            // 
            // kiwoomApi
            // 
            this.kiwoomApi.Enabled = true;
            this.kiwoomApi.Location = new System.Drawing.Point(791, 0);
            this.kiwoomApi.Margin = new System.Windows.Forms.Padding(4);
            this.kiwoomApi.Name = "kiwoomApi";
            this.kiwoomApi.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("kiwoomApi.OcxState")));
            this.kiwoomApi.Size = new System.Drawing.Size(15, 15);
            this.kiwoomApi.TabIndex = 1;
            this.kiwoomApi.Visible = false;
            this.kiwoomApi.OnReceiveTrData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEventHandler(this.kiwoomApi_OnReceiveTrData);
            this.kiwoomApi.OnReceiveRealData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEventHandler(this.kiwoomApi_OnReceiveRealData);
            this.kiwoomApi.OnReceiveMsg += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEventHandler(this.kiwoomApi_OnReceiveMsg);
            this.kiwoomApi.OnReceiveChejanData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEventHandler(this.kiwoomApi_OnReceiveChejanData);
            this.kiwoomApi.OnEventConnect += new AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEventHandler(this.kiwoomApi_OnEventConnect);
            this.kiwoomApi.OnReceiveRealCondition += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEventHandler(this.kiwoomApi_OnReceiveRealCondition);
            this.kiwoomApi.OnReceiveTrCondition += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEventHandler(this.kiwoomApi_OnReceiveTrCondition);
            this.kiwoomApi.OnReceiveConditionVer += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEventHandler(this.kiwoomApi_OnReceiveConditionVer);
            // 
            // comboAccount
            // 
            this.comboAccount.Enabled = false;
            this.comboAccount.FormattingEnabled = true;
            this.comboAccount.Location = new System.Drawing.Point(517, 18);
            this.comboAccount.Margin = new System.Windows.Forms.Padding(4);
            this.comboAccount.Name = "comboAccount";
            this.comboAccount.Size = new System.Drawing.Size(145, 26);
            this.comboAccount.TabIndex = 25;
            this.comboAccount.Text = "계좌번호 선택";
            this.comboAccount.SelectedIndexChanged += new System.EventHandler(this.comboAccount_SelectedIndexChanged);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(361, 13);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(96, 39);
            this.buttonStop.TabIndex = 26;
            this.buttonStop.Text = "중지";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 836);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.comboAccount);
            this.Controls.Add(this.kiwoomApi);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.labelEstimatedBalance);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.comboConditionList);
            this.Controls.Add(this.buttonLogin);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "조건식 매매 프로그램";
            ((System.ComponentModel.ISupportInitialize)(this.dataDetectCondition)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataOrder)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataHolding)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kiwoomApi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.ComboBox comboConditionList;
        private System.Windows.Forms.DataGridView dataDetectCondition;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.DataGridView dataOrder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataHolding;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBoxSystemLog;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxTradingLog;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelEstimatedBalance;
        private System.Windows.Forms.Label label5;
        private AxKHOpenAPILib.AxKHOpenAPI kiwoomApi;
        private System.Windows.Forms.ComboBox comboAccount;
        private System.Windows.Forms.Button buttonStop;
    }
}

