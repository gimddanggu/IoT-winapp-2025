namespace PaintBoard
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
            WhiteBoard = new PictureBox();
            DlgColor = new ColorDialog();
            BtnPenColor = new Button();
            BtnEraser = new Button();
            BtnAllClear = new Button();
            BtnPen = new Button();
            BtnBackgroundColor = new Button();
            ((System.ComponentModel.ISupportInitialize)WhiteBoard).BeginInit();
            SuspendLayout();
            // 
            // WhiteBoard
            // 
            WhiteBoard.BackColor = Color.White;
            WhiteBoard.Location = new Point(15, 16);
            WhiteBoard.Margin = new Padding(4);
            WhiteBoard.Name = "WhiteBoard";
            WhiteBoard.Size = new Size(626, 568);
            WhiteBoard.TabIndex = 0;
            WhiteBoard.TabStop = false;
            WhiteBoard.MouseDown += WhiteBoard_MouseDown;
            WhiteBoard.MouseMove += WhiteBoard_MouseMove;
            WhiteBoard.MouseUp += WhiteBoard_MouseUp;
            // 
            // BtnPenColor
            // 
            BtnPenColor.Location = new Point(648, 51);
            BtnPenColor.Name = "BtnPenColor";
            BtnPenColor.Size = new Size(94, 29);
            BtnPenColor.TabIndex = 1;
            BtnPenColor.Text = "펜 색상";
            BtnPenColor.UseVisualStyleBackColor = true;
            BtnPenColor.Click += BtnColor_Click;
            // 
            // BtnEraser
            // 
            BtnEraser.Location = new Point(748, 16);
            BtnEraser.Name = "BtnEraser";
            BtnEraser.Size = new Size(94, 29);
            BtnEraser.TabIndex = 2;
            BtnEraser.Text = "지우개";
            BtnEraser.UseVisualStyleBackColor = true;
            BtnEraser.Click += BtnEraser_Click;
            // 
            // BtnAllClear
            // 
            BtnAllClear.Location = new Point(848, 16);
            BtnAllClear.Name = "BtnAllClear";
            BtnAllClear.Size = new Size(103, 29);
            BtnAllClear.TabIndex = 2;
            BtnAllClear.Text = "전체 지우개";
            BtnAllClear.UseVisualStyleBackColor = true;
            BtnAllClear.Click += BtnAllClear_Click;
            // 
            // BtnPen
            // 
            BtnPen.Location = new Point(648, 16);
            BtnPen.Name = "BtnPen";
            BtnPen.Size = new Size(94, 29);
            BtnPen.TabIndex = 3;
            BtnPen.Text = "펜";
            BtnPen.UseVisualStyleBackColor = true;
            BtnPen.Click += BtnPen_Click;
            // 
            // BtnBackgroundColor
            // 
            BtnBackgroundColor.Location = new Point(748, 51);
            BtnBackgroundColor.Name = "BtnBackgroundColor";
            BtnBackgroundColor.Size = new Size(94, 29);
            BtnBackgroundColor.TabIndex = 3;
            BtnBackgroundColor.Text = "캔버스 색";
            BtnBackgroundColor.UseVisualStyleBackColor = true;
            BtnBackgroundColor.Click += BtnBackgroundColor_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1029, 600);
            Controls.Add(BtnBackgroundColor);
            Controls.Add(BtnPen);
            Controls.Add(BtnAllClear);
            Controls.Add(BtnEraser);
            Controls.Add(BtnPenColor);
            Controls.Add(WhiteBoard);
            Margin = new Padding(4);
            Name = "FrmMain";
            Text = "그림판";
            Load += FrmMain_Load;
            ((System.ComponentModel.ISupportInitialize)WhiteBoard).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox WhiteBoard;
        private ColorDialog DlgColor;
        private Button BtnPenColor;
        private Button BtnEraser;
        private Button BtnAllClear;
        private Button BtnPen;
        private Button BtnBackgroundColor;
    }
}
