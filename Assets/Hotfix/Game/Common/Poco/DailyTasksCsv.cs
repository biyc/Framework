using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("DailyTasks")]
    public class DailyTasksCsv : CsvBaseTable<DailyTasksRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new DailyTasksRow();
                    name = "SN";
                    row.SN = CsvTypeHelper.ToInt(parts[0]);
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[1]);
                    name = "Type";
                    row.Type = CsvTypeHelper.ToInt(parts[2]);
                    name = "Condition";
                    row.Condition = CsvTypeHelper.ToInt(parts[3]);
                    name = "Rewards";
                    row.Rewards = CsvTypeHelper.ToIntArray(parts[4]);
                    name = "WeightEnoughRewards";
                    row.WeightEnoughRewards = CsvTypeHelper.ToIntArray(parts[5]);
                    name = "Icon";
                    row.Icon = CsvTypeHelper.ToInt(parts[6]);
                    name = "Version";
                    row.Version = CsvTypeHelper.ToString(parts[7]);
            
            
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
            return "Hotfix.Game.Common$DailyTasks.csv";
        }

        public virtual void ProcessRow(DailyTasksRow row)
        {
        }
    }

    public class DailyTasksRow
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SN { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        public int Condition { get; set; }

        /// <summary>
        /// 奖励
        /// </summary>
        public List<int> Rewards { get; set; }

        /// <summary>
        /// 奖励数量
        /// </summary>
        public List<int> WeightEnoughRewards { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public int Icon { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }

    public static partial class CsvHelper
    {
        private static DailyTasksCsv DailyTasksCsv ;


        public static DailyTasksCsv GetDailyTasksCsv()
        {
            return DailyTasksCsv ?? (DailyTasksCsv = new DailyTasksCsv());
        }
    }
}
