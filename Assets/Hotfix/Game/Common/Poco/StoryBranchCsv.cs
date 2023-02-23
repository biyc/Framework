using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("StoryBranch")]
    public class StoryBranchCsv : CsvBaseTable<StoryBranchRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new StoryBranchRow();
                    name = "BranchId";
                    row.BranchId = CsvTypeHelper.ToInt(parts[0]);
                    name = "ChapterId";
                    row.ChapterId = CsvTypeHelper.ToInt(parts[1]);
                    name = "RoleId";
                    row.RoleId = CsvTypeHelper.ToInt(parts[2]);
                    name = "CostRes";
                    row.CostRes = ResInfo.Create(parts[3]);
                    name = "CostTip";
                    row.CostTip = CsvTypeHelper.ToString(parts[4]);
                    name = "IsHaveBranch";
                    row.IsHaveBranch = CsvTypeHelper.ToBool(parts[5]);
            
                    InitRowByUnid(""+row.BranchId,row);
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
            return "Hotfix.Game.Common$StoryBranch.csv";
        }

        public virtual void ProcessRow(StoryBranchRow row)
        {
        }
    }

    public class StoryBranchRow
    {
        /// <summary>
        /// 分支编号
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// 章节ID
        /// </summary>
        public int ChapterId { get; set; }

        /// <summary>
        /// 分支对应的角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 购买需要花费的资源(钥匙)
        /// </summary>
        public ResInfo CostRes { get; set; }

        /// <summary>
        /// 显示在UI上的花费文案
        /// </summary>
        public string CostTip { get; set; }

        /// <summary>
        /// 是否拥有分支
        /// </summary>
        public bool IsHaveBranch { get; set; }
    }

    public static partial class CsvHelper
    {
        private static StoryBranchCsv StoryBranchCsv ;


        public static StoryBranchCsv GetStoryBranchCsv()
        {
            return StoryBranchCsv ?? (StoryBranchCsv = new StoryBranchCsv());
        }
    }
}
