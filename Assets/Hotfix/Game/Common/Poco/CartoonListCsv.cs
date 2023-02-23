using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("CartoonList")]
    public class CartoonListCsv : CsvBaseTable<CartoonListRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new CartoonListRow();
                    name = "SN";
                    row.SN = CsvTypeHelper.ToInt(parts[0]);
                    name = "Type";
                    row.Type = CsvTypeHelper.ToInt(parts[1]);
                    name = "CartoonAsset";
                    row.CartoonAsset = CsvTypeHelper.ToString(parts[2]);
                    name = "Synopsis";
                    row.Synopsis = CsvTypeHelper.ToString(parts[3]);
                    name = "Desc";
                    row.Desc = CsvTypeHelper.ToString(parts[4]);
                    name = "Value";
                    row.Value = CsvTypeHelper.ToInt(parts[5]);
            
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
            return "Hotfix.Game.Common$CartoonList.csv";
        }

        public virtual void ProcessRow(CartoonListRow row)
        {
        }
    }

    public class CartoonListRow
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SN { get; set; }

        /// <summary>
        /// 卡通类型0:使用编辑器器的剧本1：手写的剧本
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 资源文件名
        /// </summary>
        public string CartoonAsset { get; set; }

        /// <summary>
        /// 剧情简介
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        /// 触发描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 触发剧本编号
        /// </summary>
        public int Value { get; set; }
    }

    public static partial class CsvHelper
    {
        private static CartoonListCsv CartoonListCsv ;


        public static CartoonListCsv GetCartoonListCsv()
        {
            return CartoonListCsv ?? (CartoonListCsv = new CartoonListCsv());
        }
    }
}
