using System;
using System.Collections.Generic;
using Blaze.Common;
using Blaze.Manage.Data;
using Blaze.Manage.Progress;
using Blaze.Manage.Safety;
using Blaze.Resource;
using Blaze.Resource.Common;
using Blaze.Resource.Task;
using Blaze.Utility;
using Blaze.Utility.Base;
using Blaze.Utility.Helper;
using Game.Sdk;
using Model.Base.Blaze.Manage.Archive;
using UniRx;
using UniRx.WebRequest;
using UnityEngine;

namespace ETModel
{
    public class Init : MonoBehaviour
    {
        [Tooltip("是否为开发模式")] public bool isDev;

        [Tooltip("使用ab")] public bool UseAB;


        public static ICompleted<Init> InitOnLoad = new DataWatch<Init>();
        public static ICompleted<GameSettings> SettingOnLoad = new DataWatch<GameSettings>();

        public void Start()
        {
            InitOnLoad.Complet(this);
            StartGame();
        }

        // public async void Start()
        /// <summary>
        /// 开始执行游戏逻辑启动
        /// </summary>
        public async void StartGame()
        {
            Application.focusChanged += OnFocusChanged;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //禁用多点触控
            // Input.multiTouchEnabled = false;
            Application.targetFrameRate = 60;
            Define.deviceId = SystemInfo.deviceUniqueIdentifier;

            // 开发期可能为空，走默认数据
            if (Define.GameSettings == null)
            {
                Define.GameSettings = new GameSettings();
                Define.GameSettings.ResServerList = new List<string>();
            }

            SettingOnLoad.Complet(Define.GameSettings);

            Define.UseAB = UseAB;
            Define.IsDev = isDev;
            // 开发模式时打开日志
            Debug.unityLogger.logEnabled = isDev;
            DontDestroyOnLoad(gameObject);
            try
            {
                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);
                //Game.Scene.AddComponent<AudioComponent>();
                //Game.Scene.AddComponent<StartUIComponent>();
                Game.Scene.AddComponent<BlazeComponent>();
                //SdkManager._.Init();

                TaskQueue taskQueue = new TaskQueue();
                taskQueue.OnFinish = () => { Debug.Log("母包启动完成"); };

                taskQueue.AddTask(delegate(Action scb) { MonoScheduler.DispatchMain(scb); });
                // 第一步资源检查并下载
                taskQueue.AddTask(delegate(Action scb)
                {
                    ResManager._.WatchProvidComplete.OnCompleted += delegate(IAssetProvider provider) { scb(); };
                });
                // 第三步，启动热更新代码
                taskQueue.AddTask(delegate(Action scb)
                {
                    // 存档与资源都准备好后，启动业务逻辑
                    LoadHotfix();
                    scb();
                });
                taskQueue.Start();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        protected virtual void LoadHotfix()
        {
            ProgressManager._.ReportStart(ProgressPoint.StartHotfix);
            // 通过 ILRuntime 启动 APP
            Game.Hotfix?.Dispose();
            Game.Hotfix = new Hotfix();
            Game.Hotfix.LoadHotfixAssembly();
        }

        private void Update()
        {
            Game.Hotfix?.Update?.Invoke();
            Game.EventSystem.Update();
        }

        private void OnGUI()
        {
            Game.Hotfix?.OnGUI?.Invoke();
        }

        private void LateUpdate()
        {
            Game.Hotfix?.LateUpdate?.Invoke();
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            ArchiveManager._.PushSlot();
            Game.Hotfix?.OnApplicationQuit?.Invoke();
            Game.Close();
        }

        private void OnApplicationFocus(bool focus)
        {
            // Debug.Log("OnApplicationFocus:  " + focus);
            Game.Hotfix?.OnApplicationFocus?.Invoke(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            // Debug.Log("OnApplicationPause:  " + pause);
            Game.Hotfix?.OnApplicationPause?.Invoke(pause);
        }

        private void OnFocusChanged(bool changed)
        {
            Debug.Log("OnFocusChanged:  " + changed);
            if (changed)
            {
                // 退出后台
                // 重新同步服务器时间
                NetTime._.Expired();
                NetTime._.Sync();
            }
            else
            {
                // 进入后台
                // 设置之前同步的服务器时间作废
                NetTime._.Expired();
            }

            Game.Hotfix?.OnFocusChanged?.Invoke(changed);
        }
    }
}