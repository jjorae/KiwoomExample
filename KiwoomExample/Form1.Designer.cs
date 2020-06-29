namespace KiwoomExample
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.kiwoomApi = new AxKHOpenAPILib.AxKHOpenAPI();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.labelAccountList = new System.Windows.Forms.Label();
            this.comboAccountList = new System.Windows.Forms.ComboBox();
            this.labelEstimatedAssets = new System.Windows.Forms.Label();
            this.labelEstimatedAssetsVal = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.estimatedBalance = new System.Windows.Forms.Label();
            this.totalYield = new System.Windows.Forms.Label();
            this.totalValuationAmount = new System.Windows.Forms.Label();
            this.totalEvaluationAmount = new System.Windows.Forms.Label();
            this.totalPurchaseAmount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataHolding = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.kiwoomApi)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataHolding)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 0);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 36);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "로그인";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // kiwoomApi
            // 
            this.kiwoomApi.Enabled = true;
            this.kiwoomApi.Location = new System.Drawing.Point(697, 0);
            this.kiwoomApi.Name = "kiwoomApi";
            this.kiwoomApi.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("kiwoomApi.OcxState")));
            this.kiwoomApi.Size = new System.Drawing.Size(100, 50);
            this.kiwoomApi.TabIndex = 1;
            this.kiwoomApi.OnReceiveTrData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEventHandler(this.kiwoomApi_OnReceiveTrData);
            this.kiwoomApi.OnReceiveRealData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEventHandler(this.kiwoomApi_OnReceiveRealData);
            this.kiwoomApi.OnReceiveMsg += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEventHandler(this.kiwoomApi_OnReceiveMsg);
            this.kiwoomApi.OnEventConnect += new AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEventHandler(this.kiwoomApi_OnEventConnect);
            this.kiwoomApi.OnReceiveRealCondition += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEventHandler(this.kiwoomApi_OnReceiveRealCondition);
            this.kiwoomApi.OnReceiveTrCondition += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEventHandler(this.kiwoomApi_OnReceiveTrCondition);
            this.kiwoomApi.OnReceiveConditionVer += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEventHandler(this.kiwoomApi_OnReceiveConditionVer);
            // 
            // listBoxLog
            // 
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.ItemHeight = 18;
            this.listBoxLog.Location = new System.Drawing.Point(12, 600);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(980, 436);
            this.listBoxLog.TabIndex = 2;
            // 
            // labelAccountList
            // 
            this.labelAccountList.AutoSize = true;
            this.labelAccountList.Location = new System.Drawing.Point(9, 51);
            this.labelAccountList.Name = "labelAccountList";
            this.labelAccountList.Size = new System.Drawing.Size(80, 18);
            this.labelAccountList.TabIndex = 3;
            this.labelAccountList.Text = "계좌목록";
            // 
            // comboAccountList
            // 
            this.comboAccountList.FormattingEnabled = true;
            this.comboAccountList.Location = new System.Drawing.Point(95, 48);
            this.comboAccountList.Name = "comboAccountList";
            this.comboAccountList.Size = new System.Drawing.Size(161, 26);
            this.comboAccountList.TabIndex = 4;
            this.comboAccountList.SelectedIndexChanged += new System.EventHandler(this.comboAccountList_SelectedIndexChanged);
            // 
            // labelEstimatedAssets
            // 
            this.labelEstimatedAssets.AutoSize = true;
            this.labelEstimatedAssets.Location = new System.Drawing.Point(9, 87);
            this.labelEstimatedAssets.Name = "labelEstimatedAssets";
            this.labelEstimatedAssets.Size = new System.Drawing.Size(116, 18);
            this.labelEstimatedAssets.TabIndex = 5;
            this.labelEstimatedAssets.Text = "추정예탁자산";
            // 
            // labelEstimatedAssetsVal
            // 
            this.labelEstimatedAssetsVal.AutoSize = true;
            this.labelEstimatedAssetsVal.Location = new System.Drawing.Point(131, 87);
            this.labelEstimatedAssetsVal.Name = "labelEstimatedAssetsVal";
            this.labelEstimatedAssetsVal.Size = new System.Drawing.Size(36, 18);
            this.labelEstimatedAssetsVal.TabIndex = 6;
            this.labelEstimatedAssetsVal.Text = "0원";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.estimatedBalance);
            this.groupBox1.Controls.Add(this.totalYield);
            this.groupBox1.Controls.Add(this.totalValuationAmount);
            this.groupBox1.Controls.Add(this.totalEvaluationAmount);
            this.groupBox1.Controls.Add(this.totalPurchaseAmount);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(12, 120);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(331, 186);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "계좌정보";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(296, 117);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(22, 18);
            this.label14.TabIndex = 12;
            this.label14.Text = "%";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(294, 146);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(26, 18);
            this.label13.TabIndex = 14;
            this.label13.Text = "원";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(294, 88);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 18);
            this.label12.TabIndex = 13;
            this.label12.Text = "원";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(294, 60);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 18);
            this.label11.TabIndex = 12;
            this.label11.Text = "원";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(294, 32);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(26, 18);
            this.label15.TabIndex = 11;
            this.label15.Text = "원";
            // 
            // estimatedBalance
            // 
            this.estimatedBalance.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.estimatedBalance.Location = new System.Drawing.Point(150, 146);
            this.estimatedBalance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.estimatedBalance.Name = "estimatedBalance";
            this.estimatedBalance.Size = new System.Drawing.Size(136, 18);
            this.estimatedBalance.TabIndex = 10;
            this.estimatedBalance.Text = "0";
            this.estimatedBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalYield
            // 
            this.totalYield.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalYield.Location = new System.Drawing.Point(153, 117);
            this.totalYield.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalYield.Name = "totalYield";
            this.totalYield.Size = new System.Drawing.Size(133, 18);
            this.totalYield.TabIndex = 9;
            this.totalYield.Text = "0.0";
            this.totalYield.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalValuationAmount
            // 
            this.totalValuationAmount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalValuationAmount.Location = new System.Drawing.Point(150, 88);
            this.totalValuationAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalValuationAmount.Name = "totalValuationAmount";
            this.totalValuationAmount.Size = new System.Drawing.Size(136, 18);
            this.totalValuationAmount.TabIndex = 8;
            this.totalValuationAmount.Text = "0";
            this.totalValuationAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalEvaluationAmount
            // 
            this.totalEvaluationAmount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalEvaluationAmount.Location = new System.Drawing.Point(150, 60);
            this.totalEvaluationAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalEvaluationAmount.Name = "totalEvaluationAmount";
            this.totalEvaluationAmount.Size = new System.Drawing.Size(136, 18);
            this.totalEvaluationAmount.TabIndex = 7;
            this.totalEvaluationAmount.Text = "0";
            this.totalEvaluationAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalPurchaseAmount
            // 
            this.totalPurchaseAmount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalPurchaseAmount.Location = new System.Drawing.Point(150, 32);
            this.totalPurchaseAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalPurchaseAmount.Name = "totalPurchaseAmount";
            this.totalPurchaseAmount.Size = new System.Drawing.Size(136, 18);
            this.totalPurchaseAmount.TabIndex = 6;
            this.totalPurchaseAmount.Text = "0";
            this.totalPurchaseAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 146);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "추정예탁자산";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 117);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "총수익률(%)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "총평가손익금액";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "총평가금액";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "총매입금액";
            // 
            // dataHolding
            // 
            this.dataHolding.AllowUserToAddRows = false;
            this.dataHolding.AllowUserToDeleteRows = false;
            this.dataHolding.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataHolding.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataHolding.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataHolding.Location = new System.Drawing.Point(19, 27);
            this.dataHolding.MultiSelect = false;
            this.dataHolding.Name = "dataHolding";
            this.dataHolding.ReadOnly = true;
            this.dataHolding.RowHeadersVisible = false;
            this.dataHolding.RowHeadersWidth = 62;
            this.dataHolding.RowTemplate.Height = 30;
            this.dataHolding.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataHolding.Size = new System.Drawing.Size(934, 227);
            this.dataHolding.TabIndex = 15;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataHolding);
            this.groupBox2.Location = new System.Drawing.Point(12, 313);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(975, 272);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "잔고";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 1055);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelEstimatedAssetsVal);
            this.Controls.Add(this.labelEstimatedAssets);
            this.Controls.Add(this.comboAccountList);
            this.Controls.Add(this.labelAccountList);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.kiwoomApi);
            this.Controls.Add(this.btnLogin);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.kiwoomApi)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataHolding)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private AxKHOpenAPILib.AxKHOpenAPI kiwoomApi;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Label labelAccountList;
        private System.Windows.Forms.ComboBox comboAccountList;
        private System.Windows.Forms.Label labelEstimatedAssets;
        private System.Windows.Forms.Label labelEstimatedAssetsVal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label estimatedBalance;
        private System.Windows.Forms.Label totalYield;
        private System.Windows.Forms.Label totalValuationAmount;
        private System.Windows.Forms.Label totalEvaluationAmount;
        private System.Windows.Forms.Label totalPurchaseAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataHolding;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

