using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("TipContents")]
    public class TipContentsCsv : CsvBaseTable<TipContentsRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new TipContentsRow();
                    name = "SN";
                    row.SN = CsvTypeHelper.ToInt(parts[0]);
                    name = "IndexName";
                    row.IndexName = CsvTypeHelper.ToString(parts[1]);
                    name = "Desc";
                    row.Desc = CsvTypeHelper.ToString(parts[2]);
                    name = "Content";
                    row.Content = CsvTypeHelper.ToString(parts[3]);
                    name = "AddedValue";
                    row.AddedValue = CsvTypeHelper.ToString(parts[4]);
            
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
            return "Hotfix.Game.Common$TipContents.csv";
        }

        public virtual void ProcessRow(TipContentsRow row)
        {
        }
    }

    public class TipContentsRow
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
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 提示内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string AddedValue { get; set; }
    }

    public static partial class CsvHelper
    {
        private static TipContentsCsv TipContentsCsv ;


        public static TipContentsCsv GetTipContentsCsv()
        {
            return TipContentsCsv ?? (TipContentsCsv = new TipContentsCsv());
        }
    }
}
