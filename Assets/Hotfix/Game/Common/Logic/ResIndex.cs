using System;
using System.Collections.Generic;
using Blaze.Core;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Csv.Poco;
using Blaze.Manage.Data;
using Hotfix.Game.Common.Data;
using UnityEngine;

namespace Hotfix.Game.Common.Logic
{
    /// <summary>
    /// 游戏资源索引器，通过资源id 找到对应的名称与图标
    /// </summary>
    public class ResIndex : Singeton<ResIndex>
    {
        /// <summary>
        /// 执行奖励成功通知
        /// </summary>
        public IDataWatch<ResInfo> WatchReward = new DataWatch<ResInfo>();

        /// 执行奖励
        public bool Reward(List<ResInfo> info, AnalysisParam analysis = default)
        {
            info.ForEach(delegate(ResInfo resInfo)
            {
                if (resInfo.Analysis == default)
                    resInfo.Analysis = analysis;
                Reward(resInfo);
            });
            return true;
        }


        /// <summary>
        /// 执行奖励
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Reward(ResInfo info)
        {
            bool result = true;

            switch (info.GetTypeName())
            {
                case ResType.Furniture:
                    break;
                default:
                    // 花瓣等数据
                    LGraph._.AddRes(info.Type, info.Num, analysis: info.Analysis);
                    break;
            }

            return result;
        }


        /// <summary>
        /// 消耗某种资源
        /// </summary>
        /// <param name="resInfo"></param>
        /// <param name="endCb"></param>
        public void CostRes(ResInfo resInfo, Action<StatusData> endCb)
        {
            switch (resInfo.GetTypeName())
            {
                case ResType.EnergyLimit:
                    // 体力
                    // var status = PhysicalPoint._.Point.Custom((int) resInfo.Num);
                    // if (status)
                    // {
                    //     endCb.Invoke(StatusData.Success);
                    // }
                    // else
                    // {
                    //     var info = StatusData.ResNotEnough;
                    //     info.Desc = "体力不足";
                    //     endCb.Invoke(info);
                    // }

                    break;
                default:
                    // 花瓣等数据
                    LGraph._.CostRes(resInfo, endCb, resInfo.Analysis);
                    break;
            }
        }
    }
}