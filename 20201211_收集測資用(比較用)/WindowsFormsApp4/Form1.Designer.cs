namespace WindowsFormsApp4
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.BASE_bar = new System.Windows.Forms.ProgressBar();
            this.data_lbox = new System.Windows.Forms.ListBox();
            this.MAX_Diff_tb = new System.Windows.Forms.TextBox();
            this.MAX_ADiff_tb = new System.Windows.Forms.TextBox();
            this.MIN_ADiff_tb = new System.Windows.Forms.TextBox();
            this.MIN_Diff_tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Result_lb = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(892, 539);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(42, 29);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(819, 440);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            // 
            // BASE_bar
            // 
            this.BASE_bar.Location = new System.Drawing.Point(1000, 29);
            this.BASE_bar.Name = "BASE_bar";
            this.BASE_bar.Size = new System.Drawing.Size(226, 28);
            this.BASE_bar.Step = 1;
            this.BASE_bar.TabIndex = 2;
            // 
            // data_lbox
            // 
            this.data_lbox.FormattingEnabled = true;
            this.data_lbox.ItemHeight = 12;
            this.data_lbox.Location = new System.Drawing.Point(990, 80);
            this.data_lbox.Name = "data_lbox";
            this.data_lbox.Size = new System.Drawing.Size(218, 460);
            this.data_lbox.TabIndex = 3;
            // 
            // MAX_Diff_tb
            // 
            this.MAX_Diff_tb.Location = new System.Drawing.Point(990, 554);
            this.MAX_Diff_tb.Name = "MAX_Diff_tb";
            this.MAX_Diff_tb.Size = new System.Drawing.Size(52, 22);
            this.MAX_Diff_tb.TabIndex = 4;
            this.MAX_Diff_tb.TextChanged += new System.EventHandler(this.MAX_Diff_tb_TextChanged);
            // 
            // MAX_ADiff_tb
            // 
            this.MAX_ADiff_tb.Location = new System.Drawing.Point(1060, 554);
            this.MAX_ADiff_tb.Name = "MAX_ADiff_tb";
            this.MAX_ADiff_tb.Size = new System.Drawing.Size(52, 22);
            this.MAX_ADiff_tb.TabIndex = 5;
            this.MAX_ADiff_tb.TextChanged += new System.EventHandler(this.MAX_ADiff_tb_TextChanged);
            // 
            // MIN_ADiff_tb
            // 
            this.MIN_ADiff_tb.Location = new System.Drawing.Point(1118, 554);
            this.MIN_ADiff_tb.Name = "MIN_ADiff_tb";
            this.MIN_ADiff_tb.Size = new System.Drawing.Size(52, 22);
            this.MIN_ADiff_tb.TabIndex = 6;
            this.MIN_ADiff_tb.TextChanged += new System.EventHandler(this.MIN_ADiff_tb_TextChanged);
            // 
            // MIN_Diff_tb
            // 
            this.MIN_Diff_tb.Location = new System.Drawing.Point(958, 447);
            this.MIN_Diff_tb.Name = "MIN_Diff_tb";
            this.MIN_Diff_tb.Size = new System.Drawing.Size(52, 22);
            this.MIN_Diff_tb.TabIndex = 7;
            this.MIN_Diff_tb.TextChanged += new System.EventHandler(this.MIN_Diff_tb_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(929, 525);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "MAX Diff";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(998, 525);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "MIN Dff";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1067, 525);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "MAX ADiff";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1134, 525);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "MIN ADiff";
            // 
            // Result_lb
            // 
            this.Result_lb.AutoSize = true;
            this.Result_lb.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Result_lb.Location = new System.Drawing.Point(968, 248);
            this.Result_lb.Name = "Result_lb";
            this.Result_lb.Size = new System.Drawing.Size(82, 16);
            this.Result_lb.TabIndex = 12;
            this.Result_lb.Text = "Create Base";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 588);
            this.Controls.Add(this.Result_lb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MIN_Diff_tb);
            this.Controls.Add(this.MIN_ADiff_tb);
            this.Controls.Add(this.MAX_ADiff_tb);
            this.Controls.Add(this.MAX_Diff_tb);
            this.Controls.Add(this.data_lbox);
            this.Controls.Add(this.BASE_bar);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "AI Module";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ProgressBar BASE_bar;
        private System.Windows.Forms.ListBox data_lbox;
        private System.Windows.Forms.TextBox MAX_Diff_tb;
        private System.Windows.Forms.TextBox MAX_ADiff_tb;
        private System.Windows.Forms.TextBox MIN_ADiff_tb;
        private System.Windows.Forms.TextBox MIN_Diff_tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Result_lb;
    }
}

