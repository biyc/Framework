using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("Recharge")]
    public class RechargeCsv : CsvBaseTable<RechargeRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new RechargeRow();
                    name = "Sn";
                    row.Sn = CsvTypeHelper.ToString(parts[0]);
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[1]);
                    name = "ResType";
                    row.ResType = CsvTypeHelper.ToInt(parts[2]);
                    name = "ResValue";
                    row.ResValue = CsvTypeHelper.ToFloat(parts[3]);
                    name = "AddType";
                    row.AddType = CsvTypeHelper.ToInt(parts[4]);
                    name = "AddValue";
                    row.AddValue = CsvTypeHelper.ToInt(parts[5]);
                    name = "RewardType";
                    row.RewardType = CsvTypeHelper.ToInt(parts[6]);
                    name = "RewardValue";
                    row.RewardValue = CsvTypeHelper.ToInt(parts[7]);
                    name = "Desc";
                    row.Desc = CsvTypeHelper.ToString(parts[8]);
            
                    InitRowByUnid(""+row.Sn,row);
                    _content.Add(row);
                    ProcessRow(row);
                    count++;
                }
                catch (Exception e)
                {
                    Tuner.Error($"[>>] Parse Table Error: {e} ");
                    Tuner.Log($"[>>] Table:[[{GetCsvName()}]] - Row:[[{count}]] - Col:[[{name}]]");
                }
            }
        }

        public override string GetCsvName()
        {
            return "Hotfix.Game.Common$Recharge.csv";
        }

        public virtual void ProcessRow(RechargeRow row)
        {
        }
    }

    public class RechargeRow
    {
        /// <summary>
        /// ??????
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ??????????????????
        /// </summary>
        public int ResType { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public float ResValue { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public int AddType { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public int AddValue { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public int RewardType { get; set; }

        /// <summary>
        /// ??????????????????
        /// </summary>
        public int RewardValue { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string Desc { get; set; }
    }

    public static partial class CsvHelper
    {
        private static RechargeCsv RechargeCsv ;


        public static RechargeCsv GetRechargeCsv()
        {
            return RechargeCsv ?? (RechargeCsv = new RechargeCsv());
        }
    }
}
