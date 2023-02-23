using System;
using App;
using Blaze.Core;
using Blaze.Manage.Data;
using Blaze.Utility;
using UnityEngine;

namespace Game.Sdk
{
    /// <summary>
    /// SDK 接入
    /// </summary>
    public class SdkManager : Singeton<SdkManager>
    {

        public static readonly ICompleted<ISDK> SdkWatch = new DataWatch<ISDK>();


        /// <summary>
        /// 初始化模块
        /// </summary>
        public void Init()
        {
 
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void OnDestroy()
        {
        }


        public void AdRewardPlay(Action<bool, string> playEndCb)
        {
            if (Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                Tuner.Log("广告播放成功");
                playEndCb(true, "");
            }
            else
            {
                // if (GlobalConfig.IsPopupAd)
                // {
                //     SDK.OnCompleted += delegate(ISDK sdk) { sdk.AdRewardPlay(playEndCb); };
                // }
                // else
                // {
                //     Tuner.Log("广告播放成功");
                //     playEndCb?.Invoke(true, "");
                // }
            }
        }
    }
}