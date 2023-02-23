using System.Collections.Generic;
using Blaze.Common;
using Blaze.Core;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Csv.Poco;
using Blaze.Manage.Locale.Data;

namespace Blaze.Manage.Locale
{
    /// <summary>
    /// 多语言模块
    /// </summary>
    public class LocaleManager : Singeton<LocaleManager>
    {

        // 所有的语言表数据  语言表名 -> 语言表数据
        Dictionary<string, LocaleCsvData> _localeDatas = new Dictionary<string, LocaleCsvData>();


        /// <summary>
        /// 初始化模块
        /// </summary>
        public void Initialize()
        {
            // 默认多语言为中文
            Lang.CurLocaleType = LocaleType.zh;
            // 加载成功，通知观察者
            Lang.ChangeLocaleNotify.ChangeNotify(Lang.CurLocaleType);
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void OnDestroy()
        {
        }

        /// <summary>
        /// 获取当前的语言类型
        /// </summary>
        /// <returns></returns>
        public LocaleType GetCurLocaleType()
        {
            return Lang.CurLocaleType;
        }


        /// <summary>
        /// 修改语言类型
        /// </summary>
        /// <returns></returns>
        public void ChangeLocaleTyep(LocaleType localeType)
        {
            Lang.CurLocaleType = localeType;
            // 修改后通知观察者刷新数据
            Lang.ChangeLocaleNotify.ChangeNotify(localeType);
        }

        /// <summary>
        /// 根据表名和 多语言主键 获取对应语言的文本
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetContent(string moduleName, string id, LocaleType localeType)
        {
            if (!_localeDatas.ContainsKey(moduleName))
            {
                var localeCsvData = new LocaleCsvData(moduleName);
                _localeDatas.Add(moduleName, localeCsvData);
            }

            // 目标行的所有语言数据
            var rowdata = _localeDatas[moduleName].GetRowByUnid(id);
            // 提取到指定的语言数据并返回
            return Lang.GetTargetContext(rowdata, localeType);
        }




        /// <summary>
        /// 根据表名和 多语言主键 获取对应语言的文本
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetContent(string moduleName, string id)
        {
            if (!_localeDatas.ContainsKey(moduleName))
            {
                var localeCsvData = new LocaleCsvData(moduleName);
                _localeDatas.Add(moduleName, localeCsvData);
            }

            // 目标语言表中的行数据
            return _localeDatas[moduleName].GetContent(id);
        }


        public LocaleCsvData GetLocaleData(string moduleName)
        {
            if (!_localeDatas.ContainsKey(moduleName))
            {
                var localeCsvData = new LocaleCsvData(moduleName);
                _localeDatas.Add(moduleName, localeCsvData);
            }

            // 目标语言表
            return _localeDatas[moduleName];
        }

    }
}