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
            BtnAllDrawClear = new Button();
            BtnPen = new Button();
            BtnBackgroundColor = new Button();
            BtnUndo = new Button();
            BtnRedo = new Button();
            BtnRectangle = new Button();
            BtnTriangle = new Button();
            BtnCircle = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            LblPenSize = new Label();
            groupBox5 = new GroupBox();
            BtnSpray = new Button();
            BtnCrayon = new Button();
            BtnMarker = new Button();
            BtnBrush = new Button();
            TrbPenSize = new TrackBar();
            TrbEraserSize = new TrackBar();
            groupBox3 = new GroupBox();
            BtnNoBgSave = new Button();
            BtnSaveImg = new Button();
            BtnLoadImg = new Button();
            groupBox4 = new GroupBox();
            LblEraserSize = new Label();
            BtnBgEraser = new Button();
            BtnAllClear = new Button();
            BtnAllBgClear = new Button();
            groupBox6 = new GroupBox();
            BtnRectangleFull = new Button();
            BtnCircleFull = new Button();
            BtnTriangleFull = new Button();
            groupBox7 = new GroupBox();
            groupBox8 = new GroupBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)WhiteBoard).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrbPenSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrbEraserSize).BeginInit();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox8.SuspendLayout();
            SuspendLayout();
            // 
            // WhiteBoard
            // 
            WhiteBoard.BackColor = Color.White;
            WhiteBoard.Location = new Point(12, 12);
            WhiteBoard.Name = "WhiteBoard";
            WhiteBoard.Size = new Size(1000, 930);
            WhiteBoard.TabIndex = 0;
            WhiteBoard.TabStop = false;
            WhiteBoard.MouseDown += WhiteBoard_MouseDown;
            WhiteBoard.MouseMove += WhiteBoard_MouseMove;
            WhiteBoard.MouseUp += WhiteBoard_MouseUp;
            // 
            // BtnPenColor
            // 
            BtnPenColor.Location = new Point(16, 51);
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
            BtnEraser.Location = new Point(12, 69);
            BtnEraser.Margin = new Padding(2);
            BtnEraser.Name = "BtnEraser";
            BtnEraser.Size = new Size(80, 22);
            BtnEraser.TabIndex = 2;
            BtnEraser.Text = "지우개";
            BtnEraser.UseVisualStyleBackColor = true;
            BtnEraser.Click += BtnEraser_Click;
            // 
            // BtnAllDrawClear
            // 
            BtnAllDrawClear.Location = new Point(172, 69);
            BtnAllDrawClear.Margin = new Padding(2);
            BtnAllDrawClear.Name = "BtnAllDrawClear";
            BtnAllDrawClear.Size = new Size(80, 22);
            BtnAllDrawClear.TabIndex = 2;
            BtnAllDrawClear.Text = "그림 초기화";
            BtnAllDrawClear.UseVisualStyleBackColor = true;
            BtnAllDrawClear.Click += BtnAllDrawClear_Click;
            // 
            // BtnPen
            // 
            BtnPen.Location = new Point(13, 23);
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
            BtnBackgroundColor.Location = new Point(13, 20);
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
            BtnUndo.Location = new Point(23, 21);
            BtnUndo.Name = "BtnUndo";
            BtnUndo.Size = new Size(73, 22);
            BtnUndo.TabIndex = 4;
            BtnUndo.Text = "Undo";
            BtnUndo.UseVisualStyleBackColor = true;
            BtnUndo.Click += BtnUndo_Click;
            // 
            // BtnRedo
            // 
            BtnRedo.Location = new Point(113, 21);
            BtnRedo.Name = "BtnRedo";
            BtnRedo.Size = new Size(73, 22);
            BtnRedo.TabIndex = 5;
            BtnRedo.Text = "Redo";
            BtnRedo.UseVisualStyleBackColor = true;
            BtnRedo.Click += BtnRedo_Click;
            // 
            // BtnRectangle
            // 
            BtnRectangle.Location = new Point(26, 22);
            BtnRectangle.Name = "BtnRectangle";
            BtnRectangle.Size = new Size(45, 23);
            BtnRectangle.TabIndex = 6;
            BtnRectangle.Text = "네모";
            BtnRectangle.UseVisualStyleBackColor = true;
            BtnRectangle.Click += BtnRectangle_Click;
            // 
            // BtnTriangle
            // 
            BtnTriangle.Location = new Point(82, 22);
            BtnTriangle.Name = "BtnTriangle";
            BtnTriangle.Size = new Size(45, 23);
            BtnTriangle.TabIndex = 6;
            BtnTriangle.Text = "세모";
            BtnTriangle.UseVisualStyleBackColor = true;
            BtnTriangle.Click += BtnTriangle_Click;
            // 
            // BtnCircle
            // 
            BtnCircle.Location = new Point(138, 22);
            BtnCircle.Name = "BtnCircle";
            BtnCircle.Size = new Size(45, 23);
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
            groupBox1.Location = new Point(1023, 316);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(208, 55);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "도형";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(LblPenSize);
            groupBox2.Controls.Add(groupBox5);
            groupBox2.Controls.Add(TrbPenSize);
            groupBox2.Controls.Add(BtnPenColor);
            groupBox2.Location = new Point(1023, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(425, 150);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "펜";
            // 
            // LblPenSize
            // 
            LblPenSize.AutoSize = true;
            LblPenSize.Location = new Point(42, 25);
            LblPenSize.Name = "LblPenSize";
            LblPenSize.Size = new Size(65, 15);
            LblPenSize.TabIndex = 6;
            LblPenSize.Text = "펜 크기 : 5";
            LblPenSize.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(BtnSpray);
            groupBox5.Controls.Add(BtnCrayon);
            groupBox5.Controls.Add(BtnMarker);
            groupBox5.Controls.Add(BtnPen);
            groupBox5.Controls.Add(BtnBrush);
            groupBox5.Location = new Point(5, 86);
            groupBox5.Margin = new Padding(2);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(2);
            groupBox5.Size = new Size(410, 59);
            groupBox5.TabIndex = 5;
            groupBox5.TabStop = false;
            groupBox5.Text = "펜 종류";
            // 
            // BtnSpray
            // 
            BtnSpray.Location = new Point(321, 24);
            BtnSpray.Name = "BtnSpray";
            BtnSpray.Size = new Size(75, 23);
            BtnSpray.TabIndex = 4;
            BtnSpray.Text = "스프레이";
            BtnSpray.UseVisualStyleBackColor = true;
            BtnSpray.Click += BtnSpray_Click;
            // 
            // BtnCrayon
            // 
            BtnCrayon.Location = new Point(244, 23);
            BtnCrayon.Margin = new Padding(2);
            BtnCrayon.Name = "BtnCrayon";
            BtnCrayon.Size = new Size(73, 22);
            BtnCrayon.TabIndex = 0;
            BtnCrayon.Text = "크레용";
            BtnCrayon.UseVisualStyleBackColor = true;
            BtnCrayon.Click += BtnCrayon_Click;
            // 
            // BtnMarker
            // 
            BtnMarker.Location = new Point(167, 23);
            BtnMarker.Margin = new Padding(2);
            BtnMarker.Name = "BtnMarker";
            BtnMarker.Size = new Size(73, 22);
            BtnMarker.TabIndex = 0;
            BtnMarker.Text = "마커";
            BtnMarker.UseVisualStyleBackColor = true;
            BtnMarker.Click += BtnMarker_Click;
            // 
            // BtnBrush
            // 
            BtnBrush.Location = new Point(90, 23);
            BtnBrush.Margin = new Padding(2);
            BtnBrush.Name = "BtnBrush";
            BtnBrush.Size = new Size(73, 22);
            BtnBrush.TabIndex = 0;
            BtnBrush.Text = "브러쉬";
            BtnBrush.UseVisualStyleBackColor = true;
            BtnBrush.Click += BtnBrush_Click;
            // 
            // TrbPenSize
            // 
            TrbPenSize.Location = new Point(126, 21);
            TrbPenSize.Margin = new Padding(2);
            TrbPenSize.Maximum = 20;
            TrbPenSize.Minimum = 5;
            TrbPenSize.Name = "TrbPenSize";
            TrbPenSize.Size = new Size(290, 45);
            TrbPenSize.TabIndex = 4;
            TrbPenSize.Value = 5;
            TrbPenSize.ValueChanged += TrbPenSize_ValueChanged;
            // 
            // TrbEraserSize
            // 
            TrbEraserSize.LargeChange = 10;
            TrbEraserSize.Location = new Point(126, 20);
            TrbEraserSize.Margin = new Padding(2);
            TrbEraserSize.Maximum = 50;
            TrbEraserSize.Minimum = 10;
            TrbEraserSize.Name = "TrbEraserSize";
            TrbEraserSize.Size = new Size(290, 45);
            TrbEraserSize.SmallChange = 5;
            TrbEraserSize.TabIndex = 4;
            TrbEraserSize.TickFrequency = 5;
            TrbEraserSize.Value = 10;
            TrbEraserSize.ValueChanged += TrbEraserSize_ValueChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(BtnNoBgSave);
            groupBox3.Controls.Add(BtnSaveImg);
            groupBox3.Controls.Add(BtnLoadImg);
            groupBox3.Location = new Point(1023, 391);
            groupBox3.Margin = new Padding(2);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(2);
            groupBox3.Size = new Size(424, 55);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "파일";
            // 
            // BtnNoBgSave
            // 
            BtnNoBgSave.Location = new Point(296, 21);
            BtnNoBgSave.Margin = new Padding(2);
            BtnNoBgSave.Name = "BtnNoBgSave";
            BtnNoBgSave.Size = new Size(114, 22);
            BtnNoBgSave.TabIndex = 0;
            BtnNoBgSave.Text = "배경없이 저장";
            BtnNoBgSave.UseVisualStyleBackColor = true;
            BtnNoBgSave.Click += BtnNoBgSave_Click;
            // 
            // BtnSaveImg
            // 
            BtnSaveImg.Location = new Point(155, 21);
            BtnSaveImg.Margin = new Padding(2);
            BtnSaveImg.Name = "BtnSaveImg";
            BtnSaveImg.Size = new Size(114, 22);
            BtnSaveImg.TabIndex = 0;
            BtnSaveImg.Text = "배경유지 저장";
            BtnSaveImg.UseVisualStyleBackColor = true;
            BtnSaveImg.Click += BtnSaveImg_Click;
            // 
            // BtnLoadImg
            // 
            BtnLoadImg.Location = new Point(14, 20);
            BtnLoadImg.Margin = new Padding(2);
            BtnLoadImg.Name = "BtnLoadImg";
            BtnLoadImg.Size = new Size(114, 22);
            BtnLoadImg.TabIndex = 0;
            BtnLoadImg.Text = "이미지 불러오기";
            BtnLoadImg.UseVisualStyleBackColor = true;
            BtnLoadImg.Click += BtnLoadImg_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(LblEraserSize);
            groupBox4.Controls.Add(TrbEraserSize);
            groupBox4.Controls.Add(BtnBgEraser);
            groupBox4.Controls.Add(BtnEraser);
            groupBox4.Controls.Add(BtnAllClear);
            groupBox4.Controls.Add(BtnAllBgClear);
            groupBox4.Controls.Add(BtnAllDrawClear);
            groupBox4.Location = new Point(1022, 182);
            groupBox4.Margin = new Padding(2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(2);
            groupBox4.Size = new Size(425, 114);
            groupBox4.TabIndex = 10;
            groupBox4.TabStop = false;
            groupBox4.Text = "지우개";
            // 
            // LblEraserSize
            // 
            LblEraserSize.AutoSize = true;
            LblEraserSize.Location = new Point(18, 29);
            LblEraserSize.Name = "LblEraserSize";
            LblEraserSize.Size = new Size(96, 15);
            LblEraserSize.TabIndex = 6;
            LblEraserSize.Text = "지우개 크기 : 10";
            LblEraserSize.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // BtnBgEraser
            // 
            BtnBgEraser.Location = new Point(92, 69);
            BtnBgEraser.Margin = new Padding(2);
            BtnBgEraser.Name = "BtnBgEraser";
            BtnBgEraser.Size = new Size(80, 22);
            BtnBgEraser.TabIndex = 2;
            BtnBgEraser.Text = "배경 지우개";
            BtnBgEraser.UseVisualStyleBackColor = true;
            BtnBgEraser.Click += BtnBgEraser_Click;
            // 
            // BtnAllClear
            // 
            BtnAllClear.Location = new Point(332, 69);
            BtnAllClear.Margin = new Padding(2);
            BtnAllClear.Name = "BtnAllClear";
            BtnAllClear.Size = new Size(80, 22);
            BtnAllClear.TabIndex = 2;
            BtnAllClear.Text = "전체 초기화";
            BtnAllClear.UseVisualStyleBackColor = true;
            BtnAllClear.Click += BtnAllClear_Click;
            // 
            // BtnAllBgClear
            // 
            BtnAllBgClear.Location = new Point(252, 69);
            BtnAllBgClear.Margin = new Padding(2);
            BtnAllBgClear.Name = "BtnAllBgClear";
            BtnAllBgClear.Size = new Size(80, 22);
            BtnAllBgClear.TabIndex = 2;
            BtnAllBgClear.Text = "배경 초기화";
            BtnAllBgClear.UseVisualStyleBackColor = true;
            BtnAllBgClear.Click += BtnAllBgClear_Click;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(BtnRectangleFull);
            groupBox6.Controls.Add(BtnCircleFull);
            groupBox6.Controls.Add(BtnTriangleFull);
            groupBox6.Location = new Point(1241, 316);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(208, 55);
            groupBox6.TabIndex = 7;
            groupBox6.TabStop = false;
            groupBox6.Text = "도형 채우기";
            // 
            // BtnRectangleFull
            // 
            BtnRectangleFull.Location = new Point(26, 22);
            BtnRectangleFull.Name = "BtnRectangleFull";
            BtnRectangleFull.Size = new Size(45, 23);
            BtnRectangleFull.TabIndex = 6;
            BtnRectangleFull.Text = "네모";
            BtnRectangleFull.UseVisualStyleBackColor = true;
            BtnRectangleFull.Click += BtnRectangleFull_Click;
            // 
            // BtnCircleFull
            // 
            BtnCircleFull.Location = new Point(138, 22);
            BtnCircleFull.Name = "BtnCircleFull";
            BtnCircleFull.Size = new Size(45, 23);
            BtnCircleFull.TabIndex = 6;
            BtnCircleFull.Text = "원";
            BtnCircleFull.UseVisualStyleBackColor = true;
            BtnCircleFull.Click += BtnCircleFull_Click;
            // 
            // BtnTriangleFull
            // 
            BtnTriangleFull.Location = new Point(82, 22);
            BtnTriangleFull.Name = "BtnTriangleFull";
            BtnTriangleFull.Size = new Size(45, 23);
            BtnTriangleFull.TabIndex = 6;
            BtnTriangleFull.Text = "세모";
            BtnTriangleFull.UseVisualStyleBackColor = true;
            BtnTriangleFull.Click += BtnTriangleFull_Click;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(BtnUndo);
            groupBox7.Controls.Add(BtnRedo);
            groupBox7.Location = new Point(1241, 466);
            groupBox7.Margin = new Padding(2);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new Padding(2);
            groupBox7.Size = new Size(208, 55);
            groupBox7.TabIndex = 9;
            groupBox7.TabStop = false;
            groupBox7.Text = "Undo/Redo";
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(BtnBackgroundColor);
            groupBox8.Location = new Point(1023, 466);
            groupBox8.Margin = new Padding(2);
            groupBox8.Name = "groupBox8";
            groupBox8.Padding = new Padding(2);
            groupBox8.Size = new Size(208, 55);
            groupBox8.TabIndex = 9;
            groupBox8.TabStop = false;
            groupBox8.Text = "기타";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1460, 955);
            Controls.Add(groupBox4);
            Controls.Add(groupBox7);
            Controls.Add(groupBox8);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox6);
            Controls.Add(groupBox1);
            Controls.Add(WhiteBoard);
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
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
            groupBox6.ResumeLayout(false);
            groupBox7.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox WhiteBoard;
        private ColorDialog DlgColor;
        private Button BtnPenColor;
        private Button BtnEraser;
        private Button BtnAllDrawClear;
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
        private Button BtnSpray;
        private GroupBox groupBox6;
        private Button BtnRectangleFull;
        private Button BtnCircleFull;
        private Button BtnTriangleFull;
        private GroupBox groupBox7;
        private Label LblPenSize;
        private Label LblEraserSize;
        private GroupBox groupBox8;
        private Button BtnNoBgSave;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button BtnBgEraser;
        private Button BtnAllBgClear;
        private Button BtnAllClear;
    }
}
