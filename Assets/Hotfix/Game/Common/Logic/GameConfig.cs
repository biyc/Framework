using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Csv.Poco;

namespace Hotfix.Game.Common.Logic
{
    /// <summary>
    /// 从CSV中获取配置信息
    /// </summary>
    public static class GameConfig
    {
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public static int GetInt(DefaultConfigType def) => CsvHelper.GetDefaultConfigCsv().GetRowByUnid((int) def).DefaultValue;

        /// <summary>
        /// 获取配置数据
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string GetStr(DefaultConfigType def) => CsvHelper.GetDefaultConfigCsv().GetRowByUnid((int) def).DefaultValueStr;

        /// <summary>
        /// 获取配置表中的原始配置信息
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public static DefaultConfigRow GetData(DefaultConfigType def) => CsvHelper.GetDefaultConfigCsv().GetRowByUnid((int) def);
    }
}