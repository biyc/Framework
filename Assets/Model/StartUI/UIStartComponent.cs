using System;
using Blaze.Common;
using Blaze.Manage.Progress;
using Blaze.Utility.Base;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class UIStartComponent : MonoBehaviour
    {
        private Slider _hotfixSlider;
        private Text _des;

        private void Start()
        {
            _hotfixSlider = transform.Find("HotSlider").GetComponent<Slider>();
            _des = transform.Find("Des").GetComponent<Text>();
            TaskQueue taskQueue = new TaskQueue();
            // // 闪烁 logo1
            // taskQueue.AddTask(delegate(Action scb)
            // {
            //     // 开始 logo 页面
            //     Statistic._.Report(AnPointModel.start_logo, StatisticParam.Succeed.ToString());
            //     scb();
            // });
            // // 开始启动游戏逻辑
            taskQueue.AddTask(delegate(Action scb)
            {
                Init.InitOnLoad.OnCompleted += delegate(Init init)
                {
                    init.StartGame();
                    scb();
                };
            });
            // taskQueue.AddTask(m =>
            // {
            //     StartUIComponent._.Start();
            //     m();
            // });
             taskQueue.Start();


            ProgressManager._.OnStart += delegate(ProgressPoint point)
            {
                switch (point)
                {
                    case ProgressPoint.CheckVersion:
                    case ProgressPoint.DownLoadAb:
                        break;
                    case ProgressPoint.Login:
                        break;
                    case ProgressPoint.StartHotfix:
                        break;
                }
            };


            // 正在为您下载更新资源包
            ProgressManager._.OnSubProgress +=
                delegate(ProgressPoint point, float total, float down, string txt)
                {
                    var per = down / total;
                    switch (point)
                    {
                        case ProgressPoint.CheckVersion:
                            _des.text = $"正在检测资源完整性 {Math.Round(per * 100)}%";
                            _hotfixSlider.value = per;
                            break;
                        case ProgressPoint.DownLoadAb:
                            _des.text = $"正在为您下载更新资源包 {Math.Round(per * 100)}%";
                            _hotfixSlider.value = per;
                            // Debug.LogError($"正在为您下载更新资源包 {Math.Round(per * 100)}%");
                            break;
                    }
                };


            // 小阶段完成后
            ProgressManager._.OnEnd += delegate(ProgressPoint point)
            {
                switch (point)
                {
                    case ProgressPoint.CheckVersion:
                    case ProgressPoint.DownLoadAb:
                        break;
                    case ProgressPoint.Login:
                        //  view.text_txt_wait.text = $"登录成功";
                        break;
                    case ProgressPoint.StartHotfix:
                        //  view.text_txt_wait.text = $"启动成功";
                        break;
                    case ProgressPoint.Finish:
                        // view.txt_a4_10.text = $"加载完成";
                        // 所有启动过程结束后，显示启动游戏大按钮
                        //  view.text_txt_wait.gameObject.SetActive(false);
                        // 显示 开始游戏 按钮
                        //   view.go_m_beginGame.SetActive(true);
                        //    Show(view.go_m_beginGame);
                        break;
                }
            };
        }
    }
}