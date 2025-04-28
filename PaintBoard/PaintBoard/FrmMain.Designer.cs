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
            BtnUndo = new Button();
            BtnRedo = new Button();
            BtnRectangle = new Button();
            BtnTriangle = new Button();
            BtnCircle = new Button();
            ((System.ComponentModel.ISupportInitialize)WhiteBoard).BeginInit();
            SuspendLayout();
            // 
            // WhiteBoard
            // 
            WhiteBoard.BackColor = Color.White;
            WhiteBoard.Location = new Point(12, 12);
            WhiteBoard.Name = "WhiteBoard";
            WhiteBoard.Size = new Size(487, 426);
            WhiteBoard.TabIndex = 0;
            WhiteBoard.TabStop = false;
            WhiteBoard.MouseDown += WhiteBoard_MouseDown;
            WhiteBoard.MouseMove += WhiteBoard_MouseMove;
            WhiteBoard.MouseUp += WhiteBoard_MouseUp;
            // 
            // BtnPenColor
            // 
            BtnPenColor.Location = new Point(504, 38);
            BtnPenColor.Margin = new Padding(2);
            BtnPenColor.Name = "BtnPenColor";
            BtnPenColor.Size = new Size(73, 22);
            BtnPenColor.TabIndex = 1;
            BtnPenColor.Text = "펜 색상";
            BtnPenColor.UseVisualStyleBackColor = true;
            BtnPenColor.Click += BtnColor_Click;
            // 
            // BtnEraser
            // 
            BtnEraser.Location = new Point(588, 12);
            BtnEraser.Margin = new Padding(2);
            BtnEraser.Name = "BtnEraser";
            BtnEraser.Size = new Size(73, 22);
            BtnEraser.TabIndex = 2;
            BtnEraser.Text = "지우개";
            BtnEraser.UseVisualStyleBackColor = true;
            BtnEraser.Click += BtnEraser_Click;
            // 
            // BtnAllClear
            // 
            BtnAllClear.Location = new Point(674, 12);
            BtnAllClear.Margin = new Padding(2);
            BtnAllClear.Name = "BtnAllClear";
            BtnAllClear.Size = new Size(80, 22);
            BtnAllClear.TabIndex = 2;
            BtnAllClear.Text = "전체 지우개";
            BtnAllClear.UseVisualStyleBackColor = true;
            BtnAllClear.Click += BtnAllClear_Click;
            // 
            // BtnPen
            // 
            BtnPen.Location = new Point(504, 12);
            BtnPen.Margin = new Padding(2);
            BtnPen.Name = "BtnPen";
            BtnPen.Size = new Size(73, 22);
            BtnPen.TabIndex = 3;
            BtnPen.Text = "펜";
            BtnPen.UseVisualStyleBackColor = true;
            BtnPen.Click += BtnPen_Click;
            // 
            // BtnBackgroundColor
            // 
            BtnBackgroundColor.Location = new Point(588, 38);
            BtnBackgroundColor.Margin = new Padding(2);
            BtnBackgroundColor.Name = "BtnBackgroundColor";
            BtnBackgroundColor.Size = new Size(73, 22);
            BtnBackgroundColor.TabIndex = 3;
            BtnBackgroundColor.Text = "캔버스 색";
            BtnBackgroundColor.UseVisualStyleBackColor = true;
            BtnBackgroundColor.Click += BtnBackgroundColor_Click;
            // 
            // BtnUndo
            // 
            BtnUndo.Location = new Point(505, 76);
            BtnUndo.Name = "BtnUndo";
            BtnUndo.Size = new Size(75, 23);
            BtnUndo.TabIndex = 4;
            BtnUndo.Text = "Undo";
            BtnUndo.UseVisualStyleBackColor = true;
            BtnUndo.Click += BtnUndo_Click;
            // 
            // BtnRedo
            // 
            BtnRedo.Location = new Point(586, 76);
            BtnRedo.Name = "BtnRedo";
            BtnRedo.Size = new Size(75, 23);
            BtnRedo.TabIndex = 5;
            BtnRedo.Text = "Redo";
            BtnRedo.UseVisualStyleBackColor = true;
            BtnRedo.Click += BtnRedo_Click;
            // 
            // BtnRectangle
            // 
            BtnRectangle.Location = new Point(502, 129);
            BtnRectangle.Name = "BtnRectangle";
            BtnRectangle.Size = new Size(75, 23);
            BtnRectangle.TabIndex = 6;
            BtnRectangle.Text = "네모";
            BtnRectangle.UseVisualStyleBackColor = true;
            BtnRectangle.Click += BtnRectangle_Click;
            // 
            // BtnTriangle
            // 
            BtnTriangle.Location = new Point(583, 129);
            BtnTriangle.Name = "BtnTriangle";
            BtnTriangle.Size = new Size(75, 23);
            BtnTriangle.TabIndex = 6;
            BtnTriangle.Text = "세모";
            BtnTriangle.UseVisualStyleBackColor = true;
            BtnTriangle.Click += BtnTriangle_Click;
            // 
            // BtnCircle
            // 
            BtnCircle.Location = new Point(664, 129);
            BtnCircle.Name = "BtnCircle";
            BtnCircle.Size = new Size(75, 23);
            BtnCircle.TabIndex = 6;
            BtnCircle.Text = "원";
            BtnCircle.UseVisualStyleBackColor = true;
            BtnCircle.Click += BtnCircle_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BtnCircle);
            Controls.Add(BtnTriangle);
            Controls.Add(BtnRectangle);
            Controls.Add(BtnRedo);
            Controls.Add(BtnUndo);
            Controls.Add(BtnBackgroundColor);
            Controls.Add(BtnPen);
            Controls.Add(BtnAllClear);
            Controls.Add(BtnEraser);
            Controls.Add(BtnPenColor);
            Controls.Add(WhiteBoard);
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
        private Button BtnUndo;
        private Button BtnRedo;
        private Button BtnRectangle;
        private Button BtnTriangle;
        private Button BtnCircle;
    }
}
