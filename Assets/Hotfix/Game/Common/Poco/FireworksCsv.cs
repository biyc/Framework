using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("Fireworks")]
    public class FireworksCsv : CsvBaseTable<FireworksRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new FireworksRow();
                    name = "FireworksID";
                    row.FireworksID = CsvTypeHelper.ToInt(parts[0]);
                    name = "Probability";
                    row.Probability = CsvTypeHelper.ToInt(parts[1]);
                    name = "FireworksRole";
                    row.FireworksRole = CsvTypeHelper.ToInt(parts[2]);
                    name = "AniTime";
                    row.AniTime = CsvTypeHelper.ToInt(parts[3]);
            
                    InitRowByUnid(""+row.FireworksID,row);
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
            return "Hotfix.Game.Common$Fireworks.csv";
        }

        public virtual void ProcessRow(FireworksRow row)
        {
        }
    }

    public class FireworksRow
    {
        /// <summary>
        /// 烟花编号
        /// </summary>
        public int FireworksID { get; set; }

        /// <summary>
        /// 概率
        /// </summary>
        public int Probability { get; set; }

        /// <summary>
        /// 对应角色
        /// </summary>
        public int FireworksRole { get; set; }

        /// <summary>
        /// 播放一次的时间
        /// </summary>
        public int AniTime { get; set; }
    }

    public static partial class CsvHelper
    {
        private static FireworksCsv FireworksCsv ;


        public static FireworksCsv GetFireworksCsv()
        {
            return FireworksCsv ?? (FireworksCsv = new FireworksCsv());
        }
    }
}
