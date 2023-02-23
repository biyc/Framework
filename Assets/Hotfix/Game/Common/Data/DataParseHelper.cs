using System.Collections.Generic;
using Blaze.Manage.Csv;

namespace Hotfix.Game.Common.Data
{
    public class DataParseHelper
    {
        /// <summary>
        /// 直接解析数据
        /// xxx:yyy | xxx:yyy | xxx:yyy
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Dict(string data)
        {
            var obj = new Dictionary<string, string>();
            // 初步解析到原始字典中
            foreach (var line in data.Split('|'))
            {
                var item = line.Split(':');
                if (item.Length >= 2)
                    obj.Add(item[0], item[1]);
            }

            return obj;
        }

        /// <summary>
        /// 直接解析数据
        ///  1|2|3
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<int> ToIntArray(string data) => CsvTypeHelper.ToIntArray(data);
    }
}