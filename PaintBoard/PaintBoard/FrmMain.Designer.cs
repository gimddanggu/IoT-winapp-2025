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
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox5 = new GroupBox();
            BtnCrayon = new Button();
            BtnMarker = new Button();
            BtnBrush = new Button();
            TrbPenSize = new TrackBar();
            TrbEraserSize = new TrackBar();
            groupBox3 = new GroupBox();
            BtnSaveImg = new Button();
            BtnLoadImg = new Button();
            groupBox4 = new GroupBox();
            groupBox6 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)WhiteBoard).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrbPenSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrbEraserSize).BeginInit();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
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
            BtnPenColor.Location = new Point(16, 27);
            BtnPenColor.Name = "BtnPenColor";
            BtnPenColor.Size = new Size(94, 29);
            BtnPenColor.TabIndex = 1;
            BtnPenColor.Text = "펜 색상";
            BtnPenColor.UseVisualStyleBackColor = true;
            BtnPenColor.Click += BtnColor_Click;
            // 
            // BtnEraser
            // 
            BtnEraser.Location = new Point(22, 26);
            BtnEraser.Name = "BtnEraser";
            BtnEraser.Size = new Size(94, 29);
            BtnEraser.TabIndex = 2;
            BtnEraser.Text = "지우개";
            BtnEraser.UseVisualStyleBackColor = true;
            BtnEraser.Click += BtnEraser_Click;
            // 
            // BtnAllClear
            // 
            BtnAllClear.Location = new Point(21, 92);
            BtnAllClear.Name = "BtnAllClear";
            BtnAllClear.Size = new Size(103, 29);
            BtnAllClear.TabIndex = 2;
            BtnAllClear.Text = "전체 지우개";
            BtnAllClear.UseVisualStyleBackColor = true;
            BtnAllClear.Click += BtnAllClear_Click;
            // 
            // BtnPen
            // 
            BtnPen.Location = new Point(17, 44);
            BtnPen.Name = "BtnPen";
            BtnPen.Size = new Size(94, 29);
            BtnPen.TabIndex = 3;
            BtnPen.Text = "펜";
            BtnPen.UseVisualStyleBackColor = true;
            BtnPen.Click += BtnPen_Click;
            // 
            // BtnBackgroundColor
            // 
            BtnBackgroundColor.Location = new Point(141, 92);
            BtnBackgroundColor.Name = "BtnBackgroundColor";
            BtnBackgroundColor.Size = new Size(94, 29);
            BtnBackgroundColor.TabIndex = 3;
            BtnBackgroundColor.Text = "캔버스 색";
            BtnBackgroundColor.UseVisualStyleBackColor = true;
            BtnBackgroundColor.Click += BtnBackgroundColor_Click;
            // 
            // BtnUndo
            // 
            BtnUndo.Location = new Point(942, 352);
            BtnUndo.Margin = new Padding(4);
            BtnUndo.Name = "BtnUndo";
            BtnUndo.Size = new Size(96, 31);
            BtnUndo.TabIndex = 4;
            BtnUndo.Text = "Undo";
            BtnUndo.UseVisualStyleBackColor = true;
            BtnUndo.Click += BtnUndo_Click;
            // 
            // BtnRedo
            // 
            BtnRedo.Location = new Point(1058, 352);
            BtnRedo.Margin = new Padding(4);
            BtnRedo.Name = "BtnRedo";
            BtnRedo.Size = new Size(96, 31);
            BtnRedo.TabIndex = 5;
            BtnRedo.Text = "Redo";
            BtnRedo.UseVisualStyleBackColor = true;
            BtnRedo.Click += BtnRedo_Click;
            // 
            // BtnRectangle
            // 
            BtnRectangle.Location = new Point(8, 29);
            BtnRectangle.Margin = new Padding(4);
            BtnRectangle.Name = "BtnRectangle";
            BtnRectangle.Size = new Size(58, 31);
            BtnRectangle.TabIndex = 6;
            BtnRectangle.Text = "네모";
            BtnRectangle.UseVisualStyleBackColor = true;
            BtnRectangle.Click += BtnRectangle_Click;
            // 
            // BtnTriangle
            // 
            BtnTriangle.Location = new Point(73, 29);
            BtnTriangle.Margin = new Padding(4);
            BtnTriangle.Name = "BtnTriangle";
            BtnTriangle.Size = new Size(72, 31);
            BtnTriangle.TabIndex = 6;
            BtnTriangle.Text = "세모";
            BtnTriangle.UseVisualStyleBackColor = true;
            BtnTriangle.Click += BtnTriangle_Click;
            // 
            // BtnCircle
            // 
            BtnCircle.Location = new Point(165, 29);
            BtnCircle.Margin = new Padding(4);
            BtnCircle.Name = "BtnCircle";
            BtnCircle.Size = new Size(62, 31);
            BtnCircle.TabIndex = 6;
            BtnCircle.Text = "원";
            BtnCircle.UseVisualStyleBackColor = true;
            BtnCircle.Click += BtnCircle_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(BtnRectangle);
            groupBox1.Controls.Add(BtnCircle);
            groupBox1.Controls.Add(BtnTriangle);
            groupBox1.Location = new Point(657, 323);
            groupBox1.Margin = new Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new Size(257, 73);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "도형";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox5);
            groupBox2.Controls.Add(TrbPenSize);
            groupBox2.Controls.Add(BtnPenColor);
            groupBox2.Location = new Point(657, 16);
            groupBox2.Margin = new Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4);
            groupBox2.Size = new Size(504, 163);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "groupBox2";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(BtnCrayon);
            groupBox5.Controls.Add(BtnMarker);
            groupBox5.Controls.Add(BtnPen);
            groupBox5.Controls.Add(BtnBrush);
            groupBox5.Location = new Point(13, 62);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(484, 94);
            groupBox5.TabIndex = 5;
            groupBox5.TabStop = false;
            groupBox5.Text = "groupBox5";
            // 
            // BtnCrayon
            // 
            BtnCrayon.Location = new Point(353, 44);
            BtnCrayon.Name = "BtnCrayon";
            BtnCrayon.Size = new Size(94, 29);
            BtnCrayon.TabIndex = 0;
            BtnCrayon.Text = "크레용";
            BtnCrayon.UseVisualStyleBackColor = true;
            BtnCrayon.Click += BtnCrayon_Click;
            // 
            // BtnMarker
            // 
            BtnMarker.Location = new Point(248, 44);
            BtnMarker.Name = "BtnMarker";
            BtnMarker.Size = new Size(94, 29);
            BtnMarker.TabIndex = 0;
            BtnMarker.Text = "마커";
            BtnMarker.UseVisualStyleBackColor = true;
            BtnMarker.Click += BtnMarker_Click;
            // 
            // BtnBrush
            // 
            BtnBrush.Location = new Point(128, 44);
            BtnBrush.Name = "BtnBrush";
            BtnBrush.Size = new Size(94, 29);
            BtnBrush.TabIndex = 0;
            BtnBrush.Text = "브러쉬";
            BtnBrush.UseVisualStyleBackColor = true;
            BtnBrush.Click += BtnBrush_Click;
            // 
            // TrbPenSize
            // 
            TrbPenSize.Location = new Point(116, 27);
            TrbPenSize.Maximum = 20;
            TrbPenSize.Minimum = 2;
            TrbPenSize.Name = "TrbPenSize";
            TrbPenSize.Size = new Size(381, 56);
            TrbPenSize.TabIndex = 4;
            TrbPenSize.Value = 2;
            TrbPenSize.Scroll += trackBar1_Scroll;
            TrbPenSize.ValueChanged += TrbPenSize_ValueChanged;
            // 
            // TrbEraserSize
            // 
            TrbEraserSize.LargeChange = 10;
            TrbEraserSize.Location = new Point(128, 26);
            TrbEraserSize.Maximum = 50;
            TrbEraserSize.Minimum = 10;
            TrbEraserSize.Name = "TrbEraserSize";
            TrbEraserSize.Size = new Size(369, 56);
            TrbEraserSize.SmallChange = 5;
            TrbEraserSize.TabIndex = 4;
            TrbEraserSize.TickFrequency = 5;
            TrbEraserSize.Value = 10;
            TrbEraserSize.ValueChanged += TrbEraserSize_ValueChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(BtnSaveImg);
            groupBox3.Controls.Add(BtnLoadImg);
            groupBox3.Location = new Point(664, 416);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(250, 125);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "파일";
            // 
            // BtnSaveImg
            // 
            BtnSaveImg.Location = new Point(23, 70);
            BtnSaveImg.Name = "BtnSaveImg";
            BtnSaveImg.Size = new Size(94, 29);
            BtnSaveImg.TabIndex = 0;
            BtnSaveImg.Text = "Save";
            BtnSaveImg.UseVisualStyleBackColor = true;
            // 
            // BtnLoadImg
            // 
            BtnLoadImg.Location = new Point(23, 26);
            BtnLoadImg.Name = "BtnLoadImg";
            BtnLoadImg.Size = new Size(94, 29);
            BtnLoadImg.TabIndex = 0;
            BtnLoadImg.Text = "Load";
            BtnLoadImg.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(TrbEraserSize);
            groupBox4.Controls.Add(BtnEraser);
            groupBox4.Controls.Add(BtnAllClear);
            groupBox4.Controls.Add(BtnBackgroundColor);
            groupBox4.Location = new Point(657, 189);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(504, 127);
            groupBox4.TabIndex = 10;
            groupBox4.TabStop = false;
            groupBox4.Text = "groupBox4";
            // 
            // groupBox6
            // 
            groupBox6.Location = new Point(942, 416);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(212, 125);
            groupBox6.TabIndex = 11;
            groupBox6.TabStop = false;
            groupBox6.Text = "레이어";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1174, 600);
            Controls.Add(groupBox6);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(BtnRedo);
            Controls.Add(BtnUndo);
            Controls.Add(WhiteBoard);
            Margin = new Padding(4);
            Name = "FrmMain";
            Text = "그림판";
            Load += FrmMain_Load;
            ((System.ComponentModel.ISupportInitialize)WhiteBoard).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)TrbPenSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrbEraserSize).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
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
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TrackBar TrbPenSize;
        private TrackBar TrbEraserSize;
        private GroupBox groupBox3;
        private Button BtnSaveImg;
        private Button BtnLoadImg;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private Button BtnCrayon;
        private Button BtnMarker;
        private Button BtnBrush;
        private GroupBox groupBox6;
    }
}
