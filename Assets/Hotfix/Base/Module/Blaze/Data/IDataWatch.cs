using System;

namespace Blaze.Manage.Data
{
    public interface IDataWatch<T>
    {

        /// <summary>
        /// 添加观察   只在数据变化时调用
        /// </summary>
        event Action<T> OnIneresting;

        /// <summary>
        /// 添加观察  第一次通知  数据变化时调用
        /// </summary>
        event Action<T> OnMessage;


        /// <summary>
        /// 通知数据观察者数据变化
        /// </summary>
        void ChangeNotify();

        /// <summary>
        /// 通知数据观察者数据变化，并刷新数据
        /// </summary>
        void ChangeNotify(T data);

        void CleanWatch();

    }
}