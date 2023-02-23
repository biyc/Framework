using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("Music")]
    public class MusicCsv : CsvBaseTable<MusicRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new MusicRow();
                    name = "MusicID";
                    row.MusicID = CsvTypeHelper.ToInt(parts[0]);
                    name = "Name";
                    row.Name = CsvTypeHelper.ToString(parts[1]);
            
                    InitRowByUnid(""+row.MusicID,row);
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
            return "Hotfix.Game.Common$Music.csv";
        }

        public virtual void ProcessRow(MusicRow row)
        {
        }
    }

    public class MusicRow
    {
        /// <summary>
        /// 音乐ID
        /// </summary>
        public int MusicID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    public static partial class CsvHelper
    {
        private static MusicCsv MusicCsv ;


        public static MusicCsv GetMusicCsv()
        {
            return MusicCsv ?? (MusicCsv = new MusicCsv());
        }
    }
}
