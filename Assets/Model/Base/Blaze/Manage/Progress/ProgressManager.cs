using System;
using Blaze.Core;

namespace Blaze.Manage.Progress
{
    public enum ProgressPoint
    {
        // 游戏开始
        Start = 0,

        // 资源版本与完整性检查
        CheckVersion = 50,

        // 下载AB包
        DownLoadAb = 80,

        // 用户登录
        Login = 85,

        // 启动游戏逻辑
        StartHotfix = 90,

        // 完成启动
        Finish = 100,
    }

    public class ProgressManager
    {
        #region Singleton

        private static readonly ProgressManager _instance = new ProgressManager();

        public static ProgressManager _ => _instance;

        #endregion


        // 当前已完成阶段
        // private ProgressPoint _curPoint = ProgressPoint.Start;
        // private float _curProgress = 0;

        // 回调函数
        // private Action<float> _finishCb;


        // 通知进度(真正进行的任务，总进度，已进行进度)
        // public Action<ProgressPoint, float, float> OnProgress;

        // 子任务进度条
        public Action<ProgressPoint, float, float, string> OnSubProgress;

        // 通知开始某一个子周期
        public Action<ProgressPoint> OnStart;
        // 通知结束某一个子周期
        public Action<ProgressPoint> OnEnd;


        // /// <summary>
        // /// 只在加载完成后通知
        // /// </summary>
        // public event Action<float> OnCompleted
        // {
        //     add
        //     {
        //         if ((int) _curProgress == (int) ProgressPoint.Finish)
        //             value(_curProgress);
        //         else
        //             _finishCb += value;
        //     }
        //     remove { _finishCb -= value; }
        // }

        /// <summary>
        /// 报告完成点
        /// </summary>
        /// <param name="point"></param>
        public void ReportStart(ProgressPoint point)
        {
            OnStart?.Invoke(point);
        }


        /// <summary>
        /// 报告完成点
        /// </summary>
        /// <param name="point"></param>
        public void ReportFinish(ProgressPoint point)
        {
            // _curPoint = point;
            // _curProgress = (float) point;
            OnEnd?.Invoke(point);
            // 通知进度条
            // OnProgress?.Invoke(point, (float) ProgressPoint.Finish, _curProgress);
            // 如果完成，发送完成事件
            // if ((int) _curProgress == (int) ProgressPoint.Finish)
            // {
            //     _finishCb?.Invoke(_curProgress);
            //     _finishCb = null;
            // }
        }

        //
        // /// <summary>
        // /// 报告小阶段
        // /// </summary>
        // /// <param name="target">目标阶段</param>
        // /// <param name="total">本阶段总量</param>
        // /// <param name="down">本阶段已完成量</param>
        // public void ReportSub(ProgressPoint target, float total, float down, string text = "")
        // {
        //     // 当前阶段到目标阶段的长度
        //     var dt = target - _curPoint;
        //     // 小段内已完成进度
        //     var dtSuccess = dt * (down / total);
        //     var totalSuccess = (float) _curPoint + dtSuccess;
        //     // 通知进度条
        //     OnProgress?.Invoke(target, (float) ProgressPoint.Finish, totalSuccess);
        //     OnSubProgress?.Invoke(target, total, down, text);
        // }


        /// <summary>
        /// 报告小阶段
        /// </summary>
        /// <param name="target">目标阶段</param>
        /// <param name="total">本阶段总量</param>
        /// <param name="down">本阶段已完成量</param>
        public void ReportSub(ProgressPoint target, long total, long down, string text = "")
        {
            // 处理 除0 异常
            if (total <= 0) return;
            // 当前阶段到目标阶段的长度
            // var dt = target - _curPoint;
            // 小段内已完成进度
            // var dtSuccess = dt * (down / (double) total);
            // var totalSuccess = (float) _curPoint + dtSuccess;
            // 通知进度条
            // OnProgress?.Invoke(target, (float) ProgressPoint.Finish, (float) totalSuccess);
            OnSubProgress?.Invoke(target, total, down, text);
        }
    }
}