using System;
using System.Collections.Generic;
using Blaze.Core;
using Blaze.Utility;
using Game.Sdk;

namespace Model.Sdk.Statistics
{
    /// <summary>
    /// 打点记录
    /// </summary>
    public class Statistic : Singeton<Statistic>
    {
        // 上报数据方法
        // Statistic._.Report(AnPointModel.guide_state,StatisticParam.Succeed.ToString());

        public void Report(AnPointModel point, string param)
        {
            var pointName = point.ToString().ToLower();
            var eventName = "";
            switch (point)
            {
                case AnPointModel.enroll_method:
                case AnPointModel.enroll_SDK_post:
                case AnPointModel.enroll_SDK_get:
                case AnPointModel.enroll_state_register:
                    eventName = "register";
                    break;
                default:
                    eventName = "start_game";
                    break;
            }

            try
            {
                SdkManager.SdkWatch.OnCompleted += delegate(ISDK sdk)
                {
                    var args = new Dictionary<string, string>();
                    args.Add(pointName, param);
                    sdk?.CustomEvent(eventName, args);
                };
            }
            catch (Exception e)
            {
                Tuner.Error("埋点上报出错：" + e.StackTrace);
            }
        }
    }
}