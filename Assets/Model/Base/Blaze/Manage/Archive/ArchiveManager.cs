using System;
using System.Text;
using System.Threading.Tasks;
using Blaze.Common;
using Blaze.Core;
using Blaze.Manage.Data;
using Blaze.Utility;
using ETModel;
using Newtonsoft.Json;
using UniRx;
using UniRx.WebRequest;

namespace Model.Base.Blaze.Manage.Archive
{
    /// <summary>
    /// 登录管理管理
    /// </summary>
    public class ArchiveManager
    {
        #region Singleton

        private static readonly ArchiveManager _instance = new ArchiveManager();

        public static ArchiveManager _ => _instance;

        #endregion

        public ICompleted<ArchiveData> OnLoad = new DataWatch<ArchiveData>();

        /// <summary>
        /// 当前存档
        /// </summary>
        public ArchiveData Archive;


        /// <summary>
        /// 根据存档名称加载存档
        /// </summary>
        /// <param name="userName"></param>
        public async void Load(string userName)
        {
            // 登录成功后，检查存档
            if (userName == null)
            {
                Tuner.Log("用户名为空");
                userName = "slot";
            }
            else
            {
                Tuner.Log("user: " + userName);
            }

            var local = ArchiveData.LoadLocal(userName);
            if (local == null)
            {
                // 全新用户
                Archive = ArchiveData.Create(userName);
                Archive.Save();
            }
            OnLoad.Complet(Archive);
        }

        /// <summary>
        /// 获取当前的存档数据
        /// </summary>
        /// <returns></returns>
        public ArchiveData GetArchive()
        {
            return Archive;
        }

    }
}