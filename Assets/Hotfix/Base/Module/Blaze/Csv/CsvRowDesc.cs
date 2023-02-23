namespace Blaze.Manage.Csv
{
    // CSV 列数据描述信息
    public class CsvRowDesc
    {
        // 数据所在的编号
        public int Index { get; set; }

        // 数据的键
        public string Key { get; set; }

        // 数据类型描述
        public string Type { get; set; }

        // 注释描述
        public string Desc { get; set; }
    }
}