//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Blaze.Resource.Task
{
    /// <summary>
    /// 任务管理器,来做在Mono运行期的任务
    /// </summary>
    public class MonoScheduler : MonoBehaviour
    {
        /// <summary>
        /// 核心任务管理器命名
        /// </summary>
        public static string MonoObjectName = "* MonoScheduler *";

        /// <summary>
        /// 主线程Id
        /// </summary>
        private static int mainThreadId;

        /// <summary>
        /// 句柄
        /// </summary>
        private static MonoScheduler _;

        /// <summary>
        /// 初始化任务管理器
        /// </summary>
        public static void Initialize()
        {
            if (_ == null)
            {
                mainThreadId = Thread.CurrentThread.ManagedThreadId;
                var go = new GameObject(MonoObjectName);
                _ = go.AddComponent<MonoScheduler>();
                go.hideFlags = HideFlags.HideInHierarchy;
                _.StartCoroutine(_Update());
                DontDestroyOnLoad(_);
            }
        }

        /// <summary>
        /// 任务列表
        /// </summary>
        private static List<Action> _actions = new List<Action>();


        /// <summary>
        /// 协程循环,永不停歇
        /// </summary>
        /// <returns></returns>
        private static IEnumerator _Update()
        {
            while (true)
            {
                lock (_actions)
                {
                    for (int i = 0, size = _actions.Count; i < size; i++)
                    {
                        _actions[i]();
                    }

                    _actions.Clear();
                }

                yield return null;
            }
        }

        /// <summary>
        /// Coroutine 运行期执行器
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        public static Coroutine DispatchCoroutine(IEnumerator co)
        {
            return _.StartCoroutine(co);
        }

        /// <summary>
        /// 停止正在运行中的 Coroutine
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        public static void Stop(IEnumerator co)
        {
            _.StopCoroutine(co);
        }


        /// <summary>
        /// 延迟处理器
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        // main thread only
        public static void DispatchAfter(Action action, float seconds)
        {
            _.StartCoroutine(_AfterSeconds(action, seconds));
        }

        public static void DispatchAfter(float seconds, Action action)
        {
            _.StartCoroutine(_AfterSeconds(action, seconds));
        }

        private static IEnumerator _AfterSeconds(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }

        /// <summary>
        /// 任意线程执行器
        /// </summary>
        /// <param name="action"></param>
        public static void DispatchMainAnyway(Action action)
        {
            // Debug.Assert(_mb != null);
            lock (_actions)
            {
                _actions.Add(action);
            }
        }


        /// <summary>
        /// 主线程执行器
        /// </summary>
        /// <param name="action"></param>
        public static void DispatchMain(Action action)
        {
            // Debug.Assert(_mb != null);
            // 当前为主线程直接执行
            if (mainThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                action();
                return;
            }

            // 当前不是主线程则放入主线程执行器中
            lock (_actions)
            {
                _actions.Add(action);
            }
        }


        /// <summary>
        /// 循环执行任务列表
        /// </summary>
        private static List<Action> _updateActions = new List<Action>();

        /// <summary>
        /// 秒循环任务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private static IEnumerator _UpdateSeconds(Action action, float seconds)
        {
            while (true)
            {
                if (_updateActions.Contains(action))
                {
                    action();
                    yield return new WaitForSeconds(seconds);
                }
                else
                {
                    // 退出执行
                    break;
                }
            }

            yield return null;
        }

        /// <summary>
        /// 指定秒数循环执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        // main thread only
        public static void DispatchUpdateSeconds(Action action, float seconds)
        {
            Initialize();
            _updateActions.Add(action);
            _.StartCoroutine(_UpdateSeconds(action, seconds));
        }



    }
}