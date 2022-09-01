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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.Text = this.Text + " - " + Program._AppName;
            //this.Deactivate += new System.EventHandler(this.AboutForm_Deactivate);
            this.KeyPreview = true;
            HotKeyManager.AddFormControlHotKey(this, this.Close, Keys.Escape);
        }

        private void AboutForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbl_doc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //this.Deactivate -= new System.EventHandler(this.AboutForm_Deactivate);
            Utils.OpenWebUrl("https://blog.renzicu.com/2022/small-coder/");
        }

        private void lbl_liquid_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //this.Deactivate -= new System.EventHandler(this.AboutForm_Deactivate);
            Utils.OpenWebUrl("https://liquid.bootcss.com/");
        }
    }
}
