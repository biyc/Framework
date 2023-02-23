using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("OutAward")]
    public class OutAwardCsv : CsvBaseTable<OutAwardRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new OutAwardRow();
                    name = "Sn";
                    row.Sn = CsvTypeHelper.ToInt(parts[0]);
                    name = "SSR";
                    row.SSR = CsvTypeHelper.ToInt(parts[1]);
                    name = "SpecProduct";
                    row.SpecProduct = CsvTypeHelper.ToIntArray(parts[2]);
                    name = "SpecProbability";
                    row.SpecProbability = CsvTypeHelper.ToFloatList(parts[3]);
                    name = "NormProduct";
                    row.NormProduct = CsvTypeHelper.ToIntArray(parts[4]);
                    name = "NormProbability";
                    row.NormProbability = CsvTypeHelper.ToFloatList(parts[5]);
                    name = "Treasure1";
                    row.Treasure1 = CsvTypeHelper.ToTreasure(parts[6]);
                    name = "Treasure2";
                    row.Treasure2 = CsvTypeHelper.ToTreasure(parts[7]);
                    name = "Treasure3";
                    row.Treasure3 = CsvTypeHelper.ToTreasure(parts[8]);
                    name = "Treasure4";
                    row.Treasure4 = CsvTypeHelper.ToTreasure(parts[9]);
            
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
            return "Hotfix.Game.Common$OutAward.csv";
        }

        public virtual void ProcessRow(OutAwardRow row)
        {
        }
    }

    public class OutAwardRow
    {
        /// <summary>
        /// 期数
        /// </summary>
        public int Sn { get; set; }

        /// <summary>
        /// 必出SSR
        /// </summary>
        public int SSR { get; set; }

        /// <summary>
        /// 稀有
        /// </summary>
        public List<int> SpecProduct { get; set; }

        /// <summary>
        /// 稀有概率
        /// </summary>
        public List<float> SpecProbability { get; set; }

        /// <summary>
        /// 普通
        /// </summary>
        public List<int> NormProduct { get; set; }

        /// <summary>
        /// 普通对应概率
        /// </summary>
        public List<float> NormProbability { get; set; }

        /// <summary>
        /// 偶遇奖池1(类型-数量-概率)
        /// </summary>
        public Treasure Treasure1 { get; set; }

        /// <summary>
        /// 偶遇奖池2(类型-数量-概率)
        /// </summary>
        public Treasure Treasure2 { get; set; }

        /// <summary>
        /// 偶遇奖池3(类型-数量-概率)
        /// </summary>
        public Treasure Treasure3 { get; set; }

        /// <summary>
        /// 偶遇奖池4(类型-数量-概率)
        /// </summary>
        public Treasure Treasure4 { get; set; }
    }

    public static partial class CsvHelper
    {
        private static OutAwardCsv OutAwardCsv ;


        public static OutAwardCsv GetOutAwardCsv()
        {
            return OutAwardCsv ?? (OutAwardCsv = new OutAwardCsv());
        }
    }
}
