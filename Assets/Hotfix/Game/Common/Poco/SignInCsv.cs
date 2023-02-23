using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("Signin")]
    public class SigninCsv : CsvBaseTable<SigninRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new SigninRow();
                    name = "Sn";
                    row.Sn = CsvTypeHelper.ToInt(parts[0]);
                    name = "Day";
                    row.Day = CsvTypeHelper.ToInt(parts[1]);
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[2]);
                    name = "RewardValue";
                    row.RewardValue = ResInfoList.Create(parts[3]);
                    name = "UiShow";
                    row.UiShow = CsvTypeHelper.ToString(parts[4]);
            
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
            return "Hotfix.Game.Common$Signin.csv";
        }

        public virtual void ProcessRow(SigninRow row)
        {
        }
    }

    public class SigninRow
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Sn { get; set; }

        /// <summary>
        /// 天数
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 奖励内容
        /// </summary>
        public ResInfoList RewardValue { get; set; }

        /// <summary>
        /// 要显示到UI上的内容
        /// </summary>
        public string UiShow { get; set; }
    }

    public static partial class CsvHelper
    {
        private static SigninCsv SigninCsv ;


        public static SigninCsv GetSigninCsv()
        {
            return SigninCsv ?? (SigninCsv = new SigninCsv());
        }
    }
}
