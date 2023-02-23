using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("DecorateGame")]
    public class DecorateGameCsv : CsvBaseTable<DecorateGameRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new DecorateGameRow();
                    name = "SN";
                    row.SN = CsvTypeHelper.ToInt(parts[0]);
                    name = "ChapterNumber";
                    row.ChapterNumber = CsvTypeHelper.ToInt(parts[1]);
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[2]);
                    name = "Desc";
                    row.Desc = CsvTypeHelper.ToString(parts[3]);
                    name = "Reward";
                    row.Reward = ResInfoList.Create(parts[4]);
                    name = "RewardShowInfo";
                    row.RewardShowInfo = CsvTypeHelper.ToString(parts[5]);
                    name = "RewardDesc";
                    row.RewardDesc = CsvTypeHelper.ToString(parts[6]);
                    name = "Type";
                    row.Type = CsvTypeHelper.ToEnum<ResType>(parts[7]);
                    name = "Param";
                    row.Param = CsvTypeHelper.ToString(parts[8]);
                    name = "AttachChapterType";
                    row.AttachChapterType = CsvTypeHelper.ToString(parts[9]);
                    name = "AttachChapterId";
                    row.AttachChapterId = CsvTypeHelper.ToString(parts[10]);
                    name = "Cost";
                    row.Cost = ResInfo.Create(parts[11]);
                    name = "CostDesc";
                    row.CostDesc = CsvTypeHelper.ToString(parts[12]);
                    name = "CostDescCover";
                    row.CostDescCover = CsvTypeHelper.ToString(parts[13]);
                    name = "DottedLineColor";
                    row.DottedLineColor = CsvTypeHelper.ToString(parts[14]);
                    name = "PanelLocal";
                    row.PanelLocal = CsvTypeHelper.ToString(parts[15]);
                    name = "IsDefaultUnlock";
                    row.IsDefaultUnlock = CsvTypeHelper.ToBool(parts[16]);
            
                    InitRowByUnid(""+row.SN,row);
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
            return "Hotfix.Game.Common$DecorateGame.csv";
        }

        public virtual void ProcessRow(DecorateGameRow row)
        {
        }
    }

    public class DecorateGameRow
    {
        /// <summary>
        /// 关卡ID
        /// </summary>
        public int SN { get; set; }

        /// <summary>
        /// 关卡编号
        /// </summary>
        public int ChapterNumber { get; set; }

        /// <summary>
        /// 关卡名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 奖励
        /// </summary>
        public ResInfoList Reward { get; set; }

        /// <summary>
        /// 要显示的奖励信息到UI
        /// </summary>
        public string RewardShowInfo { get; set; }

        /// <summary>
        /// 奖励描述
        /// </summary>
        public string RewardDesc { get; set; }

        /// <summary>
        /// 关卡类型
        /// </summary>
        public ResType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 附加剧情或手机
        /// </summary>
        public string AttachChapterType { get; set; }

        /// <summary>
        /// 剧情或手机编号
        /// </summary>
        public string AttachChapterId { get; set; }

        /// <summary>
        /// 消耗
        /// </summary>
        public ResInfo Cost { get; set; }

        /// <summary>
        /// 花费描述
        /// </summary>
        public string CostDesc { get; set; }

        /// <summary>
        /// 显示在封面上的花费描述信息
        /// </summary>
        public string CostDescCover { get; set; }

        /// <summary>
        /// 虚线颜色
        /// </summary>
        public string DottedLineColor { get; set; }

        /// <summary>
        /// 所在面板位置
        /// </summary>
        public string PanelLocal { get; set; }

        /// <summary>
        /// 是否默认解锁
        /// </summary>
        public bool IsDefaultUnlock { get; set; }
    }

    public static partial class CsvHelper
    {
        private static DecorateGameCsv DecorateGameCsv ;


        public static DecorateGameCsv GetDecorateGameCsv()
        {
            return DecorateGameCsv ?? (DecorateGameCsv = new DecorateGameCsv());
        }
    }
}
