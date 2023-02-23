using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("GiftPack")]
    public class GiftPackCsv : CsvBaseTable<GiftPackRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new GiftPackRow();
                    name = "Sn";
                    row.Sn = CsvTypeHelper.ToInt(parts[0]);
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[1]);
                    name = "RewardValue";
                    row.RewardValue = ResInfoList.Create(parts[2]);
            
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
            return "Hotfix.Game.Common$GiftPack.csv";
        }

        public virtual void ProcessRow(GiftPackRow row)
        {
        }
    }

    public class GiftPackRow
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Sn { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 奖励内容
        /// </summary>
        public ResInfoList RewardValue { get; set; }
    }

    public static partial class CsvHelper
    {
        private static GiftPackCsv GiftPackCsv ;


        public static GiftPackCsv GetGiftPackCsv()
        {
            return GiftPackCsv ?? (GiftPackCsv = new GiftPackCsv());
        }
    }
}
