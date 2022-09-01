namespace SmallCoder
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbEntity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Refresh = new System.Windows.Forms.Button();
            this.txtEntityCustom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Generate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSpaceName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDbCon = new System.Windows.Forms.ComboBox();
            this.btnConf = new System.Windows.Forms.Button();
            this.cbTemplate = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_AppendSpace = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.lbl_CS = new System.Windows.Forms.Label();
            this.rtbJson = new System.Windows.Forms.RichTextBox();
            this.lblJson = new System.Windows.Forms.Label();
            this.btnJsonCheck = new System.Windows.Forms.Button();
            this.lbl_Editor = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbDb = new System.Windows.Forms.ComboBox();
            this.btnJsonSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtFilter);
            this.groupBox1.Location = new System.Drawing.Point(12, 257);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 62);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "过滤模板";
            // 
            // txtFilter
            // 
            this.txtFilter.Font = new System.Drawing.Font("宋体", 10F);
            this.txtFilter.Location = new System.Drawing.Point(68, 22);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(340, 23);
            this.txtFilter.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "数据连接";
            // 
            // cbEntity
            // 
            this.cbEntity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEntity.Font = new System.Drawing.Font("宋体", 11F);
            this.cbEntity.FormattingEnabled = true;
            this.cbEntity.Location = new System.Drawing.Point(81, 100);
            this.cbEntity.Name = "cbEntity";
            this.cbEntity.Size = new System.Drawing.Size(265, 23);
            this.cbEntity.TabIndex = 5;
            this.cbEntity.TabStop = false;
            this.cbEntity.SelectedValueChanged += new System.EventHandler(this.cbEntity_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "数据表";
            // 
            // lbl_Refresh
            // 
            this.lbl_Refresh.Location = new System.Drawing.Point(362, 100);
            this.lbl_Refresh.Name = "lbl_Refresh";
            this.lbl_Refresh.Size = new System.Drawing.Size(56, 24);
            this.lbl_Refresh.TabIndex = 7;
            this.lbl_Refresh.TabStop = false;
            this.lbl_Refresh.Text = "刷新";
            this.lbl_Refresh.UseVisualStyleBackColor = true;
            this.lbl_Refresh.Click += new System.EventHandler(this.lbl_Refresh_Click);
            // 
            // txtEntityCustom
            // 
            this.txtEntityCustom.Font = new System.Drawing.Font("宋体", 11F);
            this.txtEntityCustom.Location = new System.Drawing.Point(81, 143);
            this.txtEntityCustom.Name = "txtEntityCustom";
            this.txtEntityCustom.Size = new System.Drawing.Size(265, 24);
            this.txtEntityCustom.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "实体名称";
            // 
            // btn_Generate
            // 
            this.btn_Generate.Font = new System.Drawing.Font("宋体", 16F);
            this.btn_Generate.Location = new System.Drawing.Point(199, 11);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(89, 56);
            this.btn_Generate.TabIndex = 2;
            this.btn_Generate.Text = "生成";
            this.btn_Generate.UseVisualStyleBackColor = true;
            this.btn_Generate.Click += new System.EventHandler(this.btn_Generate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "命名空间";
            // 
            // txtSpaceName
            // 
            this.txtSpaceName.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSpaceName.Location = new System.Drawing.Point(81, 180);
            this.txtSpaceName.Name = "txtSpaceName";
            this.txtSpaceName.Size = new System.Drawing.Size(265, 24);
            this.txtSpaceName.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label5.Location = new System.Drawing.Point(400, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "manong";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // cbDbCon
            // 
            this.cbDbCon.DisplayMember = "name";
            this.cbDbCon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDbCon.Font = new System.Drawing.Font("宋体", 11F);
            this.cbDbCon.FormattingEnabled = true;
            this.cbDbCon.Location = new System.Drawing.Point(81, 22);
            this.cbDbCon.Name = "cbDbCon";
            this.cbDbCon.Size = new System.Drawing.Size(265, 23);
            this.cbDbCon.TabIndex = 15;
            this.cbDbCon.TabStop = false;
            this.cbDbCon.ValueMember = "conn";
            this.cbDbCon.SelectedIndexChanged += new System.EventHandler(this.cbDbCon_SelectedIndexChanged);
            // 
            // btnConf
            // 
            this.btnConf.Location = new System.Drawing.Point(362, 22);
            this.btnConf.Name = "btnConf";
            this.btnConf.Size = new System.Drawing.Size(56, 24);
            this.btnConf.TabIndex = 14;
            this.btnConf.TabStop = false;
            this.btnConf.Text = "配置";
            this.btnConf.UseVisualStyleBackColor = true;
            this.btnConf.Click += new System.EventHandler(this.btnConf_Click);
            // 
            // cbTemplate
            // 
            this.cbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTemplate.Font = new System.Drawing.Font("宋体", 11F);
            this.cbTemplate.FormattingEnabled = true;
            this.cbTemplate.Location = new System.Drawing.Point(10, 25);
            this.cbTemplate.Name = "cbTemplate";
            this.cbTemplate.Size = new System.Drawing.Size(147, 23);
            this.cbTemplate.TabIndex = 16;
            this.cbTemplate.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_AppendSpace);
            this.groupBox2.Controls.Add(this.cbTemplate);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.groupBox2.Location = new System.Drawing.Point(14, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(167, 60);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模板";
            // 
            // checkBox_AppendSpace
            // 
            this.checkBox_AppendSpace.AutoSize = true;
            this.checkBox_AppendSpace.Location = new System.Drawing.Point(37, 0);
            this.checkBox_AppendSpace.Name = "checkBox_AppendSpace";
            this.checkBox_AppendSpace.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox_AppendSpace.Size = new System.Drawing.Size(120, 16);
            this.checkBox_AppendSpace.TabIndex = 27;
            this.checkBox_AppendSpace.TabStop = false;
            this.checkBox_AppendSpace.Text = "目录追加命名空间";
            this.checkBox_AppendSpace.UseVisualStyleBackColor = true;
            this.checkBox_AppendSpace.CheckedChanged += new System.EventHandler(this.checkBox_AppendSpace_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "功能描述";
            // 
            // txtDesc
            // 
            this.txtDesc.Font = new System.Drawing.Font("宋体", 11F);
            this.txtDesc.Location = new System.Drawing.Point(81, 219);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(265, 24);
            this.txtDesc.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btn_Generate);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 333);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(444, 73);
            this.panel1.TabIndex = 2;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("宋体", 16F);
            this.btnClear.Location = new System.Drawing.Point(303, 11);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(83, 56);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "清理";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lbl_CS
            // 
            this.lbl_CS.AutoSize = true;
            this.lbl_CS.Font = new System.Drawing.Font("Gabriola", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CS.ForeColor = System.Drawing.Color.Black;
            this.lbl_CS.Location = new System.Drawing.Point(363, 143);
            this.lbl_CS.Name = "lbl_CS";
            this.lbl_CS.Size = new System.Drawing.Size(55, 45);
            this.lbl_CS.TabIndex = 22;
            this.lbl_CS.Text = "Json";
            this.lbl_CS.Click += new System.EventHandler(this.lbl_CS_Click);
            this.lbl_CS.DoubleClick += new System.EventHandler(this.lbl_CS_Click);
            // 
            // rtbJson
            // 
            this.rtbJson.AcceptsTab = true;
            this.rtbJson.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbJson.DetectUrls = false;
            this.rtbJson.Font = new System.Drawing.Font("宋体", 10.5F);
            this.rtbJson.Location = new System.Drawing.Point(456, 41);
            this.rtbJson.Name = "rtbJson";
            this.rtbJson.Size = new System.Drawing.Size(296, 359);
            this.rtbJson.TabIndex = 4;
            this.rtbJson.TabStop = false;
            this.rtbJson.Text = "";
            this.rtbJson.WordWrap = false;
            this.rtbJson.TextChanged += new System.EventHandler(this.rtbJson_TextChanged);
            // 
            // lblJson
            // 
            this.lblJson.AutoSize = true;
            this.lblJson.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lblJson.Location = new System.Drawing.Point(455, 23);
            this.lblJson.Name = "lblJson";
            this.lblJson.Size = new System.Drawing.Size(70, 12);
            this.lblJson.TabIndex = 24;
            this.lblJson.Text = "自定义参数";
            // 
            // btnJsonCheck
            // 
            this.btnJsonCheck.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnJsonCheck.Location = new System.Drawing.Point(651, 14);
            this.btnJsonCheck.Name = "btnJsonCheck";
            this.btnJsonCheck.Size = new System.Drawing.Size(47, 23);
            this.btnJsonCheck.TabIndex = 25;
            this.btnJsonCheck.TabStop = false;
            this.btnJsonCheck.Text = "校验";
            this.btnJsonCheck.UseVisualStyleBackColor = true;
            this.btnJsonCheck.Click += new System.EventHandler(this.btnJsonCheck_Click);
            // 
            // lbl_Editor
            // 
            this.lbl_Editor.AutoSize = true;
            this.lbl_Editor.Font = new System.Drawing.Font("Gabriola", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Editor.ForeColor = System.Drawing.Color.Black;
            this.lbl_Editor.Location = new System.Drawing.Point(361, 201);
            this.lbl_Editor.Name = "lbl_Editor";
            this.lbl_Editor.Size = new System.Drawing.Size(55, 45);
            this.lbl_Editor.TabIndex = 26;
            this.lbl_Editor.Text = "Edit";
            this.lbl_Editor.Click += new System.EventHandler(this.lbl_Editor_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 28;
            this.label8.Text = "数据库";
            // 
            // cbDb
            // 
            this.cbDb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDb.Font = new System.Drawing.Font("宋体", 11F);
            this.cbDb.FormattingEnabled = true;
            this.cbDb.Location = new System.Drawing.Point(81, 61);
            this.cbDb.Name = "cbDb";
            this.cbDb.Size = new System.Drawing.Size(265, 23);
            this.cbDb.TabIndex = 27;
            this.cbDb.TabStop = false;
            // 
            // btnJsonSave
            // 
            this.btnJsonSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnJsonSave.Location = new System.Drawing.Point(705, 14);
            this.btnJsonSave.Name = "btnJsonSave";
            this.btnJsonSave.Size = new System.Drawing.Size(47, 23);
            this.btnJsonSave.TabIndex = 29;
            this.btnJsonSave.TabStop = false;
            this.btnJsonSave.Text = "保存";
            this.btnJsonSave.UseVisualStyleBackColor = true;
            this.btnJsonSave.Click += new System.EventHandler(this.btnJsonSave_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(444, 406);
            this.Controls.Add(this.btnJsonSave);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbDb);
            this.Controls.Add(this.lbl_Editor);
            this.Controls.Add(this.btnJsonCheck);
            this.Controls.Add(this.lblJson);
            this.Controls.Add(this.rtbJson);
            this.Controls.Add(this.lbl_CS);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.cbDbCon);
            this.Controls.Add(this.btnConf);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSpaceName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEntityCustom);
            this.Controls.Add(this.lbl_Refresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbEntity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmallCoder";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button lbl_Refresh;
        private System.Windows.Forms.TextBox txtEntityCustom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Generate;
        private System.Windows.Forms.ComboBox cbEntity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSpaceName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbDbCon;
        private System.Windows.Forms.Button btnConf;
        private System.Windows.Forms.ComboBox cbTemplate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_CS;
        private System.Windows.Forms.RichTextBox rtbJson;
        private System.Windows.Forms.Label lblJson;
        private System.Windows.Forms.Button btnJsonCheck;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lbl_Editor;
        private System.Windows.Forms.CheckBox checkBox_AppendSpace;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbDb;
        private System.Windows.Forms.Button btnJsonSave;
    }
}