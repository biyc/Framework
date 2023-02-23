using System.Linq;
using Blaze.Core;
using Blaze.Manage.Csv.Poco;
using Blaze.Utility;
using Blaze.Utility.Extend;
using Blaze.Utility.Helper;
using Game.Common.Slots;
using Hotfix.Game.Common.Logic;
using Model.Base.Blaze.Manage.Archive;

namespace Hotfix.Game.Dev
{
    public class SlotGen : Singeton<SlotGen>
    {
        // 存档生成
        public void UnlockAll()
        {
            // ArchiveGen
            CsvHelper.GetArchiveGenCsv().GetTable().ForEach(delegate(ArchiveGenRow row)
            {

                SlotCommon._.ReadSlot().IsSuccessGuidChapter = true;
                // 体力
                SlotCommon._.ReadSlot().PhysicalLastTime = TimeHelper.ClientNow();
                SlotCommon._.ReadSlot().PhysicalCurPoint = row.Phy;
                SlotCommon._.Save();


                switch (row.Type)
                {
                    case 0:
                        // 空白账户不跳过
                        SlotCommon._.ReadSlot().IsSuccessGuidChapter = false;
                        SlotCommon._.Save();
                        // 空白
                        break;
                    case 1:
                        // 低级
                        var items =
                            "10001|10002|10003|10004|10005|10006|10007|10008|10009|10010|10011|10012|10013|10014|10015|10016|10017|10018|10019|10020|10021|10022|10023|10024|10025|10026|10027|10028|10029|10030|10031|10032|10033|10034|10035|10036|10037|10038|10039|10040|10041|10042|10043|10044|10045|10046|10047|10048|10049|10050|10051|10052|10053|10054|10055|10056|10057|10058|10059|10060|10061|10062|10063|10064|10065|10066|10067";
                        var itemsId = items.Split('|').ToList().ConvertAll(delegate(string input)
                        {
                            return input.ToInt();
                        });
                        break;
                    case 2:
                        // 中级
           
                        var itemsM =
                            "10001|10002|10003|10004|10005|10006|10007|10008|10009|10010|10011|10012|10013|10014|10015|10016|10017|10018|10019|10020|10021|10022|10023|10024|10025|10026|10027|10028|10029|10030|10031|10032|10033|10034|10035|10036|10037|10038|10039|10040|10041|10042|10043|10044|10045|10046|10047|10048|10049|10050|10051|10052|10053|10054|10055|10056|10057|10058|10059|10060|10061|10062|10063|10064|10065|10066|10067|10068|10069|10070|10071|10072|10073|10074|10075|10076|10077|10078|10079|10080|10081|10082|10083|10084|10085|10086|10087|10088|10089|10090|10091|10092|10093|10094|10095|10096|10097|10098|10099|10100|10101|10102|10103|10104|10105|10106|10107|10108|10109|10110|10111|10112|10113|10114|10115|10116|10117|10118|10119|10120|10121|10122|10123|10124|10125|10126|10127|10128|10129|10130|10131|10132|10133|10134|10135|10136|10137";
                        var itemsMId = itemsM.Split('|').ToList().ConvertAll(delegate(string input)
                        {
                            return input.ToInt();
                        });
     
                        break;
                    case 3:
                        // 高级
                        break;
                }

                // 100001	Petals	花瓣
                LGraph._.WriteRes(100001, row.Coin);

                // 保存存档
                var slot = ArchiveManager._.GetArchive().Save(row.Name);
                slot.CreateDate = TimeHelper.ClientNow();
                // 上传存档
                ArchiveManager._.PushSlot(slot, delegate(bool b) { Tuner.Log("上传存档:" + b); });
            });
        }
    }
}