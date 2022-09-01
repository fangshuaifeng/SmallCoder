namespace SmallCoder
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_doc = new System.Windows.Forms.LinkLabel();
            this.lbl_liquid = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::SmallCoder.Properties.Resources.dashang;
            this.pictureBox1.Location = new System.Drawing.Point(-1, -6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(276, 314);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("华文行楷", 30F);
            this.label1.Location = new System.Drawing.Point(278, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "简单";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("华文行楷", 30F);
            this.label2.Location = new System.Drawing.Point(278, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 46);
            this.label2.TabIndex = 2;
            this.label2.Text = "易用";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("华文行楷", 30F);
            this.label3.Location = new System.Drawing.Point(278, 138);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 46);
            this.label3.TabIndex = 3;
            this.label3.Text = "轻快";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_doc
            // 
            this.lbl_doc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_doc.AutoSize = true;
            this.lbl_doc.Font = new System.Drawing.Font("宋体", 11F);
            this.lbl_doc.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lbl_doc.Location = new System.Drawing.Point(346, 262);
            this.lbl_doc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_doc.Name = "lbl_doc";
            this.lbl_doc.Size = new System.Drawing.Size(67, 15);
            this.lbl_doc.TabIndex = 4;
            this.lbl_doc.TabStop = true;
            this.lbl_doc.Text = "在线文档";
            this.lbl_doc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbl_doc_LinkClicked);
            // 
            // lbl_liquid
            // 
            this.lbl_liquid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_liquid.AutoSize = true;
            this.lbl_liquid.Font = new System.Drawing.Font("宋体", 11F);
            this.lbl_liquid.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lbl_liquid.Location = new System.Drawing.Point(300, 284);
            this.lbl_liquid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_liquid.Name = "lbl_liquid";
            this.lbl_liquid.Size = new System.Drawing.Size(115, 15);
            this.lbl_liquid.TabIndex = 5;
            this.lbl_liquid.TabStop = true;
            this.lbl_liquid.Text = "Liquid中文手册";
            this.lbl_liquid.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbl_liquid_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(416, 305);
            this.Controls.Add(this.lbl_liquid);
            this.Controls.Add(this.lbl_doc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "关于";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lbl_doc;
        private System.Windows.Forms.LinkLabel lbl_liquid;
    }
}