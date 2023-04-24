using System;
using Blaze.Common;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Progress;
using Blaze.Utility;
using ETModel;
using Hotfix.Base.Module.Blaze.Audio;
using Hotfix.Game.Common;
using UnityEngine;

namespace ETHotfix
{
    public static class Init
    {
        public static async void Start()
        {
            Tuner.Info(Co._("           love:red:b;Sakura Hotfix Launched!:darkrad:b;love:red:b;"));
            Game.Scene.ModelScene = ETModel.Game.Scene;
            // 注册热更层回调
            ETModel.Game.Hotfix.Update += Update;
            ETModel.Game.Hotfix.OnGUI += OnGUI;
            ETModel.Game.Hotfix.LateUpdate += LateUpdate;
            ETModel.Game.Hotfix.OnApplicationQuit += OnApplicationQuit;
            ETModel.Game.Hotfix.OnApplicationFocus += OnApplicationFocus;
            ETModel.Game.Hotfix.OnApplicationPause += OnApplicationFocus;
            ETModel.Game.Hotfix.OnFocusChanged += OnFocusChanged;
            // 初始化热更端 blaze 框架
            // Game.Scene.AddComponent<BlazeHotfixComponent>();
            // 全局UI控制器
            Game.Scene.AddComponent<UIComponent>();
            // Game.Scene.AddComponent<CDComponent>();
            // 启动游戏逻辑
            CommonManager._.Initialize();

            //   是否打开开发信息
            if (Define.IsDev)
            {
                // 开启键盘测试
                Game.Scene.AddComponent<KeyboardTestCaseComponent>();
                // 开启FPS显示
                //await BUI.Create(UIFrameShowComponent.Args);
                // 开发期关闭CSV数据缓存
                DefaultDebug.DisableCsvDataCache = true;
            }

            //主页
            var home = await BUI.Create(HomePageComponent.Args);
            var isStart = false;
            home.Show();
            // StartUIComponent._.OnStartGame += delegate
            // {
            //     if (isStart) return;
            //     isStart = true;
            //     //播放背景音乐
            //     AudioManager._.Play(500004, true);
            //     home.Show();
            // };

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                UnityCallAndroid._.OnUnityInitialCompleteFun();

            ProgressManager._.ReportFinish(ProgressPoint.StartHotfix);
            //
            // // 启动完成，撤除loging
            ProgressManager._.ReportFinish(ProgressPoint.Finish);
            //
        }


        public static void Update()
        {
            try
            {
                Game.EventSystem.Update();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void OnGUI()
        {
            try
            {
                Game.EventSystem.OnGUI();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void LateUpdate()
        {
            try
            {
                Game.EventSystem.LateUpdate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void OnApplicationQuit()
        {
            Game.Close();
        }

        public static void OnApplicationFocus(bool focus)
        {
        }

        public static void OnApplicationPause(bool pause)
        {
        }

        private static void OnFocusChanged(bool changed)
        {
            //UnityEngine.Debug.Log($"OnApplicationChanged:{changed}");
            if (UnityEngine.Application.platform != RuntimePlatform.Android &&
                UnityEngine.Application.platform != RuntimePlatform.IPhonePlayer)
            {
                //UnityEngine.Debug.Log("不是目标平台不计算后台运行时间");
                return;
            }

            // if (changed)
            // {
            //     Game.Scene.GetComponent<CDComponent>().BackstageOut();
            // }
            // else
            // {
            //     Game.Scene.GetComponent<CDComponent>().BackstageIn();
            // }
        }
    }
}