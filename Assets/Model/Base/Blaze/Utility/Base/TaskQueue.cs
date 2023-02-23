using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blaze.Utility.Base
{
    public class TaskInfo
    {
        //任务名
        private string m_TaskName;

        public string TaskName
        {
            set { m_TaskName = value; }
            get { return m_TaskName; }
        }

        //任务具体内容，外部传入
        public Action<Action> Work;

        public TaskInfo(Action<Action> work, string taskName = "defaultTaskName")
        {
            this.Work = work;
            this.m_TaskName = taskName;
        }
    }

    /// <summary>
    /// 异步任务顺序执行管理工具
    /// </summary>
    public class TaskQueue
    {
        private bool isRunning;

        //构造函数
        public TaskQueue()
        {
            m_TaskQueue = new Queue<TaskInfo>();
            m_TasksNum = 0;
        }

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
            if(isRunning) return;
            isRunning = true;
            //获取任务队列的总任务数
            m_TasksNum = m_TaskQueue.Count;

            OnStart?.Invoke();

            NextTask();
        }

        public void Stop(Action endCb = null)
        {
            OnFinish = endCb;
            Clear();
            NextTask();
        }

        //3、清空任务
        public void Clear()
        {
            OnFinish = null;
            m_TaskQueue.Clear();
            m_TasksNum = 0;
        }

        //4、开始任务回调
        public Action OnStart = null;

        //5、完成所有任务回调
        public Action OnFinish = null;

        //6、下一个任务
        private void NextTask()
        {
            if (m_TaskQueue.Count > 0)
            {
                bool isExecCallBack = false;
                TaskInfo taskInfo = m_TaskQueue.Dequeue();
                taskInfo.Work(delegate
                {
                    // 只执行一次 next ,防止多次执行 next 造成的错误
                    if (!isExecCallBack)
                    {
                        isExecCallBack = true;
                        NextTask();
                    }
                });
            }
            else
            {
                isRunning = false;

                OnFinish?.Invoke();
            }
        }

        //7、当前任务进度
        public float TaskProcess
        {
            get { return 1 - m_TaskQueue.Count * 1.0f / m_TasksNum; }
        }

        //8、任务队列总任务量
        private int m_TasksNum = 0;

        //9、任务队列
        private Queue<TaskInfo> m_TaskQueue;


        // 使用方法
        void Demo()
        {
            TaskQueue taskQueue = new TaskQueue();
            taskQueue.OnStart = () => { Debug.Log("OnStart"); };
            taskQueue.OnFinish = () => { Debug.Log("OnFinish"); };
            taskQueue.AddTask(delegate(Action scb)
            {
                // task1
                scb();
            });
            taskQueue.AddTask(delegate(Action scb)
            {
                // task2
                scb();
            });
            taskQueue.Start();
        }
    }
}