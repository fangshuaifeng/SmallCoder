using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallCoder
{
    public partial class DataConf : Form
    {
        public DataConf()
        {
            InitializeComponent();
            this.FormClosed += DataConf_FormClosed;
        }

        private void DataConf_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private string _strSqlCon;
        private bool _testOk = false;
        private AppConfig _appConfig = new AppConfig();

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.btnTest.Enabled = false; this.btnTest.Text = "连接中...";
            Task.Run(() =>
            {
                try
                {
                    using (var sqlCon = new MySqlConnection(this._strSqlCon))
                    {
                        sqlCon.Open();
                        var rst = sqlCon.Query<int?>("SELECT 1;").FirstOrDefault();
                        if (rst == 1)
                        {
                            this._testOk = true;
                            MessageBox.Show("连接成功");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._testOk = false;
                    MessageBox.Show(ex.Message);
                }
            }).ContinueWith((a) =>
            {
                this.Invoke(new Action(() => { this.btnTest.Enabled = true; this.btnTest.Text = "测试"; }));
            });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this._testOk)
            {
                this.btnSave.Enabled = false; this.btnSave.Text = "保存中...";
                var first = this._appConfig.connections.FirstOrDefault(f => f.name == this.txtName.Text.Trim());
                if (first == null)
                {
                    this._appConfig.connections.Add(new AppConnItem
                    {
                        name = this.txtName.Text.Trim(),
                        dbName = this.txtDbName.Text.Trim(),
                        conn = this.txt_Sql.Text.Trim()
                    });
                }
                else
                {
                    first.dbName = this.txtDbName.Text.Trim();
                    first.conn = this.txt_Sql.Text.Trim();
                }
                this.writeToConfFile();
                this.flushConfig();
                this.btnSave.Enabled = true; this.btnSave.Text = "保存";
                this.txt_Sql.Clear();
                this.txtDbName.Clear();
                this.txtName.Clear();
            }
            else
            {
                MessageBox.Show("连接测试未通过");
            }
        }

        private void txt_Sql_TextChanged(object sender, EventArgs e)
        {
            this._strSqlCon = txt_Sql.Text.Trim();
        }

        private void flushConfig()
        {
            _appConfig = Utils.LoadConfFile();
            this.Invoke(new Action(() =>
            {
                this.cbDatabase.DataSource = _appConfig.connections;
                var txtName = this.txtName.Text.Trim();
                if (!string.IsNullOrWhiteSpace(txtName))
                {
                    var idx = _appConfig.connections.FindIndex(f => f.name == txtName);
                    if (idx > -1) this.cbDatabase.SelectedIndex = idx;
                }
            }));
        }

        private void DataConf_Load(object sender, EventArgs e)
        {
            this.flushConfig();
        }

        private void writeToConfFile()
        {
            var strApp = Newtonsoft.Json.JsonConvert.SerializeObject(this._appConfig, formatting: Newtonsoft.Json.Formatting.Indented);
            Utils.WriteToFile(Utils._confPath, strApp);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var selApp = this.cbDatabase.Text;
            if (string.IsNullOrWhiteSpace(selApp)) return;

            var modApp = this._appConfig.connections.FirstOrDefault(f => f.name == selApp);
            if (modApp != null)
            {
                this._appConfig.connections.Remove(modApp);
                this.writeToConfFile();
                this.flushConfig();
            }
        }
    }
}
