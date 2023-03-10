using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("Campaign")]
    public class CampaignCsv : CsvBaseTable<CampaignRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new CampaignRow();
                    name = "CampaignID";
                    row.CampaignID = CsvTypeHelper.ToInt(parts[0]);
                    name = "CampaignName";
                    row.CampaignName = CsvTypeHelper.ToString(parts[1]);
                    name = "CampaignImage";
                    row.CampaignImage = CsvTypeHelper.ToString(parts[2]);
                    name = "CampaignTime";
                    row.CampaignTime = CsvTypeHelper.ToString(parts[3]);
                    name = "CampaignEndTime";
                    row.CampaignEndTime = CsvTypeHelper.ToString(parts[4]);
                    name = "CampaignType";
                    row.CampaignType = CsvTypeHelper.ToInt(parts[5]);
                    name = "CampaignLink";
                    row.CampaignLink = CsvTypeHelper.ToString(parts[6]);
                    name = "AppLink";
                    row.AppLink = CsvTypeHelper.ToString(parts[7]);
            
            
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
            return "Hotfix.Game.Common$Campaign.csv";
        }

        public virtual void ProcessRow(CampaignRow row)
        {
        }
    }

    public class CampaignRow
    {
        /// <summary>
        /// ???????????????ID
        /// </summary>
        public int CampaignID { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        public string CampaignImage { get; set; }

        /// <summary>
        /// ??????????????????
        /// </summary>
        public string CampaignTime { get; set; }

        /// <summary>
        /// ??????????????????
        /// </summary>
        public string CampaignEndTime { get; set; }

        /// <summary>
        /// 1????????????????????????2???????????????
        /// </summary>
        public int CampaignType { get; set; }

        /// <summary>
        /// ????????????2???????????????????????????
        /// </summary>
        public string CampaignLink { get; set; }

        /// <summary>
        /// ????????????2?????????????????????app??????????????????
        /// </summary>
        public string AppLink { get; set; }
    }

    public static partial class CsvHelper
    {
        private static CampaignCsv CampaignCsv ;


        public static CampaignCsv GetCampaignCsv()
        {
            return CampaignCsv ?? (CampaignCsv = new CampaignCsv());
        }
    }
}
