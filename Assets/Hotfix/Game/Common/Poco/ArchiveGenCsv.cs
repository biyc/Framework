using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("ArchiveGen")]
    public class ArchiveGenCsv : CsvBaseTable<ArchiveGenRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new ArchiveGenRow();
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[0]);
                    name = "Age";
                    row.Age = CsvTypeHelper.ToInt(parts[1]);
                    name = "Type";
                    row.Type = CsvTypeHelper.ToInt(parts[2]);
                    name = "Items";
                    row.Items = CsvTypeHelper.ToIntArray(parts[3]);
                    name = "Chapters";
                    row.Chapters = CsvTypeHelper.ToIntArray(parts[4]);
                    name = "Coin";
                    row.Coin = CsvTypeHelper.ToInt(parts[5]);
                    name = "Phy";
                    row.Phy = CsvTypeHelper.ToInt(parts[6]);
            
            
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
            return "Hotfix.Game.Common$ArchiveGen.csv";
        }

        public virtual void ProcessRow(ArchiveGenRow row)
        {
        }
    }

    public class ArchiveGenRow
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 小屋装扮家具列表
        /// </summary>
        public List<int> Items { get; set; }

        /// <summary>
        /// 章节列表
        /// </summary>
        public List<int> Chapters { get; set; }

        /// <summary>
        /// 金币数量
        /// </summary>
        public int Coin { get; set; }

        /// <summary>
        /// 体力值
        /// </summary>
        public int Phy { get; set; }
    }

    public static partial class CsvHelper
    {
        private static ArchiveGenCsv ArchiveGenCsv ;


        public static ArchiveGenCsv GetArchiveGenCsv()
        {
            return ArchiveGenCsv ?? (ArchiveGenCsv = new ArchiveGenCsv());
        }
    }
}
