using System;
using Blaze.Common;
using Blaze.Manage.Progress;
using Blaze.Utility.Base;
using Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class UIStartComponent : MonoBehaviour
    {
        private Slider _hotfixSlider;
        private Text _des;
        private Button _loginBtn;

        private void Start()
        {
            _hotfixSlider = transform.Find("HotSlider").GetComponent<Slider>();
            _des = transform.Find("Des").GetComponent<Text>();
            _loginBtn = transform.Find("Login").GetComponent<Button>();
            _loginBtn.gameObject.SetActive(false);
            _loginBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                LoginComponent._.Login(-1,"");
            });
            
            TaskQueue taskQueue = new TaskQueue();

            // 开始启动游戏逻辑
            taskQueue.AddTask(delegate(Action scb)
            {
                Init.InitOnLoad.OnCompleted += delegate(Init init)
                {
                    init.StartGame();
                    scb();
                };
            });
            taskQueue.Start();


            ProgressManager._.OnStart += delegate(ProgressPoint point)
            {
                switch (point)
                {
                    case ProgressPoint.CheckVersion:
                    case ProgressPoint.DownLoadAb:
                        break;
                    case ProgressPoint.Login:
                        _loginBtn.gameObject.SetActive(true);
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
                        _hotfixSlider.gameObject.SetActive(false);
                        _des.gameObject.SetActive(false);
                        break;
                    case ProgressPoint.Login:
                        break;
                    case ProgressPoint.StartHotfix:
                        break;
                    case ProgressPoint.Finish:
                        Observable.Timer(TimeSpan.FromSeconds(0.5f))
                            .Subscribe(_ => Destroy(gameObject));
                        break;
                }
            };
        }
    }
}