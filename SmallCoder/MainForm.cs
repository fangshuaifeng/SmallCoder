using DotLiquid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallCoder
{
    public partial class MainForm : Form
    {
        private List<TableColumn> dataColumns = new List<TableColumn>();
        private AppConfig _appConfig;
        private AppConnItem _currentSelItem;
        private int _MinWidth = 460;
        private int _MaxWidth = 777;
        Form tempForm;

        public MainForm()
        {
            InitializeComponent();
            this.getDpi();
        }

        private void getDpi()
        {
            var _dpiValue = Utils.GetScreenScaleValue(this);

            this._MinWidth = (int)(this._MinWidth * _dpiValue);
            this._MaxWidth = (int)(this._MaxWidth * _dpiValue);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.loadAndRefreshConfig();
            this.txtSpaceName.LostFocus += TxtSpaceName_LostFocus;
            this.rtbJson.Text = this._appConfig.customJson ?? string.Empty;
            this.checkBox_AppendSpace.Checked = this._appConfig.appendSpace;
            this.Text += $" - {Program._Version}";
            this.loadTemplate_DropdownList();
            this.initRtbJson();
            // 装载安全类型
            Template.RegisterSafeType(typeof(TableColumn), typeof(TableColumn).GetProperties().Select(p => p.Name).ToArray());
        }

        private void loadAndRefreshConfig()
        {
            _appConfig = Utils.LoadConfFile();
            this.cbDbCon.DataSource = _appConfig.connections;
            this._currentSelItem = _appConfig.connections.FirstOrDefault(f => f.selected) ?? _appConfig.connections.FirstOrDefault() ?? new AppConnItem();
            this.txtSpaceName.Text = _appConfig.spaceName;
            this.sameRefreshDbAndTable();
        }

        /// <summary>
        /// 加载输出模板下拉
        /// </summary>
        private void loadTemplate_DropdownList()
        {
            Task.Run(() =>
            {
                return Utils.GetOutTemplates().Prepend(String.Empty).ToList();
            }).ContinueWith(t =>
            {
                var cblistTemps = t.Result;
                this.BeginInvoke(new Action(() =>
                {
                    this.cbTemplate.SelectedValueChanged -= new System.EventHandler(this.cbTemplate_SelectedValueChanged);
                    this.cbTemplate.DataSource = cblistTemps;
                    if (!string.IsNullOrWhiteSpace(this._appConfig.defaultTemplate))
                    {
                        this.cbTemplate.SelectedIndex = cblistTemps.IndexOf(this._appConfig.defaultTemplate);
                    }
                    this.cbTemplate.SelectedValueChanged += new System.EventHandler(this.cbTemplate_SelectedValueChanged);
                }));
            });
        }

        private void lbl_Refresh_Click(object sender, EventArgs e)
        {
            this.sameRefreshDbAndTable();
        }

        #region 数据库和表下拉相关

        private void sameRefreshDbAndTable()
        {
            this.bind_Database_DropdownList(() =>
            {
                this.bind_Table_DropdownList();
            });
        }

        private void bind_Database_DropdownList(Action callback)
        {
            if (string.IsNullOrWhiteSpace(this._currentSelItem?.conn)) return;

            Task.Run(() =>
            {
                try
                {
                    return new SqlUtils(this._currentSelItem.conn).getAllDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"获取数据库失败：{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return new List<string>();
            }).ContinueWith((t) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.cbDb.SelectedValueChanged -= new System.EventHandler(this.cbDb_SelectedValueChanged);
                    var list = t.Result;
                    cbDb.DataSource = list;
                    if (list.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(this._currentSelItem.dbName))
                        {
                            if (!list.Any(a => a == this._currentSelItem.dbName))
                            {
                                this._currentSelItem.dbName = list.First();
                                Utils.writeToConfFile(this._appConfig);
                            }
                            cbDb.SelectedIndex = list.IndexOf(this._currentSelItem.dbName);
                        }
                    }
                    this.cbDb.SelectedValueChanged += new System.EventHandler(this.cbDb_SelectedValueChanged);
                    callback?.Invoke();
                }));
            });
        }

        private void bind_Table_DropdownList()
        {
            if (string.IsNullOrWhiteSpace(cbDbCon.Text) || string.IsNullOrEmpty(cbDb.Text)) return;

            this.cbEntity.DataSource = null;
            var currentSelDb = cbDb.Text;
            Task.Run(() =>
            {
                try
                {
                    return new SqlUtils(this._currentSelItem.conn).getAllTable(currentSelDb);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"获取数据表失败：{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return new List<string>();
            }).ContinueWith((t) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    cbEntity.DataSource = t.Result;
                }));
            });
        }

        /// <summary>
        /// 切换数据库连接
        /// </summary>
        private void cbDbCon_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._currentSelItem = this._appConfig.connections.FirstOrDefault(f => f.name == cbDbCon.Text) ?? new AppConnItem();
        }

        /// <summary>
        /// 切换数据库
        /// </summary>
        private void cbDb_SelectedValueChanged(object sender, EventArgs e)
        {
            this._currentSelItem.dbName = this.cbDb.Text.Trim();
            Utils.writeToConfFile(this._appConfig);
            this.bind_Table_DropdownList();
        }

        /// <summary>
        /// 切换数据表时执行
        /// </summary>
        private void cbEntity_SelectedValueChanged(object sender, EventArgs e)
        {
            var selEntity = this.cbEntity.Text.Trim();
            if (!string.IsNullOrWhiteSpace(selEntity))
            {
                txtEntityCustom.Text = Utils.UpperTable(selEntity);

                Task.Run(() =>
                {
                    try
                    {
                        dataColumns = new SqlUtils(this._currentSelItem.conn).getTableColumns(this._currentSelItem.dbName, selEntity);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"获取列信息失败：{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return new List<TableColumn>();
                });
            }
        }

        #endregion

        private Dictionary<string, object> transformCustomJson(string strJson, out bool checkOk)
        {
            checkOk = true;
            if (string.IsNullOrWhiteSpace(strJson)) return new Dictionary<string, object>();

            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);
            }
            catch (Exception ex)
            {
                checkOk = false;
                MessageBox.Show($"{this.lblJson.Text}无效，{ex.Message}", this.lblJson.Text);
            }
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// 点击生成
        /// </summary>
        private void btn_Generate_Click(object sender, EventArgs e)
        {
            if (this.btn_Generate.Enabled == false) return;
            Dictionary<string, object> json = transformCustomJson(this._appConfig.customJson, out var _checkOk); if (!_checkOk) return;

            var txtSpaceName = this.txtSpaceName.Text;
            var txtEntityCustome = this.txtEntityCustom.Text;
            var txtTableName = this.cbEntity.Text.Trim();
            var subTemplath = this.cbTemplate.Text.Trim();
            var subTemplateIsNotNull = !string.IsNullOrWhiteSpace(subTemplath);
            var filterTemps = this.txtFilter.Text.Trim();
            var txtDesc = this.txtDesc.Text.Trim();
            var appendSpace = this.checkBox_AppendSpace.Checked;

            // 参数
            var model = new
            {
                _SpaceName = txtSpaceName,
                _TableName = txtTableName,
                _EntityName = txtEntityCustome,
                _Columns = dataColumns,
                _Description = txtDesc,
                _Model = json,
            };
            this.btn_Generate.Enabled = false;
            this.btn_Generate.Text = "生成中...";
            Task.Run(() =>
            {
                var generateingFile = string.Empty;
                try
                {
                    var allFiles = Utils.GetAllFiles(subTemplateIsNotNull ? $"\\{subTemplath}" : null);
                    if (!string.IsNullOrWhiteSpace(filterTemps))
                    {
                        var filters = filterTemps.Split(new string[] { ";", "；" }, StringSplitOptions.RemoveEmptyEntries);
                        allFiles = allFiles.Where(w => !filters.Any(f => f.EndsWith(w.OutFileName))).ToList();
                    }

                    foreach (var item in allFiles)
                    {
                        generateingFile = item.RootPath;
                        var template = Utils.ReadFileToTemplate(item.FilePath);
                        var rstTemplate = template.Render(Hash.FromAnonymousObject(model));
                        if (template.Errors.Count > 0) throw template.Errors.First();//如果有异常抛出并结束
                        var outPath = item.OutPath;
                        if (subTemplateIsNotNull && appendSpace && outPath.Replace($"{Utils._outPath}\\{subTemplath}", String.Empty).LastIndexOf("\\") > -1)
                        {
                            var splitPath = outPath.Split(new string[] { $"\\{subTemplath}\\" }, StringSplitOptions.RemoveEmptyEntries);
                            outPath = $"{splitPath[0]}\\{subTemplath}\\{txtSpaceName}.{splitPath[1]}";
                        }
                        outPath = outPath.Replace($"\\EntityFolder", $"\\{txtEntityCustome}");//遇到EntityFolder目录替换成实体文件夹

                        if (!Directory.Exists(outPath))
                        {
                            Directory.CreateDirectory(outPath);
                        }

                        Utils.WriteToFile($"{outPath}/{txtEntityCustome}{item.OutFileName}{item.Suffix}", rstTemplate);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{generateingFile} 文件中断：{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }).ContinueWith((t) =>
            {
                this.Invoke(new Action(() =>
                {
                    this.btn_Generate.Text = "生成";
                    this.btn_Generate.Enabled = true;
                }));
                if (t.Result == true)
                {
                    var rstDig = MessageBox.Show("生成成功，是否打开目录", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    if (rstDig == DialogResult.Yes)
                    {
                        this.OpenOutputFolder(subTemplath);
                    }
                }
            });
        }

        /// <summary>
        /// 点击清理
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.btnClear.Enabled = false;
            this.btnClear.Text = "清理中...";
            Task.Run(() =>
            {
                Utils.DeleteDir(Utils._outPath);
            }).ContinueWith((t) =>
            {
                this.Invoke(new Action(() =>
                {
                    this.btnClear.Enabled = true;
                    this.btnClear.Text = "清理";
                }));
            });
        }

        private void OpenOutputFolder(string subTemplath)
        {
            System.Diagnostics.Process.Start("explorer.exe", Utils._outPath + "\\" + subTemplath);
        }

        /// <summary>
        /// 打开连接配置
        /// </summary>
        private void btnConf_Click(object sender, EventArgs e)
        {
            var dataConfForm = new DataConf();
            dataConfForm.Location = Utils.GetBestScreenLocation(this, dataConfForm.Width, dataConfForm.Height);
            if (dataConfForm.ShowDialog() == DialogResult.OK)
            {
                this.loadAndRefreshConfig();
            }
        }

        private void TxtSpaceName_LostFocus(object sender, EventArgs e)
        {
            if (this.txtSpaceName.Text != this._appConfig.spaceName)
            {
                this._appConfig.spaceName = this.txtSpaceName.Text;
                Utils.writeToConfFile(this._appConfig);
            }
        }

        /// <summary>
        /// 切换生成模板后执行
        /// </summary>
        private void cbTemplate_SelectedValueChanged(object sender, EventArgs e)
        {
            var txtTemp = this.cbTemplate.Text.Trim();
            if (txtTemp != this._appConfig.defaultTemplate)
            {
                this._appConfig.defaultTemplate = txtTemp;
                Utils.writeToConfFile(this._appConfig);
            }
        }

        #region 自定义JSON

        /// <summary>
        /// 展开/关闭Json
        /// </summary>
        private void lbl_CS_Click(object sender, EventArgs e)
        {
            if (this.Width >= _MinWidth - 10 && this.Width <= _MinWidth + 10)//展开Json面板
            {
                this.Width = _MaxWidth;
                //this.rtbJson.TabStop = true;
            }
            else
            {
                this.Width = _MinWidth;
                //this.rtbJson.TabStop = false;
            }
        }

        private void initRtbJson()
        {
            this.rtbJson.AutoWordSelection = false;
            if (this.rtbJson.Text != string.Empty) rtbJsonSwitchColor();
        }

        private void rtbJson_TextChanged(object sender, EventArgs e)
        {
            var txtJson = this.rtbJson.Text.Trim();
            var nowColor = string.IsNullOrWhiteSpace(txtJson) ? Color.Black : Color.HotPink;
            if (nowColor != this.lbl_CS.ForeColor) this.lbl_CS.ForeColor = nowColor;
        }

        private void btnJsonCheck_Click(object sender, EventArgs e)
        {
            var txt = this.rtbJson.Text.Trim();
            try
            {
                if (!string.IsNullOrWhiteSpace(txt))
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(txt);
                    this.rtbJson.Text = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
                    //MessageBox.Show("校验通过", this.lblJson.Text);
                    this.rtbJsonSwitchColor();
                }
            }
            catch (Exception ex)
            {
                var oldSelectionStart = this.rtbJson.SelectionStart;
                this.rtbJson.SelectAll();
                this.rtbJson.SelectionColor = this.rtbJson.ForeColor;
                rtbJson.SelectionStart = oldSelectionStart;
                rtbJson.ScrollToCaret();
                MessageBox.Show(ex.Message, this.lblJson.Text);
            }
        }

        /// <summary>
        /// 给自定义Json键添加颜色
        /// </summary>
        private void rtbJsonSwitchColor(Color? color = null)
        {
            var oldSelectionStart = this.rtbJson.SelectionStart;
            // 给json键添加颜色
            foreach (Match item in Utils.RegexJsonKeys(this.rtbJson.Text))
            {
                this.rtbJson.Select(item.Index + 1, item.Value.Length - 3);
                this.rtbJson.SelectionFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                this.rtbJson.SelectionColor = color ?? Color.ForestGreen;
            }
            this.rtbJson.DeselectAll();
            rtbJson.SelectionStart = oldSelectionStart;
            rtbJson.ScrollToCaret();
        }

        private void btnJsonSave_Click(object sender, EventArgs e)
        {
            var txt = this.rtbJson.Text.Trim();
            this._appConfig.customJson = txt;
            Utils.writeToConfFile(this._appConfig);
        }

        #endregion

        #region 编辑器相关

        /// <summary>
        /// 打开编辑器
        /// </summary>
        private void lbl_Editor_Click(object sender, EventArgs e)
        {
            if (tempForm == null || tempForm.IsDisposed)
            {
                tempForm = new TemplateEditor();
                tempForm.Location = Utils.GetBestScreenLocation(this, tempForm.Width, tempForm.Height);
                tempForm.FormClosed += TempForm_FormClosed;
            }
            tempForm.BringToFront();
            tempForm.Show();
        }

        private void TempForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.loadTemplate_DropdownList();
            this.tempForm = null;
            _appConfig = Utils.LoadConfFile();
        }

        #endregion

        /// <summary>
        /// 追加命名空间复选框
        /// </summary>
        private void checkBox_AppendSpace_CheckedChanged(object sender, EventArgs e)
        {
            this._appConfig.appendSpace = this.checkBox_AppendSpace.Checked;
            Utils.writeToConfFile(this._appConfig);
        }

        /// <summary>
        /// 在线文档
        /// </summary>
        private void label5_Click(object sender, EventArgs e)
        {
            var about = new AboutForm();
            about.Location = Utils.GetBestScreenLocation(this, about.Width, about.Height);
            about.ShowDialog();
        }

    }
}
