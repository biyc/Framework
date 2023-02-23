using System;
using Blaze.Manage.Data;
using Blaze.Utility.Helper;
using UniRx;

namespace Blaze.Utility.Base
{
    /// <summary>
    /// 点数计时器
    /// </summary>
    public class PointMeter
    {
        // 最后更新时间(毫秒)
        public long LastTimer;

        // 总点数(自动恢复点数上限)
        public int TotalPoint;

        // 当前点数
        public int CurPoint;

        // 恢复时间间隔(毫秒)
        private long _recoveryInterval;

        // 每个间隔恢复的点数
        private int _recoveryPoint;

        // 每次消费点数
        private int _customPoint;

        // 点数改变通知
        public DataWatch<PointMeter> PointChange = new DataWatch<PointMeter>();

        // 检查任务
        private IDisposable _task;

        private PointMeter()
        {
        }

        /// <summary>
        /// 创建点数计时器
        /// </summary>
        /// <param name="lastTimer">最后更新时间(毫秒)</param>
        /// <param name="totalPoint">总点数(点数上限)</param>
        /// <param name="curPoint">当前点数</param>
        /// <param name="recoveryInterval">恢复时间间隔(毫秒)</param>
        /// <param name="recoveryPoint">每个间隔恢复的点数</param>
        /// <param name="customPoint">每次消费点数</param>
        public PointMeter(long lastTimer, int totalPoint, int curPoint, long recoveryInterval, int recoveryPoint,
            int customPoint)
        {
            LastTimer = lastTimer;
            TotalPoint = totalPoint;
            CurPoint = curPoint;
            _recoveryInterval = recoveryInterval;
            _recoveryPoint = recoveryPoint;
            _customPoint = customPoint;

            if (LastTimer == 0)
            {
                // 全新的存档，充满点数
                LastTimer = TimeHelper.ClientNow();
                CurPoint = TotalPoint;
            }

            PointChange.ChangeNotify(this);
        }

        /// <summary>
        /// 检查是否可以消费
        /// </summary>
        /// <returns></returns>
        public bool CheckAbleCustom()
        {
            // 检查点数是否超过上限
            // if (CurPoint > TotalPoint)
            // CurPoint = TotalPoint;

            return CurPoint >= _customPoint;
        }

        /// <summary>
        /// 消费点数
        /// </summary>
        /// <returns></returns>
        public bool Custom()
        {
            return Custom(_customPoint);
        }

        /// <summary>
        /// 消费点数
        /// </summary>
        /// <param name="customPoint"></param>
        /// <returns></returns>
        public bool Custom(int customPoint)
        {
            // 检查点数是否超过上限
            // if (CurPoint > TotalPoint)
            // CurPoint = TotalPoint;

            // 更新倒计时恢复时间点
            if (CurPoint >= TotalPoint)
                LastTimer = TimeHelper.ClientNow();

            if (CurPoint >= customPoint)
            {
                CurPoint = CurPoint - customPoint;
                PointChange.ChangeNotify(this);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查恢复
        /// </summary>
        /// <returns></returns>
        private void Recover()
        {
            // 点数不满
            if (CurPoint < TotalPoint)
            {
                var curTime = TimeHelper.ClientNow();
                // 间隔数量
                var dt = (curTime - LastTimer) / _recoveryInterval;
                if (dt > 0)
                {
                    // 本次恢复的总点数
                    var recoveryPoint = (int) dt * _recoveryPoint;
                    CurPoint += recoveryPoint;
                    // 检查点数是否超过上限
                    if (CurPoint > TotalPoint)
                        CurPoint = TotalPoint;
                    // 移动结算时间点
                    LastTimer += dt * _recoveryInterval;
                    // 通知存档
                    PointChange.ChangeNotify(this);
                }
            }
        }

        /// <summary>
        /// 开发期直接恢复所有点数
        /// </summary>
        public void RecoverAll()
        {
            CurPoint = TotalPoint;
            PointChange.ChangeNotify(this);
        }

        /// <summary>
        /// 增加体力
        /// </summary>
        public void Add(int num)
        {
            // 处理异常值
            if (num < 0) return;
            CurPoint += num;
            // if (CurPoint > TotalPoint)
            // CurPoint = TotalPoint;
            PointChange.ChangeNotify(this);
        }

        /// <summary>
        /// 开始自动恢复任务
        /// </summary>
        public void Start()
        {
            Recover();
            var curTime = TimeHelper.ClientNow();
            var dt = curTime - LastTimer;
            if (dt < _recoveryInterval)
            {
                // 将公式对其到整数结算时间
                Observable.Timer(new TimeSpan(TimeSpan.TicksPerMillisecond * (_recoveryInterval - dt) + 50)).Subscribe(
                    delegate(long ll)
                    {
                        Recover();
                        _task?.Dispose();
                        _task = Observable.Interval(new TimeSpan(TimeSpan.TicksPerMillisecond * _recoveryInterval))
                            .Subscribe(delegate(long l) { Recover(); });
                    });
            }
            else
            {
                _task?.Dispose();
                _task = Observable.Interval(new TimeSpan(TimeSpan.TicksPerMillisecond * _recoveryInterval))
                    .Subscribe(delegate(long l) { Recover(); });
            }
        }

        /// <summary>
        /// 停止自动恢复任务
        /// </summary>
        public void Stop()
        {
            _task?.Dispose();
        }

        /// <summary>
        /// 下一个恢复点在多少秒之后
        /// </summary>
        /// <returns></returns>
        public long GetNextRecoverTime()
        {
            var curTime = TimeHelper.ClientNow();
            var dt = curTime - LastTimer;
            if (dt < _recoveryInterval)
            {
                return (_recoveryInterval - dt) / 1000;
            }
            else
            {
                return _recoveryInterval / 1000;
            }
        }


        /// <summary>
        /// 当前是否已经恢复到最大值
        /// </summary>
        /// <returns></returns>
        public bool IsMax()
        {
            if (TotalPoint <= CurPoint)
                return true;
            return false;
        }
    }
}