using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("Feel")]
    public class FeelCsv : CsvBaseTable<FeelRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new FeelRow();
                    name = "SN";
                    row.SN = CsvTypeHelper.ToInt(parts[0]);
                    name = "LikeType";
                    row.LikeType = CsvTypeHelper.ToEnum<FeelType>(parts[1]);
                    name = "RoleId";
                    row.RoleId = CsvTypeHelper.ToInt(parts[2]);
                    name = "SenseOne";
                    row.SenseOne = CsvTypeHelper.ToInt(parts[3]);
                    name = "SenseTwo";
                    row.SenseTwo = CsvTypeHelper.ToInt(parts[4]);
                    name = "GroupId";
                    row.GroupId = CsvTypeHelper.ToInt(parts[5]);
            
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
            return "Hotfix.Game.Common$Feel.csv";
        }

        public virtual void ProcessRow(FeelRow row)
        {
        }
    }

    public class FeelRow
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SN { get; set; }

        /// <summary>
        /// 喜爱类型
        /// </summary>
        public FeelType LikeType { get; set; }

        /// <summary>
        /// 人物ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 主好感度
        /// </summary>
        public int SenseOne { get; set; }

        /// <summary>
        /// 副好感度
        /// </summary>
        public int SenseTwo { get; set; }

        /// <summary>
        /// 组ID
        /// </summary>
        public int GroupId { get; set; }
    }

    public static partial class CsvHelper
    {
        private static FeelCsv FeelCsv ;


        public static FeelCsv GetFeelCsv()
        {
            return FeelCsv ?? (FeelCsv = new FeelCsv());
        }
    }
}
