using SmallCoder.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallCoder
{
    public partial class DataConf : Form
    {
        public DataConf()
        {
            InitializeComponent();
            this.cb_dbType.Items.AddRange(new object[] { "MySql", "Oracle", "SqlServer" });
            this.cb_dbType.SelectedIndex = 0;
            this.FormClosed += DataConf_FormClosed;
        }

        private void DataConf_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private string _strSqlCon;
        private DbType _dbType;
        private bool _testOk = false;
        private AppConfig _appConfig = new AppConfig();

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.btnTest.Enabled = false; this.btnTest.Text = "连接中...";
            Task.Run(() =>
            {
                try
                {
                    var isOk = new SqlUtils(this._strSqlCon, this._dbType).testConnection();
                    if (isOk)
                    {
                        this._testOk = true;
                        MessageBox.Show("连接成功");
                    }
                    else
                    {
                        this._testOk = false;
                        MessageBox.Show("连接失败");
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
                        conn = this.txt_Sql.Text.Trim(),
                        dbType = this.cb_dbType.Text.ToEnum<DbType>(),
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

        private void cb_dbType_SelectedValueChanged(object sender, EventArgs e)
        {
            this._dbType = this.cb_dbType.Text.ToEnum<DbType>();
        }
    }
}
