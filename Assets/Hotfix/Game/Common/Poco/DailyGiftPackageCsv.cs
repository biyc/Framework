using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("DailyGiftPackage")]
    public class DailyGiftPackageCsv : CsvBaseTable<DailyGiftPackageRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new DailyGiftPackageRow();
                    name = "SN";
                    row.SN = CsvTypeHelper.ToInt(parts[0]);
                    name = "DressId";
                    row.DressId = CsvTypeHelper.ToInt(parts[1]);
            
            
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
            return "Hotfix.Game.Common$DailyGiftPackage.csv";
        }

        public virtual void ProcessRow(DailyGiftPackageRow row)
        {
        }
    }

    public class DailyGiftPackageRow
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SN { get; set; }

        /// <summary>
        /// 服饰ID
        /// </summary>
        public int DressId { get; set; }
    }

    public static partial class CsvHelper
    {
        private static DailyGiftPackageCsv DailyGiftPackageCsv ;


        public static DailyGiftPackageCsv GetDailyGiftPackageCsv()
        {
            return DailyGiftPackageCsv ?? (DailyGiftPackageCsv = new DailyGiftPackageCsv());
        }
    }
}
