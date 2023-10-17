using System;
using Model.Base.Blaze.Manage.Archive;
using Sirenix.Utilities;

namespace Blaze.Utility.Base
{
    /// <summary>
    /// 通用存档操作窗口
    /// </summary>
    public class BaseSlotOperate<T> where T : class, new()
    {
        #region 单例

        private static readonly object locker = new object();
        private static BaseSlotOperate<T> instance;


        public static BaseSlotOperate<T> _
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new BaseSlotOperate<T>();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        // 存档内容
        protected T _slot;


        /// <summary>
        /// 读取存档
        /// </summary>
        /// <returns></returns>
        public T ReadSlot()
        {
            // 其他线程去主线程读取存档
            if (_slot == null)
            {
                try
                {
                    var data = ArchiveManager._.Archive.Get(SlotName());
                    // var data = Storage.Get(SlotName());
                    if (data.IsNullOrWhitespace())
                    {
                        _slot = CreateNewSlot();
                        Save();
                    }
                    else
                    {
                        _slot = LitJson.JsonMapper.ToObject<T>(data);
                    }
                }
                catch (Exception e)
                {
                    Tuner.Log(e.StackTrace);
                }
            }

            if (_slot == null)
            {
                _slot = CreateNewSlot();
                Save();
            }

            return _slot;
        }
        
        /// <summary>
        /// 写入存档文件
        /// </summary>
        public void Save()
        {
            try
            {
                ArchiveManager._.Archive.Save(SlotName(), LitJson.JsonMapper.ToJson(_slot));
            }
            catch (Exception e)
            {
                Tuner.Log(e.StackTrace);
            }
        }


        /// <summary>
        /// 写入存档
        /// </summary>
        /// <param name="slot"></param>
        public void WriteSlot(T slot)
        {
            _slot = slot;
            Save();
        }
        

        /// <summary>
        /// 存档名称
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string SlotName()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// 创建新存档
        /// </summary>
        /// <returns></returns>
        protected virtual T CreateNewSlot()
        {
            return new T();
        }
    }
}