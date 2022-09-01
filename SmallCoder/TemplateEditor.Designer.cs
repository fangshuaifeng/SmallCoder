namespace SmallCoder
{
    partial class TemplateEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView_Files = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_RightMouse = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重命名F2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开所在目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增 = new System.Windows.Forms.ToolStripMenuItem();
            this.新增目录 = new System.Windows.Forms.ToolStripMenuItem();
            this.复制 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.contextMenuStrip_Editor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.保存CtrlSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findReplaceControl1 = new SmallCoder.FindReplaceControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip_RightMouse.SuspendLayout();
            this.contextMenuStrip_Editor.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView_Files);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.findReplaceControl1);
            this.splitContainer1.Panel2.Controls.Add(this.elementHost1);
            this.splitContainer1.Size = new System.Drawing.Size(1167, 530);
            this.splitContainer1.SplitterDistance = 387;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // treeView_Files
            // 
            this.treeView_Files.AllowDrop = true;
            this.treeView_Files.ContextMenuStrip = this.contextMenuStrip_RightMouse;
            this.treeView_Files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Files.Font = new System.Drawing.Font("宋体", 10F);
            this.treeView_Files.Location = new System.Drawing.Point(0, 0);
            this.treeView_Files.Name = "treeView_Files";
            this.treeView_Files.Size = new System.Drawing.Size(387, 530);
            this.treeView_Files.TabIndex = 0;
            this.treeView_Files.TabStop = false;
            this.treeView_Files.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_Files_AfterLabelEdit);
            this.treeView_Files.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Files_AfterCollapse);
            this.treeView_Files.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Files_AfterExpand);
            this.treeView_Files.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_Files_ItemDrag);
            this.treeView_Files.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_Files_DragDrop);
            this.treeView_Files.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_Files_DragEnter);
            this.treeView_Files.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_Files_DragOver);
            this.treeView_Files.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView_Files_MouseDoubleClick);
            this.treeView_Files.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_Files_MouseDown);
            // 
            // contextMenuStrip_RightMouse
            // 
            this.contextMenuStrip_RightMouse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem,
            this.重命名F2ToolStripMenuItem,
            this.打开所在目录ToolStripMenuItem,
            this.新增,
            this.新增目录,
            this.复制,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip_RightMouse.Name = "contextMenuStrip_RightMouse";
            this.contextMenuStrip_RightMouse.Size = new System.Drawing.Size(206, 158);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.刷新ToolStripMenuItem.Text = "刷新 (F5)";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 重命名F2ToolStripMenuItem
            // 
            this.重命名F2ToolStripMenuItem.Name = "重命名F2ToolStripMenuItem";
            this.重命名F2ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.重命名F2ToolStripMenuItem.Text = "重命名 (F2)";
            this.重命名F2ToolStripMenuItem.Click += new System.EventHandler(this.重命名F2ToolStripMenuItem_Click);
            // 
            // 打开所在目录ToolStripMenuItem
            // 
            this.打开所在目录ToolStripMenuItem.Image = global::SmallCoder.Properties.Resources.folder_16;
            this.打开所在目录ToolStripMenuItem.Name = "打开所在目录ToolStripMenuItem";
            this.打开所在目录ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.打开所在目录ToolStripMenuItem.Text = "打开目录(Ctrl+E)";
            this.打开所在目录ToolStripMenuItem.Click += new System.EventHandler(this.打开所在目录ToolStripMenuItem_Click);
            // 
            // 新增
            // 
            this.新增.Name = "新增";
            this.新增.Size = new System.Drawing.Size(205, 22);
            this.新增.Text = "新增文件(Ctrl+N)";
            this.新增.Click += new System.EventHandler(this.文件ToolStripMenuItem_Click);
            // 
            // 新增目录
            // 
            this.新增目录.Name = "新增目录";
            this.新增目录.Size = new System.Drawing.Size(205, 22);
            this.新增目录.Text = "新增目录(Ctrl+Shift+N)";
            this.新增目录.Click += new System.EventHandler(this.文件夹ToolStripMenuItem_Click);
            // 
            // 复制
            // 
            this.复制.Image = global::SmallCoder.Properties.Resources.copy;
            this.复制.Name = "复制";
            this.复制.Size = new System.Drawing.Size(205, 22);
            this.复制.Text = "复制粘贴(Ctrl+Shift+C)";
            this.复制.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Image = global::SmallCoder.Properties.Resources.del_16;
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.删除ToolStripMenuItem.Text = "删除 (Delete)";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // elementHost1
            // 
            this.elementHost1.AutoSize = true;
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(776, 530);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // contextMenuStrip_Editor
            // 
            this.contextMenuStrip_Editor.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip_Editor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存CtrlSToolStripMenuItem,
            this.关闭ToolStripMenuItem});
            this.contextMenuStrip_Editor.Name = "contextMenuStrip_Editor";
            this.contextMenuStrip_Editor.Size = new System.Drawing.Size(149, 48);
            // 
            // 保存CtrlSToolStripMenuItem
            // 
            this.保存CtrlSToolStripMenuItem.Name = "保存CtrlSToolStripMenuItem";
            this.保存CtrlSToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.保存CtrlSToolStripMenuItem.Text = "保存 (Ctrl+S)";
            this.保存CtrlSToolStripMenuItem.Click += new System.EventHandler(this.保存CtrlSToolStripMenuItem_Click);
            // 
            // 关闭ToolStripMenuItem
            // 
            this.关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem";
            this.关闭ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.关闭ToolStripMenuItem.Text = "关闭 (ESC)";
            this.关闭ToolStripMenuItem.Click += new System.EventHandler(this.关闭ToolStripMenuItem_Click);
            // 
            // findReplaceControl1
            // 
            this.findReplaceControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.findReplaceControl1.Location = new System.Drawing.Point(143, 180);
            this.findReplaceControl1.Name = "findReplaceControl1";
            this.findReplaceControl1.Size = new System.Drawing.Size(326, 220);
            this.findReplaceControl1.TabIndex = 1;
            this.findReplaceControl1.Visible = false;
            // 
            // TemplateEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1167, 530);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TemplateEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SmallCoder - 模板编辑器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TemplateEditor_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TemplateEditor_FormClosed);
            this.Load += new System.EventHandler(this.TemplateEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip_RightMouse.ResumeLayout(false);
            this.contextMenuStrip_Editor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView_Files;
        private System.Windows.Forms.ToolStripMenuItem 新增;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_RightMouse;
        private System.Windows.Forms.ToolStripMenuItem 新增目录;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.ToolStripMenuItem 重命名F2ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Editor;
        private System.Windows.Forms.ToolStripMenuItem 关闭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开所在目录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存CtrlSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制;
        private FindReplaceControl findReplaceControl1;
    }
}