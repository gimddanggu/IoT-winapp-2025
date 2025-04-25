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
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(WhiteBoard);
            Name = "FrmMain";
            Text = "그림판";
            Load += FrmMain_Load;
            ((System.ComponentModel.ISupportInitialize)WhiteBoard).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox WhiteBoard;
    }
}
