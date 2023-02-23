using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Blaze.Utility;
using Blaze.Manage.Csv.Enum;

namespace Blaze.Manage.Csv.Poco
{
    [CsvTable("Mail")]
    public class MailCsv : CsvBaseTable<MailRow>
    {
        protected override void Load(List<string> list)
        {
            int count = 0;
            string name = "";
            foreach (var item in list){
                try
                {
                    var parts = item.Split(',');
                    var row = new MailRow();
                    name = "MailID";
                    row.MailID = CsvTypeHelper.ToInt(parts[0]);
                    name = "MessageName";
                    row.MessageName = CsvTypeHelper.ToString(parts[1]);
                    name = "MessageTime";
                    row.MessageTime = CsvTypeHelper.ToString(parts[2]);
                    name = "MessageEndTime1";
                    row.MessageEndTime1 = CsvTypeHelper.ToString(parts[3]);
                    name = "MessageEndTime2";
                    row.MessageEndTime2 = CsvTypeHelper.ToString(parts[4]);
                    name = "MessageType";
                    row.MessageType = CsvTypeHelper.ToInt(parts[5]);
                    name = "MessageRewards";
                    row.MessageRewards = ResInfoList.Create(parts[6]);
                    name = "MessageTitle";
                    row.MessageTitle = CsvTypeHelper.ToString(parts[7]);
                    name = "MessageContent";
                    row.MessageContent = CsvTypeHelper.ToString(parts[8]);
            
            
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
            return "Hotfix.Game.Common$Mail.csv";
        }

        public virtual void ProcessRow(MailRow row)
        {
        }
    }

    public class MailRow
    {
        /// <summary>
        /// 邮件的对应ID
        /// </summary>
        public int MailID { get; set; }

        /// <summary>
        /// 邮件名称
        /// </summary>
        public string MessageName { get; set; }

        /// <summary>
        /// 发送邮件的时期
        /// </summary>
        public string MessageTime { get; set; }

        /// <summary>
        /// 邮件截止日期
        /// </summary>
        public string MessageEndTime1 { get; set; }

        /// <summary>
        /// 截止创档日期
        /// </summary>
        public string MessageEndTime2 { get; set; }

        /// <summary>
        /// 1为有奖励邮件，2为无奖励邮件
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 当邮件类型为1时填写，用“|”隔开
        /// </summary>
        public ResInfoList MessageRewards { get; set; }

        /// <summary>
        /// 填写邮件的标题，直接读取显示
        /// </summary>
        public string MessageTitle { get; set; }

        /// <summary>
        /// 填写邮件的文本，直接读取显示
        /// </summary>
        public string MessageContent { get; set; }
    }

    public static partial class CsvHelper
    {
        private static MailCsv MailCsv ;


        public static MailCsv GetMailCsv()
        {
            return MailCsv ?? (MailCsv = new MailCsv());
        }
    }
}
