using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("AnalysisInfo")]
    public class AnalysisInfoCsv : CsvBaseTable<AnalysisInfoRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new AnalysisInfoRow();
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[0]);
                    name = "Event";
                    row.Event = CsvTypeHelper.ToString(parts[1]);
                    name = "Once";
                    row.Once = CsvTypeHelper.ToBool(parts[2]);
            
            
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
            return "Hotfix.Game.Common$AnalysisInfo.csv";
        }

        public virtual void ProcessRow(AnalysisInfoRow row)
        {
        }
    }

    public class AnalysisInfoRow
    {
        /// <summary>
        /// 事件参数
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 是否每日最多只上报一次
        /// </summary>
        public bool Once { get; set; }
    }

    public static partial class CsvHelper
    {
        private static AnalysisInfoCsv AnalysisInfoCsv ;


        public static AnalysisInfoCsv GetAnalysisInfoCsv()
        {
            return AnalysisInfoCsv ?? (AnalysisInfoCsv = new AnalysisInfoCsv());
        }
    }
}
