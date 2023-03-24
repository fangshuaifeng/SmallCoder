using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace SmallCoder
{
    public partial class TemplateEditor : Form
    {
        string repeatAppend = "(1)";
        ResourceManager _resman;
        ICSharpCode.AvalonEdit.TextEditor _editor;
        string _editingFilePath;
        string _editOldText;
        TreeNode _editNode;
        private AppConfig _appConfig;
        private bool isInitting = true;
        private bool isBindingTreeview = true;
        /// <summary>
        /// 自定义高亮语法
        /// </summary>
        private List<HighlightingRule> _customerHighlightingRules = new List<HighlightingRule>
        {
            new HighlightingRule()
                {
                    Regex = new System.Text.RegularExpressions.Regex(@"(?<=\{\{)\s*\S+\s*(?=\}\})"),
                    Color = new HighlightingColor
                    {
                        Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(227, 0, 140))//180, 0, 158
                    }
                }
        };

        #region 初始化

        public TemplateEditor()
        {
            _resman = new ResourceManager($"{Program._AppName}.Properties.Resources", Program._Assembly);
            InitializeComponent();
            this.initTextEditor();
            this.loadAndRestoreFormSet();
            this.isInitting = false;
            this.Resize += new EventHandler(this.TemplateEditor_Resize);
            this.BindShortcuts();
        }

        /// <summary>
        /// 绑定快捷键
        /// </summary>
        private void BindShortcuts()
        {
            this.KeyPreview = true;

            // 整个窗体
            HotKeyManager.AddFormControlHotKey(this, findReplaceControl1.CloseFindDialog, Keys.Escape);

            // --------------------左侧文件树---------------
            treeView_Files.KeyPress += (o, e) => { e.Handled = true; };// 清除提示音
            // 回车打开文件进行编辑
            HotKeyManager.AddFormControlHotKey(treeView_Files, openSelectedFileOnEditor, Keys.Enter);
            // 新增文件
            HotKeyManager.AddFormControlHotKey(treeView_Files, () => this.InsertFileOrFolder(false), Keys.N, true);
            // 新增文件（夹）
            HotKeyManager.AddFormControlHotKey(treeView_Files, () => this.InsertFileOrFolder(true), Keys.N, true, true);
            // 重命名
            HotKeyManager.AddFormControlHotKey(treeView_Files, triggerReName, Keys.F2);
            // 刷新
            HotKeyManager.AddFormControlHotKey(treeView_Files, flushTreeViewBindFiles, Keys.F5);
            HotKeyManager.AddFormControlHotKey(treeView_Files, flushTreeViewBindFiles, Keys.R, true);
            // 删除
            HotKeyManager.AddFormControlHotKey(treeView_Files, RemoveFileOrFolder, Keys.Delete);
            HotKeyManager.AddFormControlHotKey(treeView_Files, RemoveFileOrFolder, Keys.D, true);
            // 关闭右侧编辑器
            HotKeyManager.AddFormControlHotKey(treeView_Files, closeEditingFile, Keys.Escape);
            // 打开文件（夹）
            HotKeyManager.AddFormControlHotKey(treeView_Files, OpenFolder, Keys.E, true);
            // 复制文件（夹）
            HotKeyManager.AddFormControlHotKey(treeView_Files, CopyFileOrFolder, Keys.C, true, true);
            // 复制 + 粘贴文件（夹）
            HotKeyManager.AddFormControlHotKey(treeView_Files, CopyPasteFileOrFolder_Start, Keys.C, true);
            HotKeyManager.AddFormControlHotKey(treeView_Files, CopyPastFileOrFolder_End, Keys.V, true);


            // --------------------右侧编辑器---------------
            HotKeyManager.AddControlHotKey(_editor, saveEditingFile, System.Windows.Input.Key.S, true);
            HotKeyManager.AddControlHotKey(_editor, closeEditingFile, System.Windows.Input.Key.Escape);
            HotKeyManager.AddControlHotKey(_editor, openFindReplaceForm, System.Windows.Input.Key.F, true);
            HotKeyManager.AddControlHotKey(_editor.TextArea, () => moveEditorDocumentRow(true), System.Windows.Input.Key.Up, false, false, true);
            HotKeyManager.AddControlHotKey(_editor.TextArea, () => moveEditorDocumentRow(false), System.Windows.Input.Key.Down, false, false, true);
        }

        #endregion

        #region 初始化窗体信息

        private void loadAndRestoreFormSet()
        {
            _appConfig = Utils.LoadConfFile();
            var formSetting = _appConfig.formSetting.templateForm;
            if (formSetting != null)
            {
                if (formSetting.formSize != null)
                {
                    this.Width = formSetting.formSize.Width;
                    this.Height = formSetting.formSize.Height;
                    this.WindowState = (FormWindowState)formSetting.formSize.FormWindowState;
                }
                if (formSetting.splitterDistance.HasValue && this.splitContainer1.SplitterDistance != formSetting.splitterDistance)
                {
                    this.splitContainer1.SplitterDistance = formSetting.splitterDistance.Value;
                }
            }
        }

        private void TemplateEditor_Load(object sender, EventArgs e)
        {
            ImageList imageList = new ImageList();
            imageList.Images.Add((Image)_resman.GetObject("folder"));
            imageList.Images.Add((Image)_resman.GetObject("file"));
            imageList.Images.Add((Image)_resman.GetObject("file2"));
            imageList.Images.Add((Image)_resman.GetObject("edit"));
            this.treeView_Files.ImageList = imageList;
            this.flushTreeViewBindFiles();
        }

        /// <summary>
        /// 初始化编辑器
        /// </summary>
        private void initTextEditor()
        {
            _editor = new ICSharpCode.AvalonEdit.TextEditor();
            _editor.TextChanged += (sender, e) => this.switchEditStatus();
            this.elementHost1.Child = _editor;

            _editor.Padding = new System.Windows.Thickness(4);
            _editor.ShowLineNumbers = true;
            _editor.Options.WordWrapIndentation = 4;
            _editor.Options.InheritWordWrapIndentation = true;
            _editor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            _editor.BorderThickness = new System.Windows.Thickness(1, 3, 0, 0);
            this.switchEditorEditable(false);

            this.findReplaceControl1.BindEditor(_editor);
            _editor.TextChanged += (sender, e) => { if (findReplaceControl1.Visible) this.findReplaceControl1.FindAll(); };
        }

        private void openFindReplaceForm()
        {
            if (string.IsNullOrEmpty(_editor.SelectedText)) return;

            this.findReplaceControl1.SendToFindText(_editor.SelectedText);
            this.findReplaceControl1.OpenFindDialog(/*this.elementHost1.Width - this.findReplaceControl1.Width - 40*/);
        }

        #endregion

        #region 刷新左侧文件树

        private void flushTreeViewBindFiles()
        {
            this.isBindingTreeview = true;
            this.PaintTreeView(treeView_Files.Nodes, Utils._tempPath, (treeNodeCollection) =>
            {
                if (_editNode != null)
                {
                    var oldSelNodes = this.treeView_Files.Nodes.Find(_editNode.Name, true);
                    if (oldSelNodes != null && oldSelNodes.Length == 1)
                    {
                        _editNode = oldSelNodes[0];
                        _editNode.ImageIndex = _editNode.SelectedImageIndex = 2;
                    }
                    else
                    {
                        _editNode = null;
                    }
                }
                this.isBindingTreeview = false;
            });
        }

        private void PaintTreeView(TreeNodeCollection treeNodeCollection, string fullPath, Action<TreeNodeCollection> successAct = null)
        {
            treeNodeCollection.Clear();

            DirectoryInfo dirs = new DirectoryInfo(fullPath); //获得程序所在路径的目录对象
            DirectoryInfo[] dir = dirs.GetDirectories();//获得目录下文件夹对象
            FileInfo[] file = dirs.GetFiles();//获得目录下文件对象
            int dircount = dir.Count();//获得文件夹对象数量
            int filecount = file.Count();//获得文件对象数量

            //循环文件夹
            for (int i = 0; i < dircount; i++)
            {
                var tn = treeNodeCollection.Add(dir[i].FullName, dir[i].Name);
                tn.ImageIndex = tn.SelectedImageIndex = 0;
                string pathNode = fullPath + "\\" + dir[i].Name;
                GetMultiNode(treeNodeCollection[i], pathNode);
            }

            //循环文件
            for (int j = 0; j < filecount; j++)
            {
                var tn = treeNodeCollection.Add(file[j].FullName, file[j].Name);
                tn.ImageIndex = tn.SelectedImageIndex = 1;
            }

            #region 还原展开折叠状态

            // 在绑定节点过程中，处理展开折叠不生效，需要在节点绑定完成后做处理
            var collapseNodes = this._appConfig.formSetting.templateForm.collapseNodes;

            void recursiveCollapse(TreeNodeCollection parentTreeNodeCollection)
            {
                foreach (TreeNode item in parentTreeNodeCollection)
                {
                    if (collapseNodes.Contains(item.FullPath))
                    {
                        item.Collapse();
                    }
                    else
                    {
                        item.Expand();
                    }

                    if (item.Nodes.Count > 0)
                    {
                        recursiveCollapse(item.Nodes);
                    }
                }
            }

            recursiveCollapse(treeNodeCollection);

            #endregion

            successAct?.Invoke(treeNodeCollection);
        }

        private bool GetMultiNode(TreeNode treeNode, string path)
        {
            if (Directory.Exists(path) == false)
            { return false; }

            DirectoryInfo dirs = new DirectoryInfo(path); //获得程序所在路径的目录对象
            DirectoryInfo[] dir = dirs.GetDirectories();//获得目录下文件夹对象
            FileInfo[] file = dirs.GetFiles();//获得目录下文件对象
            int dircount = dir.Count();//获得文件夹对象数量
            int filecount = file.Count();//获得文件对象数量
            int sumcount = dircount + filecount;

            if (sumcount == 0)
            { return false; }

            //循环文件夹
            for (int j = 0; j < dircount; j++)
            {
                var tn = treeNode.Nodes.Add(dir[j].FullName, dir[j].Name);
                tn.ImageIndex = tn.SelectedImageIndex = 0;
                string pathNodeB = path + "\\" + dir[j].Name;
                GetMultiNode(treeNode.Nodes[j], pathNodeB);
            }

            //循环文件
            for (int j = 0; j < filecount; j++)
            {
                var tn = treeNode.Nodes.Add(file[j].FullName, file[j].Name);
                tn.ImageIndex = tn.SelectedImageIndex = 1;
            }
            return true;
        }

        #endregion

        #region 窗体变化相关

        private void TemplateEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.elementHost1 != null)
            {
                _editor = null;
                elementHost1.Controls.Clear();
                elementHost1.Dispose();
                elementHost1 = null;
            }
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
            Utils.FlushMemory();
        }

        private void TemplateEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 窗体关闭前，记录一下，窗体宽高
            var formSize = this._appConfig.formSetting.templateForm.formSize;
            if (formSize == null)
            {
                this._appConfig.formSetting.templateForm.formSize = new FormSize(this.Width, this.Height, (int)this.WindowState);
            }
            else if ((int)this.WindowState != formSize.FormWindowState)
            {
                formSize.FormWindowState = (int)this.WindowState;
            }

            #region 更新记录展开折叠状态

            // 左侧树变化会很多，可能会造成原记录折叠信息失效，此处进行刷新处理
            var collapseNodes = this._appConfig.formSetting.templateForm.collapseNodes;
            collapseNodes.Clear();

            void recursiveCollapse(TreeNodeCollection treeNodeCollection)
            {
                foreach (TreeNode item in treeNodeCollection)
                {
                    if (!item.IsExpanded && item.Nodes.Count > 0)//判定文件夹
                    {
                        collapseNodes.Add(item.FullPath);
                    }

                    if (item.Nodes.Count > 0)
                    {
                        recursiveCollapse(item.Nodes);
                    }
                }
            }

            recursiveCollapse(this.treeView_Files.Nodes);

            #endregion

            Utils.writeToConfFile(this._appConfig);
            fileDocumentChangeCheck();
        }

        private void TemplateEditor_Resize(object sender, EventArgs e)
        {
            if (this.isInitting) return;

            // 窗体关闭前，记录一下，窗体宽高
            var isChange = false;
            var templateForm = this._appConfig.formSetting.templateForm;
            if (templateForm.formSize == null)
            {
                templateForm.formSize = new FormSize(this.Width, this.Height, (int)this.WindowState);
                isChange = true;
            }
            var formSize = templateForm.formSize;

            if ((int)this.WindowState != formSize.FormWindowState)
            {
                formSize.FormWindowState = (int)this.WindowState;
                isChange = true;
            }
            else if ((formSize.Width != this.Width || formSize.Height != this.Height) && formSize.FormWindowState == (int)FormWindowState.Normal)
            {
                formSize.Width = this.Width;
                formSize.Height = this.Height;
                isChange = true;
            }
            if (isChange) Utils.writeToConfFile(this._appConfig);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.isInitting) return;

            if (this._appConfig.formSetting.templateForm.splitterDistance != this.splitContainer1.SplitterDistance)
            {
                this._appConfig.formSetting.templateForm.splitterDistance = this.splitContainer1.SplitterDistance;
                Utils.writeToConfFile(this._appConfig);
            }
        }

        #endregion

        #region 左侧TreeView相关

        private Point _clickPoint;


        #region 右键菜单点击事件对应功能

        private void 重命名F2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.triggerReName();
        }

        private void 打开所在目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFolder();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flushTreeViewBindFiles();
        }

        private void 文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.InsertFileOrFolder(true);
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.InsertFileOrFolder(false);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RemoveFileOrFolder();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CopyFileOrFolder();
        }

        #endregion

        #region 复制 + 粘贴文件（夹）

        string _CPNodeFullPath;
        /// <summary>
        /// Ctrl + C 复制开始
        /// </summary>
        private void CopyPasteFileOrFolder_Start()
        {
            _CPNodeFullPath = this.treeView_Files.SelectedNode?.FullPath;
        }

        /// <summary>
        /// Ctrl + V 复制粘贴结束
        /// </summary>
        private void CopyPastFileOrFolder_End()
        {
            var targetNode = this.treeView_Files.SelectedNode;
            if (_CPNodeFullPath == null || targetNode == null) return;

            var targetPath = CopyPastFileOrFolder(Utils._tempPath + "\\" + _CPNodeFullPath, Utils._tempPath + "\\" + targetNode.FullPath);
            if (targetPath != null)
            {
                var findedNodes = this.treeView_Files.Nodes.Find(targetPath, true);
                if (findedNodes.Length > 0)
                {
                    this.treeView_Files.SelectedNode = findedNodes[0];
                    _CPNodeFullPath = this.treeView_Files.SelectedNode.FullPath;//防止在选中的文件夹下连续粘贴造成递归查询异常
                }
            }
        }

        /// <summary>
        /// 复制文件（夹）到另一个目录
        /// </summary>
        /// <returns>返回非NULL时，代表复制成功</returns>
        private string CopyPastFileOrFolder(string originalPath, string targetPath)
        {
            if (string.IsNullOrEmpty(originalPath) || string.IsNullOrEmpty(targetPath)) return null;
            var isFolder = Utils.IsDir(originalPath);
            var originalPathIsExist = isFolder ? Directory.Exists(originalPath) : File.Exists(originalPath);
            if (!originalPathIsExist) return null;

            var oldText = originalPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
            // 目标是文件时，需要放到所在文件夹下
            if (!Utils.IsDir(targetPath)) targetPath = targetPath.Substring(0, targetPath.LastIndexOf("\\"));
            var newPath = targetPath + "\\" + oldText;
            if (isFolder)
            {
                while (Directory.Exists(newPath))
                {
                    newPath += repeatAppend;
                }
                Utils.CopyDirectory(originalPath, newPath);
            }
            else
            {
                var newText = oldText;
                while (File.Exists(newPath))
                {
                    var listS = newText.Split('.');
                    var newName = listS[0] + repeatAppend;
                    for (int i = 1; i < listS.Length; i++)
                    {
                        newName = newName + "." + listS[i];
                    }
                    newPath = newPath.Substring(0, newPath.Length - newText.Length) + newName;
                    newText = newName;
                }
                File.Copy(originalPath, newPath, false);
            }
            this.flushTreeViewBindFiles();
            return newPath;
        }

        #endregion

        /// <summary>
        /// 节点展开后
        /// </summary>
        private void treeView_Files_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (this.isInitting || isBindingTreeview) return;

            var fullPath = e.Node.FullPath;
            var tf = this._appConfig.formSetting.templateForm;
            if (tf.collapseNodes.Contains(fullPath))
            {
                tf.collapseNodes.Remove(fullPath);
                Utils.writeToConfFile(_appConfig);
            }
        }

        /// <summary>
        /// 节点折叠后
        /// </summary>
        private void treeView_Files_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (this.isInitting || isBindingTreeview) return;

            var fullPath = e.Node.FullPath;

            var tf = this._appConfig.formSetting.templateForm;
            if (!tf.collapseNodes.Contains(fullPath))
            {
                tf.collapseNodes.Add(fullPath);
                Utils.writeToConfFile(_appConfig);
            }
        }


        /// <summary>
        /// 切换正在编辑文件的图标
        /// </summary>
        private void switchEditStatus()
        {
            if (this._editNode != null)
            {
                var isEdit = _editOldText != this._editor.Text;
                var imgIdx = isEdit ? 3 : 2;
                if (_editNode.ImageIndex != imgIdx || _editNode.SelectedImageIndex != imgIdx)
                {
                    this._editNode.ImageIndex = this._editNode.SelectedImageIndex = imgIdx;
                }
            }
        }

        /// <summary>
        /// 双击文件时打开编辑器
        /// </summary>
        private void treeView_Files_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.openSelectedFileOnEditor();
        }

        /// <summary>
        /// 在编辑器中打开选中的文件
        /// </summary>
        private void openSelectedFileOnEditor()
        {
            if (this.treeView_Files.SelectedNode == null) return;
            var fullPath = this.treeView_Files.SelectedNode.FullPath;
            var oldFullPath = Utils._tempPath + "\\" + fullPath;
            if (!Utils.IsDir(oldFullPath) && File.Exists(oldFullPath))
            {
                if (fileDocumentChangeCheck())
                {
                    // 搜索框未关闭时，同时关闭
                    if (this.findReplaceControl1.Visible) { this.findReplaceControl1.CloseFindDialog(); }

                    var strFile = File.ReadAllText(oldFullPath, Encoding.UTF8);

                    var splitStr = fullPath.Split('.').Where(w => "." + w != Utils._tempFileSuffix).ToList();
                    var suffix = splitStr.Count == 1 ? Utils._defaultFileSuffix : "." + splitStr.Last();

                    // 设置高亮语法，如果没有语法支持，默认使用Markdown
                    _editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(suffix) ?? HighlightingManager.Instance.GetDefinitionByExtension(".md");
                    foreach (var item in _customerHighlightingRules)
                    {
                        if (_editor.SyntaxHighlighting.MainRuleSet.Rules.Contains(item)) break;
                        _editor.SyntaxHighlighting.MainRuleSet.Rules.Add(item);
                    }

                    _editor.Text = strFile;
                    this.switchEditorEditable(true);

                    _editOldText = strFile;
                    _editingFilePath = oldFullPath;

                    if (_editNode != null && _editNode != this.treeView_Files.SelectedNode)
                    {
                        _editNode.ImageIndex = _editNode.SelectedImageIndex = 1;
                    }

                    _editNode = this.treeView_Files.SelectedNode;
                    _editNode.ImageIndex = _editNode.SelectedImageIndex = 2;
                }
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        private void treeView_Files_MouseDown(object sender, MouseEventArgs e)
        {
            _clickPoint = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                TreeNode CurrentNode = this.treeView_Files.GetNodeAt(_clickPoint);
                if (CurrentNode != null && CurrentNode.Bounds.Contains(_clickPoint.X, _clickPoint.Y))//判断是不是点在具体的节点上
                {
                    treeView_Files.SelectedNode = CurrentNode;//选中这个节点
                }
                else
                {
                    treeView_Files.SelectedNode = null;
                }
            }
        }

        /// <summary>
        /// 激活重命名
        /// </summary>
        private void triggerReName()
        {
            if (_clickPoint == null) return;
            TreeNode node = this.treeView_Files.GetNodeAt(_clickPoint);//检索位于指定点的树节点
            if (node == null) return;

            if (_clickPoint.X >= node.Bounds.Left && _clickPoint.X <= node.Bounds.Right)//当鼠标双击的位置在某个结点上时
            {
                treeView_Files.LabelEdit = true;//开启树结点的标签文本编辑状态
                if (!treeView_Files.SelectedNode.IsEditing)
                {
                    treeView_Files.SelectedNode.BeginEdit();//使该结点进入编辑状态
                }
            }
        }

        /// <summary>
        /// 重命名后处理
        /// </summary>
        private void treeView_Files_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // 文件存在时进行自动追加重命名
            (string fullPath, string newName) fileExistToRename(string fullPath, string oldText)
            {
                while (File.Exists(fullPath))
                {
                    var listS = oldText.Split('.');
                    var newName = listS[0] + repeatAppend;
                    for (int i = 1; i < listS.Length; i++)
                    {
                        newName = newName + "." + listS[i];
                    }
                    fullPath = fullPath.Substring(0, fullPath.Length - oldText.Length) + newName;
                    oldText = newName;
                }
                return (fullPath, oldText);
            }

            if (e.Label != null)
            {
                var label = e.Label.Trim();
                if (string.IsNullOrEmpty(label))
                {
                    e.CancelEdit = true;
                    return;
                }
                var curNode = this.treeView_Files.SelectedNode;
                var fullPath = curNode.FullPath;
                var oldName = curNode.Text;
                var oldFullPath = Utils._tempPath + "\\" + fullPath;
                var newFullPath = Utils._tempPath + "\\" + fullPath.Substring(0, fullPath.Length - oldName.Length) + label;
                if (oldFullPath == newFullPath) return;
                if (Utils.IsDir(oldFullPath))
                {
                    while (label != oldName && Directory.Exists(newFullPath))
                    {
                        newFullPath += repeatAppend;
                        label += repeatAppend;
                    }
                    curNode.Text = label;
                    e.CancelEdit = true;
                    if (label != oldName)
                        Directory.Move(oldFullPath, newFullPath);
                }
                else
                {
                    if (!label.EndsWith(Utils._tempFileSuffix))
                    {
                        curNode.Text = label + Utils._tempFileSuffix;
                        newFullPath += Utils._tempFileSuffix;
                        label = curNode.Text;
                    }

                    var rstNew = fileExistToRename(newFullPath, label);

                    curNode.Name = rstNew.fullPath;
                    curNode.Text = rstNew.newName;
                    newFullPath = rstNew.fullPath;
                    e.CancelEdit = true;

                    File.Move(oldFullPath, newFullPath);
                }
            }
        }

        /// <summary>
        /// 新增文件或文件夹
        /// </summary>
        private void InsertFileOrFolder(bool isFolder)
        {
            var strName = isFolder ? "新增文件夹" : "新增文件.nxt";
            var tn = this.treeView_Files.SelectedNode;
            TreeNode curNode = tn;
            if (tn != null)
            {
                var curPath = Utils._tempPath + "\\" + tn.FullPath;
                if (!Utils.IsDir(curPath))
                {
                    if (tn.Parent != null)
                    {
                        curNode = tn.Parent.Nodes.Insert(0, strName);
                    }
                    else
                    {
                        curNode = this.treeView_Files.Nodes.Insert(0, strName);
                    }
                }
                else
                {
                    curNode = tn.Nodes.Insert(0, strName);
                }
            }
            else
            {
                curNode = this.treeView_Files.Nodes.Insert(0, strName);
            }

            curNode.ImageIndex = curNode.SelectedImageIndex = isFolder ? 0 : 1;
            var fullPath = Utils._tempPath + "\\" + curNode.FullPath;
            var oldText = curNode.Text;

            if (isFolder)
            {
                while (Directory.Exists(fullPath))
                {
                    fullPath += repeatAppend;
                    oldText += repeatAppend;
                    curNode.Text = oldText;
                }

                Directory.CreateDirectory(fullPath);
            }
            else
            {
                while (File.Exists(fullPath))
                {
                    var listS = oldText.Split('.');
                    var newName = listS[0] + repeatAppend;
                    for (int i = 1; i < listS.Length; i++)
                    {
                        newName = newName + "." + listS[i];
                    }
                    curNode.Text = newName;
                    fullPath = fullPath.Substring(0, fullPath.Length - oldText.Length) + newName;
                    oldText = newName;
                }

                File.AppendAllLines(fullPath, new string[] { }, Encoding.UTF8);
                curNode.Name = fullPath;
            }

            this.flushTreeViewBindFiles();
            if (fullPath != null)
            {
                var findedNodes = this.treeView_Files.Nodes.Find(fullPath, true);
                if (findedNodes.Length > 0)
                {
                    this.treeView_Files.SelectedNode = findedNodes[0];
                    this.treeView_Files.LabelEdit = true;
                    this.treeView_Files.SelectedNode.BeginEdit();
                }
            }
        }

        /// <summary>
        /// 复制并粘贴文件或文件夹
        /// </summary>
        private void CopyFileOrFolder()
        {
            var tn = this.treeView_Files.SelectedNode;
            if (tn == null) return;
            var curPath = Utils._tempPath + "\\" + tn.FullPath;
            var isFolder = Utils.IsDir(curPath);
            var oldText = tn.Text;
            string newPath = null;
            if (isFolder)
            {
                newPath = curPath + repeatAppend;
                var newText = oldText;
                while (Directory.Exists(newPath))
                {
                    newPath += repeatAppend;
                    newText += repeatAppend;
                }
                Utils.CopyDirectory(curPath, newPath);
            }
            else
            {
                var newText = oldText;
                newPath = curPath;
                while (File.Exists(newPath))
                {
                    var listS = newText.Split('.');
                    var newName = listS[0] + repeatAppend;
                    for (int i = 1; i < listS.Length; i++)
                    {
                        newName = newName + "." + listS[i];
                    }
                    newPath = newPath.Substring(0, newPath.Length - newText.Length) + newName;
                    newText = newName;
                }
                File.Copy(curPath, newPath, false);
            }
            this.flushTreeViewBindFiles();
            if (newPath != null)
            {
                var findedNodes = this.treeView_Files.Nodes.Find(newPath, true);
                if (findedNodes.Length > 0)
                {
                    this.treeView_Files.SelectedNode = findedNodes[0];
                }
            }
        }

        /// <summary>
        /// 删除文件或文件夹
        /// </summary>
        private void RemoveFileOrFolder()
        {
            if (this.treeView_Files.SelectedNode == null) return;
            if (MessageBox.Show("此操作不可逆，删除后无法恢复", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                var fullPath = this.treeView_Files.SelectedNode.FullPath;
                var oldFullPath = Utils._tempPath + "\\" + fullPath;
                if (Utils.IsDir(oldFullPath))
                {
                    if (Directory.Exists(oldFullPath)) Directory.Delete(oldFullPath, true);
                }
                else
                {
                    if (File.Exists(oldFullPath))
                    {
                        File.Delete(oldFullPath);
                        if (_editNode != null && _editNode.Name == oldFullPath)
                        {
                            this._editNode = null;
                            this._editingFilePath = null;
                            this._editor.Clear();
                            this.switchEditorEditable(false);
                        }
                    }
                }
                this.treeView_Files.SelectedNode.Remove();
            }
        }

        /// <summary>
        /// 在资源管理器中打开文件夹
        /// </summary>
        private void OpenFolder()
        {
            var node = this.treeView_Files.SelectedNode;

            var fullPath = Utils._tempPath + "\\" + node?.FullPath;
            if (!Utils.IsDir(fullPath))
            {
                fullPath = Utils._tempPath + "\\" + node.Parent?.FullPath;
            }
            System.Diagnostics.Process.Start("explorer.exe", fullPath);
        }

        #endregion

        #region 拖拽移动文件

        private void treeView_Files_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!string.IsNullOrEmpty(this._editingFilePath))
                {
                    MessageBox.Show("移动前，请先关闭正在编辑的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void treeView_Files_DragEnter(object sender, DragEventArgs e)
        {
            //判断拖拽位置是否是节点
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //拖拽结束，Drop放下时执行
        private void treeView_Files_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode))) return;
            if (_moveTargetNode != null) { _moveTargetNode.BackColor = Color.Transparent; _moveTargetNode = null; }
            // 源节点
            var srcNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            var targetNode = this.treeView_Files.GetNodeAt(treeView_Files.PointToClient(new Point(e.X, e.Y)));
            if (srcNode == targetNode) return;

            var newNode = (TreeNode)srcNode.Clone();
            var srcFullPath = Utils._tempPath + "\\" + srcNode.FullPath;
            var targetFullPath = Utils._tempPath + "\\" + targetNode?.FullPath;
            var targetIsDir = Utils.IsDir(targetFullPath);//目前是否是文件夹
            var srcIsDir = Utils.IsDir(srcFullPath);
            var moveOk = false;
            if (targetNode != null)
            {
                if (targetIsDir)//只能移动到文件夹下
                {
                    // 不能从父节点向子节点移动
                    if (srcNode?.Parent != targetNode && srcNode.Nodes.Find(targetNode.Name, true).Length == 0)
                    {
                        targetNode.Nodes.Add(newNode);
                        moveOk = true;
                    }
                }
                else if (targetNode.Parent != null)//否则找上层文件夹
                {
                    if (targetNode.Parent != srcNode.Parent && srcNode.Nodes.Find(targetNode.Name, true).Length == 0)
                    {
                        targetNode.Parent.Nodes.Add(newNode);
                        moveOk = true;
                        targetFullPath = Utils._tempPath + "\\" + targetNode.Parent.FullPath;
                    }
                }
                else if (targetNode.Parent == null)//无上层文件夹，则放到根目录
                {
                    this.treeView_Files.Nodes.Add(newNode);
                    moveOk = true;
                    targetFullPath = Utils._tempPath + "\\" + targetNode.Parent.FullPath;
                }
            }
            else
            {
                this.treeView_Files.Nodes.Add(newNode);
                moveOk = true;
                targetFullPath = Utils._tempPath;
            }
            if (moveOk)
            {
                try
                {
                    if (srcIsDir)
                    {
                        Directory.Move(srcFullPath, targetFullPath + "\\" + srcNode.Text);
                    }
                    else
                    {
                        File.Move(srcFullPath, targetFullPath + "\\" + srcNode.Text);
                    }

                    srcNode.Remove();
                    newNode.ExpandAll();
                    treeView_Files.Refresh();
                    treeView_Files.SelectedNode = newNode;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("移动失败！" + ex.Message);
                }
            }
        }

        TreeNode _moveTargetNode;
        private void treeView_Files_DragOver(object sender, DragEventArgs e)
        {
            var newPoint = treeView_Files.PointToClient(new Point(e.X, e.Y));
            TreeNode node = this.treeView_Files.GetNodeAt(newPoint);
            if (node != _moveTargetNode)
            {
                if (_moveTargetNode != null) _moveTargetNode.BackColor = Color.Empty;
                if (node != null) node.BackColor = ColorTranslator.FromHtml("#0078d7");
                _moveTargetNode = node;
            }
        }

        #endregion

        #region 右侧编辑器相关

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.closeEditingFile();
        }

        private void 保存CtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveEditingFile();
        }

        /// <summary>
        /// 切换编辑器的编辑状态
        /// </summary>
        private void switchEditorEditable(bool canEdit)
        {
            _editor.IsReadOnly = !canEdit;
            _editor.Focusable = canEdit;
            _editor.IsEnabled = canEdit;
            this.elementHost1.ContextMenuStrip = canEdit ? this.contextMenuStrip_Editor : null;
        }

        /// <summary>
        /// 关闭正在编辑的文件
        /// </summary>
        private void closeEditingFile()
        {
            // 搜索框未关闭时，优先关闭
            if (this.findReplaceControl1.Visible) { this.findReplaceControl1.CloseFindDialog(); return; }

            if (fileDocumentChangeCheck())
            {
                if (_editNode != null) this._editNode.SelectedImageIndex = this._editNode.ImageIndex = 1;
                this._editNode = null;
                this._editingFilePath = null;
                this._editor.Clear();
                this.switchEditorEditable(false);
            }
        }

        /// <summary>
        /// 保存正在编辑的文件
        /// </summary>
        private void saveEditingFile()
        {
            if (!string.IsNullOrEmpty(_editingFilePath))
                Utils.WriteToFile(_editingFilePath, this._editor.Text);

            this._editOldText = this._editor.Text;
            this.switchEditStatus();
        }

        /// <summary>
        /// 上下移动文档行
        /// </summary>
        private void moveEditorDocumentRow(bool moveUp)
        {
            if (_editor.Document != null && _editor.Document.LineCount > 1)
            {
                var document = _editor.Document;
                var currentLine = document.GetLineByOffset(_editor.CaretOffset);
                if (currentLine != null)
                {
                    document.BeginUpdate();
                    var oldCaretOffset = _editor.CaretOffset;//记录原光标位置
                    var currentLineText = document.GetText(currentLine.Offset, currentLine.Length);
                    if (moveUp)
                    {
                        var preLine = currentLine.PreviousLine;
                        if (preLine != null)
                        {
                            var preLineText = document.GetText(preLine.Offset, preLine.Length);
                            document.Replace(preLine.Offset, preLine.Length, currentLineText);
                            document.Replace(currentLine.Offset, currentLine.Length, preLineText);
                            _editor.CaretOffset = oldCaretOffset - currentLine.TotalLength;
                        }
                    }
                    else
                    {
                        var nextLine = currentLine.NextLine;
                        if (nextLine != null)
                        {
                            var nextLineText = document.GetText(nextLine.Offset, nextLine.Length);
                            document.Replace(currentLine.Offset, currentLine.Length, nextLineText);
                            document.Replace(nextLine.Offset, nextLine.Length, currentLineText);
                            _editor.CaretOffset = oldCaretOffset + currentLine.TotalLength;
                        }
                    }
                    document.EndUpdate();
                }
            }
        }

        /// <summary>
        /// 文件更改校验
        /// </summary>
        private bool fileDocumentChangeCheck()
        {
            if (_editor != null)
            {
                if (this._editor.Text != _editOldText && !string.IsNullOrEmpty(_editingFilePath))
                {
                    var rstDig = MessageBox.Show("内容已修改，是否需要保存", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (rstDig == DialogResult.Yes)
                    {
                        Utils.WriteToFile(_editingFilePath, this._editor.Text);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}