using Blaze.Manage.Csv;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Csv.Poco;

namespace Blaze.Manage.Locale.Data
{
    [CsvTable("Locale", Extend = true)]
    public class LocaleCsvData : LocaleCsv
    {
        // 对应多语言表的 sheet 名
        private readonly string _moduleName;

        public LocaleCsvData()
        {
            _moduleName = "";
        }

        public LocaleCsvData(string moduleName)
        {
            _moduleName = moduleName;
        }

        public override string GetCsvName()
        {
            return "Blaze.Manage.Locale$Locale&" + _moduleName + ".csv";
        }


        /// <summary>
        /// 根据行ID 拿到默认语言的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetContent(string id)
        {
            var rowdata = GetRowByUnid(id);
            if (rowdata == null)
            {
                return "";
            }

            return Lang.GetTargetContext(rowdata, Lang.CurLocaleType);
        }


        /// <summary>
        /// 根据行ID 拿到默认语言的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetContent(string id, LocaleType localeType)
        {
            var rowdata = GetRowByUnid(id);
            if (rowdata == null)
            {
                return "";
            }

            return Lang.GetTargetContext(rowdata, localeType);
        }
    }
}