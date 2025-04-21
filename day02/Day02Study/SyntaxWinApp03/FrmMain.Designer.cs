namespace SyntaxWinApp03
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LblPain = new Label();
            TxtPain = new TextBox();
            label1 = new Label();
            CboPainPoint = new ComboBox();
            BtnMsg = new Button();
            BtnDisplay = new Button();
            TxtResult = new TextBox();
            BtnWhile = new Button();
            SuspendLayout();
            // 
            // LblPain
            // 
            LblPain.AutoSize = true;
            LblPain.Location = new Point(12, 9);
            LblPain.Name = "LblPain";
            LblPain.Size = new Size(64, 15);
            LblPain.TabIndex = 0;
            LblPain.Text = "통증여부 -";
            // 
            // TxtPain
            // 
            TxtPain.Location = new Point(82, 6);
            TxtPain.Name = "TxtPain";
            TxtPain.PlaceholderText = "네 또는 아니오";
            TxtPain.Size = new Size(100, 23);
            TxtPain.TabIndex = 1;
            TxtPain.KeyPress += TxtPain_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 45);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 2;
            label1.Text = "통증부위 -";
            // 
            // CboPainPoint
            // 
            CboPainPoint.FormattingEnabled = true;
            CboPainPoint.Items.AddRange(new object[] { "머리", "눈", "코", "목", "가슴", "배" });
            CboPainPoint.Location = new Point(82, 42);
            CboPainPoint.Name = "CboPainPoint";
            CboPainPoint.Size = new Size(121, 23);
            CboPainPoint.TabIndex = 2;
            CboPainPoint.Text = "부위선택";
            CboPainPoint.SelectedIndexChanged += CboPainPoint_SelectedIndexChanged;
            // 
            // BtnMsg
            // 
            BtnMsg.Location = new Point(692, 398);
            BtnMsg.Name = "BtnMsg";
            BtnMsg.Size = new Size(100, 40);
            BtnMsg.TabIndex = 6;
            BtnMsg.Text = "메시지";
            BtnMsg.UseVisualStyleBackColor = true;
            BtnMsg.Click += BtnMsg_Click;
            // 
            // BtnDisplay
            // 
            BtnDisplay.Location = new Point(586, 398);
            BtnDisplay.Name = "BtnDisplay";
            BtnDisplay.Size = new Size(100, 40);
            BtnDisplay.TabIndex = 5;
            BtnDisplay.Text = "구구단";
            BtnDisplay.UseVisualStyleBackColor = true;
            BtnDisplay.Click += BtnDisplay_Click;
            // 
            // TxtResult
            // 
            TxtResult.Location = new Point(12, 71);
            TxtResult.Multiline = true;
            TxtResult.Name = "TxtResult";
            TxtResult.Size = new Size(780, 321);
            TxtResult.TabIndex = 3;
            // 
            // BtnWhile
            // 
            BtnWhile.Location = new Point(480, 398);
            BtnWhile.Name = "BtnWhile";
            BtnWhile.Size = new Size(100, 40);
            BtnWhile.TabIndex = 4;
            BtnWhile.Text = "반복";
            BtnWhile.UseVisualStyleBackColor = true;
            BtnWhile.Click += BtnWhile_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BtnWhile);
            Controls.Add(TxtResult);
            Controls.Add(BtnDisplay);
            Controls.Add(BtnMsg);
            Controls.Add(CboPainPoint);
            Controls.Add(label1);
            Controls.Add(TxtPain);
            Controls.Add(LblPain);
            Name = "FrmMain";
            Text = "문법학습 윈앱03";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LblPain;
        private TextBox TxtPain;
        private Label label1;
        private ComboBox CboPainPoint;
        private Button BtnMsg;
        private Button BtnDisplay;
        private TextBox TxtResult;
        private Button BtnWhile;
    }
}
