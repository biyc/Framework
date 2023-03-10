using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("Locale")]
    public class LocaleCsv : CsvBaseTable<LocaleRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new LocaleRow();
                    name = "ResId";
                    row.ResId = CsvTypeHelper.ToString(parts[0]);
                    name = "en";
                    row.en = CsvTypeHelper.ToString(parts[1]);
                    name = "zh";
                    row.zh = CsvTypeHelper.ToString(parts[2]);
                    name = "zh_tw";
                    row.zh_tw = CsvTypeHelper.ToString(parts[3]);
                    name = "jp";
                    row.jp = CsvTypeHelper.ToString(parts[4]);
                    name = "ko";
                    row.ko = CsvTypeHelper.ToString(parts[5]);
                    name = "fr";
                    row.fr = CsvTypeHelper.ToString(parts[6]);
                    name = "de";
                    row.de = CsvTypeHelper.ToString(parts[7]);
                    name = "ru";
                    row.ru = CsvTypeHelper.ToString(parts[8]);
                    name = "it";
                    row.it = CsvTypeHelper.ToString(parts[9]);
                    name = "pt";
                    row.pt = CsvTypeHelper.ToString(parts[10]);
                    name = "sp";
                    row.sp = CsvTypeHelper.ToString(parts[11]);
                    name = "sw";
                    row.sw = CsvTypeHelper.ToString(parts[12]);
                    name = "nl";
                    row.nl = CsvTypeHelper.ToString(parts[13]);
            
                    InitRowByUnid(""+row.ResId,row);
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
            return "Hotfix.Base.Module.Blaze.Locale$Locale.csv";
        }

        public virtual void ProcessRow(LocaleRow row)
        {
        }
    }

    public class LocaleRow
    {
        /// <summary>
        /// ???????????????
        /// </summary>
        public string ResId { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string en { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string zh { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string zh_tw { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string jp { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string ko { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string fr { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string de { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string ru { get; set; }

        /// <summary>
        /// ?????????
        /// </summary>
        public string it { get; set; }

        /// <summary>
        /// ?????????
        /// </summary>
        public string pt { get; set; }

        /// <summary>
        /// ?????????
        /// </summary>
        public string sp { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string sw { get; set; }

        /// <summary>
        /// ??????
        /// </summary>
        public string nl { get; set; }
    }

    public static partial class CsvHelper
    {
        private static LocaleCsv LocaleCsv ;


        public static LocaleCsv GetLocaleCsv()
        {
            return LocaleCsv ?? (LocaleCsv = new LocaleCsv());
        }
    }
}
