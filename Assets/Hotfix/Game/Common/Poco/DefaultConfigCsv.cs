using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("DefaultConfig")]
    public class DefaultConfigCsv : CsvBaseTable<DefaultConfigRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new DefaultConfigRow();
                    name = "SN";
                    row.SN = CsvTypeHelper.ToInt(parts[0]);
                    name = "IndexName";
                    row.IndexName = CsvTypeHelper.ToString(parts[1]);
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[2]);
                    name = "Desc";
                    row.Desc = CsvTypeHelper.ToString(parts[3]);
                    name = "DefaultValue";
                    row.DefaultValue = CsvTypeHelper.ToInt(parts[4]);
                    name = "DefaultValueStr";
                    row.DefaultValueStr = CsvTypeHelper.ToString(parts[5]);
            
                    InitRowByUnid(""+row.SN,row);
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
            return "Hotfix.Game.Common$DefaultConfig.csv";
        }

        public virtual void ProcessRow(DefaultConfigRow row)
        {
        }
    }

    public class DefaultConfigRow
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SN { get; set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 初始参数
        /// </summary>
        public int DefaultValue { get; set; }

        /// <summary>
        /// 初始参数（字符）
        /// </summary>
        public string DefaultValueStr { get; set; }
    }

    public static partial class CsvHelper
    {
        private static DefaultConfigCsv DefaultConfigCsv ;


        public static DefaultConfigCsv GetDefaultConfigCsv()
        {
            return DefaultConfigCsv ?? (DefaultConfigCsv = new DefaultConfigCsv());
        }
    }
}
