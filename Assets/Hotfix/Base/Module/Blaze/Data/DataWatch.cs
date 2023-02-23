//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/24 | Initialize core skeleton |
*/

using System;
using System.Collections.Generic;
using Blaze.Utility;

namespace Blaze.Manage.Data
{
    /// <summary>
    /// 数据通知
    /// </summary>
    public class DataWatch<T> : DataObj<T>, ICompleted<T>, IDataWatch<T>
    {
        protected Action<T> _callbacks;

        // 是否加载
        protected bool _loaded;

        // 回调函数
        protected Action<T> _completCb;


        /// <summary>
        /// 添加兴趣者 第一次不通知数据，只在变化时调用
        /// </summary>
        public event Action<T> OnIneresting
        {
            add { AddCb(value); }

            remove { _callbacks -= value; }
        }

        /// <summary>
        /// 添加兴趣者,第一次就通知数据
        /// </summary>
        public event Action<T> OnMessage
        {
            add
            {
                if (_data != null)
                {
                    value(_data);
                }

                AddCb(value);
                // _callbacks += value;
            }

            remove { _callbacks -= value; }
        }


        /// <summary>
        /// 只在加载完成后通知一次
        /// </summary>
        public event Action<T> OnCompleted
        {
            add
            {
                if (_loaded)
                    value(_data);
                else
                    _completCb += value;
            }
            remove { _completCb -= value; }
        }


        /// <summary>
        /// 发生了事情
        /// </summary>
        public virtual void ChangeNotify()
        {
            try
            {
                _callbacks?.Invoke(_data);

                _loaded = true;
                _completCb?.Invoke(_data);
                _completCb = null;
            }
            catch (Exception e)
            {
                Tuner.Log($"[>>] {e}");
            }
        }

        /// <summary>
        /// 发生了事情
        /// </summary>
        public void ChangeNotify(T data)
        {
            Put(data);
            ChangeNotify();
        }

        /// <summary>
        /// 完成
        /// </summary>
        public virtual void Complet(T data)
        {
            ChangeNotify(data);
        }

        public bool IsLoad()
        {
            return _loaded;
        }

        public void Clean()
        {
            _loaded = false;
            _data = default;
        }

        public void CleanWatch()
        {
            _callbacks = null;
            _completCb = null;
        }


        /// <summary>
        /// 添加监听
        /// </summary>
        private void AddCb(Action<T> value)
        {
            // // 如果已经监听，不在重复监听
            // for (var i = 0; _callbacks != null && i < _callbacks.GetInvocationList().Length; i++)
            // {
            //     if (_callbacks.GetInvocationList()[i].Equals(value))
            //     {
            //         Tuner.Error("跳过添加重复的监听");
            //         return;
            //     }
            // }

            // 挂载监听
            _callbacks += value;
        }
    }
}