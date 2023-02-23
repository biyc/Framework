using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("BuyGiftBag")]
    public class BuyGiftBagCsv : CsvBaseTable<BuyGiftBagRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new BuyGiftBagRow();
                    name = "Sn";
                    row.Sn = CsvTypeHelper.ToString(parts[0]);
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[1]);
                    name = "Type";
                    row.Type = CsvTypeHelper.ToString(parts[2]);
                    name = "ResType";
                    row.ResType = CsvTypeHelper.ToInt(parts[3]);
                    name = "ResValue";
                    row.ResValue = CsvTypeHelper.ToFloat(parts[4]);
                    name = "Award";
                    row.Award = ResInfoList.Create(parts[5]);
                    name = "Reward";
                    row.Reward = ResInfo.Create(parts[6]);
                    name = "RewardDesc";
                    row.RewardDesc = CsvTypeHelper.ToString(parts[7]);
                    name = "ImageName";
                    row.ImageName = CsvTypeHelper.ToString(parts[8]);
                    name = "Desc";
                    row.Desc = CsvTypeHelper.ToString(parts[9]);
            
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
            return "Hotfix.Game.Common$BuyGiftBag.csv";
        }

        public virtual void ProcessRow(BuyGiftBagRow row)
        {
        }
    }

    public class BuyGiftBagRow
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 展示类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 充值类型
        /// </summary>
        public int ResType { get; set; }

        /// <summary>
        /// 充值数量
        /// </summary>
        public float ResValue { get; set; }

        /// <summary>
        /// 奖励
        /// </summary>
        public ResInfoList Award { get; set; }

        /// <summary>
        /// 额外奖励
        /// </summary>
        public ResInfo Reward { get; set; }

        /// <summary>
        /// 额外描述
        /// </summary>
        public string RewardDesc { get; set; }

        /// <summary>
        /// 展示图片名称
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
    }

    public static partial class CsvHelper
    {
        private static BuyGiftBagCsv BuyGiftBagCsv ;


        public static BuyGiftBagCsv GetBuyGiftBagCsv()
        {
            return BuyGiftBagCsv ?? (BuyGiftBagCsv = new BuyGiftBagCsv());
        }
    }
}
