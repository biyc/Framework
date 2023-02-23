using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("ResIndex")]
    public class ResIndexCsv : CsvBaseTable<ResIndexRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new ResIndexRow();
                    name = "Sn";
                    row.Sn = CsvTypeHelper.ToInt(parts[0]);
                    name = "Type";
                    row.Type = CsvTypeHelper.ToEnum<ResType>(parts[1]);
                    name = "NameDesc";
                    row.NameDesc = CsvTypeHelper.ToString(parts[2]);
                    name = "Module";
                    row.Module = CsvTypeHelper.ToInt(parts[3]);
                    name = "InitNum";
                    row.InitNum = CsvTypeHelper.ToInt(parts[4]);
                    name = "IconDef";
                    row.IconDef = CsvTypeHelper.ToString(parts[5]);
            
                    InitRowByUnid(""+row.Sn,row);
                    _content.Add(row);
                    ProcessRow(row);
                    count++;
                }
                catch (Exception e)
                {
                    Tuner.Error($"[>>] Parse Table Error: {e} ");
                    Tuner.Log($"[>>] Table:[[{GetCsvName()}]] - Row:[[{count}]] - Col:[[{name}]]");
                    Tuner.Log(item);
                }
            }
        }

        public override string GetCsvName()
        {
            return "Hotfix.Game.Common$ResIndex.csv";
        }

        public virtual void ProcessRow(ResIndexRow row)
        {
        }
    }

    public class ResIndexRow
    {
        /// <summary>
        /// 资源编号
        /// </summary>
        public int Sn { get; set; }

        /// <summary>
        /// 资源名
        /// </summary>
        public ResType Type { get; set; }

        /// <summary>
        /// 资源中文名
        /// </summary>
        public string NameDesc { get; set; }

        /// <summary>
        /// 资源所在模块(0手动指定，1背包)
        /// </summary>
        public int Module { get; set; }

        /// <summary>
        /// 初始数量
        /// </summary>
        public int InitNum { get; set; }

        /// <summary>
        /// 默认资源图标
        /// </summary>
        public string IconDef { get; set; }
    }

    public static partial class CsvHelper
    {
        private static ResIndexCsv ResIndexCsv ;


        public static ResIndexCsv GetResIndexCsv()
        {
            return ResIndexCsv ?? (ResIndexCsv = new ResIndexCsv());
        }
    }
}
