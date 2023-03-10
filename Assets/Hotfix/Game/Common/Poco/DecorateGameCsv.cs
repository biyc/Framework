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
        /// ??????ID
        /// </summary>
        public int SN { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public int ChapterNumber { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public ResInfoList Reward { get; set; }

        /// <summary>
        /// ???????????????????????????UI
        /// </summary>
        public string RewardShowInfo { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public string RewardDesc { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public ResType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        public string AttachChapterType { get; set; }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        public string AttachChapterId { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public ResInfo Cost { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public string CostDesc { get; set; }

        /// <summary>
        /// ???????????????????????????????????????
        /// </summary>
        public string CostDescCover { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public string DottedLineColor { get; set; }

        /// <summary>
        /// ??????????????????
        /// </summary>
        public string PanelLocal { get; set; }

        /// <summary>
        /// ??????????????????
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
