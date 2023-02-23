using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Csv.Poco;
using Blaze.Manage.Data;
using Blaze.Manage.Locale.Data;
using Blaze.Utility.Base;

namespace Blaze.Manage.Locale
{
    public class Lang
    {
        // 当前默认语言类型
        public static LocaleType CurLocaleType;

        // 修改语言类型通知
        public static IDataWatch<LocaleType> ChangeLocaleNotify = new DataWatch<LocaleType>();

        // 从找到的行数据中拿出当前语言的数据
        public static string GetTargetContext(LocaleRow row) => GetTargetContext(row, CurLocaleType);


        // 从找到的行数据中拿出目标语言的数据
        public static string GetTargetContext(LocaleRow row, LocaleType type)
        {
            switch (type)
            {
                case LocaleType.en: return row.en;
                case LocaleType.zh: return row.zh;
                case LocaleType.zh_tw: return row.zh_tw;
                case LocaleType.jp: return row.jp;
                case LocaleType.ko: return row.ko;
                case LocaleType.fr: return row.fr;
                case LocaleType.de: return row.de;
                case LocaleType.ru: return row.ru;
                case LocaleType.it: return row.it;
                case LocaleType.pt: return row.pt;
                case LocaleType.sp: return row.sp;
                case LocaleType.sw: return row.sw;
                case LocaleType.nl: return row.nl;
                default:
                    return "";
            }
        }


        /// <summary>
        /// 拿到指定的数据表
        /// </summary>
        /// <param name="name">表名</param>
        /// <returns></returns>
        public static LocaleCsvData Table(string name)
        {
            return LocaleManager._.GetLocaleData(name);
        }

        /// <summary>
        /// 拿到当前语言的数据项
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="id">数据项ID</param>
        /// <returns></returns>
        public static string Item(string name, string id)
        {
            return LocaleManager._.GetContent(name, id);
        }

        /// <summary>
        /// 从CodeLang表中取出多语言数据
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="id">数据项ID</param>
        /// <returns></returns>
        public static string Item(string id)
        {
            return LocaleManager._.GetContent("CodeLang", id);
        }


        /// <summary>
        /// 拿到指定语言的数据项
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="id">数据项ID</param>
        /// <param name="localeType">语言类型</param>
        /// <returns></returns>
        public static string Item(string name, string id, LocaleType localeType)
        {
            return LocaleManager._.GetContent(name, id, localeType);
        }

        /// <summary>
        /// 修改当前系统语言
        /// </summary>
        /// <param name="localeType"></param>
        public static void Change(LocaleType localeType)
        {
            LocaleManager._.ChangeLocaleTyep(localeType);
        }
    }
}