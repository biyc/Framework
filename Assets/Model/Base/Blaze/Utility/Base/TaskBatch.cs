using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Blaze.Utility.Base
{
    /// <summary>
    /// 异步任务顺序执行管理工具
    /// </summary>
    public class TaskBatch
    {
        // private bool isRunning;


        //9、任务队列
        private Queue<TaskInfo> m_TaskQueue = new Queue<TaskInfo>();

        // 最大并发执行
        private int maxExeNum = 10;

        // 当前并发执行
        private int curExeNum = 0;

        // 并发间隔毫秒
        private int interval = 5;


        //1、添加任务
        public void AddTask(TaskInfo taskInfo)
        {
            m_TaskQueue.Enqueue(taskInfo);
        }

        public void AddTask(Action<Action> work)
        {
            TaskInfo taskInfo = new TaskInfo(work);
            m_TaskQueue.Enqueue(taskInfo);
        }

        //2、开始任务
        public void Start()
        {
            // if (isRunning) return;
            // isRunning = true;

            NextTask();
        }

        public void Stop(Action endCb = null)
        {
            OnFinish = endCb;
            Clear();
            // isRunning = false;
            NextTask();
        }

        //3、清空任务
        public void Clear()
        {
            m_TaskQueue.Clear();
        }


        //5、完成所有任务回调
        public Action OnFinish = null;

        //6、下一个任务
        private void NextTask()
        {
            if (m_TaskQueue.Count > 0)
            {
                // 有执行空位，开始执行
                if (maxExeNum >= curExeNum)
                {
                    bool isExecCallBack = false;
                    TaskInfo taskInfo = m_TaskQueue.Dequeue();
                    curExeNum++;

                    Observable.Timer(new TimeSpan(TimeSpan.TicksPerMillisecond * interval)).Subscribe(delegate(long l)
                    // Observable.NextFrame().Subscribe(delegate(Unit unit)
                    {
                        taskInfo.Work(delegate
                        {
                            // 只执行一次 next ,防止多次执行 next 造成的错误
                            if (!isExecCallBack)
                            {
                                isExecCallBack = true;
                                curExeNum--;
                                // 检查新任务进入队列
                                NextTask();
                            }
                        });
                    });

                    if (maxExeNum >= curExeNum && m_TaskQueue.Count > 0)
                        NextTask();
                }
            }
            else
            {
                // isRunning = false;
                OnFinish?.Invoke();
            }
        }


        // 使用方法
        void Demo()
        {
            TaskBatch taskQueue = new TaskBatch();
            taskQueue.AddTask(delegate(Action endCb)
            {
                endCb?.Invoke();
            });
            taskQueue.Start();

        }
    }
}