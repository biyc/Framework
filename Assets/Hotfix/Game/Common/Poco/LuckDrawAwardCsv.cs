using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("LuckDrawAward")]
    public class LuckDrawAwardCsv : CsvBaseTable<LuckDrawAwardRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new LuckDrawAwardRow();
                    name = "Sn";
                    row.Sn = CsvTypeHelper.ToInt(parts[0]);
                    name = "ItemId";
                    row.ItemId = CsvTypeHelper.ToInt(parts[1]);
                    name = "ItemType";
                    row.ItemType = CsvTypeHelper.ToString(parts[2]);
                    name = "Reward";
                    row.Reward = ResInfo.Create(parts[3]);
                    name = "RecoveryPrice";
                    row.RecoveryPrice = CsvTypeHelper.ToInt(parts[4]);
                    name = "Probability";
                    row.Probability = CsvTypeHelper.ToFloat(parts[5]);
            
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
            return "Hotfix.Game.Common$LuckDrawAward.csv";
        }

        public virtual void ProcessRow(LuckDrawAwardRow row)
        {
        }
    }

    public class LuckDrawAwardRow
    {
        /// <summary>
        /// 奖池编号
        /// </summary>
        public int Sn { get; set; }

        /// <summary>
        /// 物品编号
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 物品类别
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// 额外奖励
        /// </summary>
        public ResInfo Reward { get; set; }

        /// <summary>
        /// 回收单价
        /// </summary>
        public int RecoveryPrice { get; set; }

        /// <summary>
        /// 获得概率（总和100）
        /// </summary>
        public float Probability { get; set; }
    }

    public static partial class CsvHelper
    {
        private static LuckDrawAwardCsv LuckDrawAwardCsv ;


        public static LuckDrawAwardCsv GetLuckDrawAwardCsv()
        {
            return LuckDrawAwardCsv ?? (LuckDrawAwardCsv = new LuckDrawAwardCsv());
        }
    }
}
