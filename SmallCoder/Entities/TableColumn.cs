namespace SmallCoder
{
    public class TableColumn
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// 程序内数据类型
        /// </summary>
        public string data_type { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string data_type_code { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        public string column_type { get; set; }
        /// <summary>
        /// 字符串长度
        /// </summary>
        public long? char_length { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool is_pri { get; set; }
        /// <summary>
        /// 数值长度
        /// </summary>
        public long? number_precision { get; set; }
        /// <summary>
        /// 数值精度
        /// </summary>
        public long? number_scale { get; set; }
    }
}
