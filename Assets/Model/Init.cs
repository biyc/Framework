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

        public GameObject screenConsoleGo;


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
            // 开发模式打开命令窗
            screenConsoleGo.SetActive(isDev);
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
                // 检查网络中是否有强制下线信息
                // taskQueue.AddTask(delegate(Action scb)
                // {
                //     scb();
                //     return;
                //     // 其它渠道直接跳过
                //     // if (Define.GameSettings.Channel != "AndroidBusinessPreview")
                //     // {
                //     //     scb();
                //     //     return;
                //     // }
                //     //
                //     //
                //     // var curtime = TimeHelper.CurrentMillis();
                //     // // 超出指定日期，游戏直接退出
                //     // if (curtime > 1669305600000)
                //     // {
                //     //     // 2022-11-25 00:00:00
                //     //     // 1669305600000
                //     //     Tuner.Log("超出指定日期，退出应用");
                //     //     Application.Quit();
                //     //     return;
                //     // }
                //     //
                //     // ObservableWebRequest.Get("http://lovinhouse2.iyloft.com/Config/Start.conf")
                //     //     .Subscribe(delegate(string data)
                //     //     {
                //     //         var conf = SafetyStartData.LoadFromData(data);
                //     //         if (conf.IsOpen && curtime < conf.DeadlineTime)
                //     //         {
                //     //             // 可以进行下一步
                //     //             scb();
                //     //         }
                //     //         else
                //     //         {
                //     //             Observable.Timer(TimeSpan.FromSeconds(8)).Subscribe(_ => { Application.Quit(); });
                //     //             UpDatePop.Instans.OnCompleted += delegate(UpDatePop pop)
                //     //             {
                //     //                 pop.Message(delegate { Application.Quit(); },
                //     //                     delegate { Application.Quit(); });
                //     //                 pop.ShowVersionUpdate("应用已过期。");
                //     //             };
                //     //         }
                //     //     }, delegate(Exception exception)
                //     //     {
                //     //         Observable.Timer(TimeSpan.FromSeconds(8)).Subscribe(_ => { Application.Quit(); });
                //     //         UpDatePop.Instans.OnCompleted += delegate(UpDatePop pop)
                //     //         {
                //     //             pop.Message(delegate { Application.Quit(); },
                //     //                 delegate { Application.Quit(); });
                //     //             pop.ShowVersionUpdate("应用验证失败。");
                //     //         };
                //     //     });
                // });

                // 初始化强更SDK
                taskQueue.AddTask(delegate(Action scb)
                {
                    MonoScheduler.DispatchMain(scb);
                });
                // 第一步资源检查并下载
                taskQueue.AddTask(delegate(Action scb)
                {
                    ResManager._.WatchProvidComplete.OnCompleted += delegate(IAssetProvider provider) { scb(); };
                });

                // 第二步，用户登录与存档更新
                // taskQueue.AddTask(delegate(Action scb)
                // {
                //     // 报告进度开始登录
                //    // ProgressManager._.ReportStart(ProgressPoint.Login);
                //     // 初始化存档
                //     //ArchiveManager._.Load("biyc");
                //
                //     // ArchiveManager._.OnLoad.OnCompleted += delegate(ArchiveData data)
                //     // {
                //     //     // 存档拉取完成，登录成功
                //     //     ProgressManager._.ReportFinish(ProgressPoint.Login);
                //     //     scb();
                //     // };
                // });
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