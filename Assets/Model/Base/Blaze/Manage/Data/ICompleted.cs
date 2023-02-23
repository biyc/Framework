using System;

namespace Blaze.Manage.Data
{
    /// <summary>
    /// 加载完成回调
    /// </summary>
    public interface ICompleted<T>
    {
        /// <summary>
        /// 只在加载完成后通知一次
        /// </summary>
        event Action<T> OnCompleted;

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="data"></param>
        void Complet(T data);

        /// <summary>
        /// 获取数据
        /// </summary>
        T Get();

        /// <summary>
        /// 是否已加载
        /// </summary>
        /// <returns></returns>
        bool IsLoad();


    }
}