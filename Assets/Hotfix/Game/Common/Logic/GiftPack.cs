using System.Collections.Generic;
using Blaze.Core;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Csv.Poco;
using ETHotfix;

namespace Hotfix.Game.Common.Logic
{
    /// <summary>
    /// 礼包奖励
    /// </summary>
    public class GiftPack : Singeton<GiftPack>
    {
        /// <summary>
        /// 获取礼包名称
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        public string Name(int giftId)
        {
            var giftPackRow = CsvHelper.GetGiftPackCsv().GetRowByUnid(giftId);
            if (giftPackRow == null) return "";

            return giftPackRow.Name;
        }

        /// <summary>
        /// 执行礼包奖励
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        public bool Reward(int giftId)
        {
            // 查找奖励
            var giftPackRow = CsvHelper.GetGiftPackCsv().GetRowByUnid(giftId);
            if (giftPackRow == null) return false;

            // 设置这批奖励解锁时不分别提示
            giftPackRow.RewardValue.ResInfos.ForEach(delegate(ResInfo info) { info.IsTipUnlock = false; });
            // 执行奖励
            var status = ResIndex._.Reward(giftPackRow.RewardValue.ResInfos);
            if (!status) return false;
            

            return status;
        }


        /// <summary>
        /// 归集所有礼包中的礼物
        /// </summary>
        /// <param name="rewards"></param>
        /// <returns></returns>
        public List<ResInfo> Collection(List<ResInfo> rewards)
        {
            var coll = new List<ResInfo>();
            rewards.ForEach(delegate(ResInfo info)
            {
                if (info.Type == (long) ResType.GiftPack)
                {
                    var giftPackRow = CsvHelper.GetGiftPackCsv().GetRowByUnid((int) info.Num);
                    if (giftPackRow == null) return;
                    coll.AddRange(giftPackRow.RewardValue.ResInfos);
                }
                else
                {
                    coll.Add(info);
                }
            });

            return coll;
        }


        /// <summary>
        /// 将所有的奖品归集到一起统一奖励并弹出奖励弹窗
        /// </summary>
        /// <param name="rewards"></param>
        /// <returns></returns>
        public bool RewardCollection(List<ResInfo> rewards, AnalysisParam analysis = default)
        {
            var coll = Collection(rewards);
            // 设置这批奖励解锁时不分别提示
            coll.ForEach(delegate(ResInfo info) { info.IsTipUnlock = false; });
            // 执行奖励
            var status = ResIndex._.Reward(coll,analysis);
            if (!status) return false;
            

            return true;
        }
    }
}