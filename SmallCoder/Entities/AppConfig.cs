using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallCoder
{
    public class AppConfig
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string spaceName { get; set; }
        /// <summary>
        /// 上次使用模板
        /// </summary>
        public string defaultTemplate { get; set; }
        /// <summary>
        /// 自定义值
        /// </summary>
        public string customJson { get; set; }
        /// <summary>
        /// 模板追加命名空间
        /// </summary>
        public bool appendSpace { get; set; }

        /// <summary>
        /// 数据库配置
        /// </summary>
        public List<AppConnItem> connections = new List<AppConnItem>();

        /// <summary>
        /// 窗体设置
        /// </summary>
        public FormSetting formSetting { get; set; } = new FormSetting();
    }

    /// <summary>
    /// 窗体设置
    /// </summary>
    public class FormSetting
    {
        public TemplateFormSet templateForm { get; set; } = new TemplateFormSet();
    }

    public class TemplateFormSet
    {
        public FormSize formSize { get; set; }
        /// <summary>
        /// 拆分器宽度
        /// </summary>
        public int? splitterDistance { get; set; }
        /// <summary>
        /// 折叠的节点路径
        /// </summary>
        public List<string> collapseNodes { get; set; } = new List<string>();
    }

    public class FormSize
    {
        public FormSize() { }
        public FormSize(int width, int height, int formWindowState)
        {
            Width = width;
            Height = height;
            FormWindowState = formWindowState;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int FormWindowState { get; set; }
    }

    public class AppConnItem
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string dbName { get; set; }
        /// <summary>
        /// 链接字符串
        /// </summary>
        public string conn { get; set; }
        /// <summary>
        /// 默认选中
        /// </summary>
        public bool selected { get; set; }
    }
}
