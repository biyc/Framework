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
            }
            else
            {
                Tuner.Log("user: " + userName);
            }

            var local = ArchiveData.LoadLocal(userName);
            // 默认存档不走网络（用于本地开发）
            if (userName == "slot")
            {
                if (local == null)
                    local = ArchiveData.Create(userName);
                Archive = local;
                OnLoad.Complet(Archive);
                return;
            }

            ArchiveData net = null;

            try
            {
                // 从服务器拉取存档
                var task = new TaskCompletionSource<byte[]>();
                ObservableWebRequest
                    .GetAndGetBytes($"{DefaultRuntime.SlotServerURI}/ReadSlot?ArchiveName={userName}")
                    .Subscribe(task.SetResult, task.SetException);
                var netSlot = await task.Task;
                net = ArchiveData.LoadFromOrigin(Encoding.UTF8.GetString(netSlot));
            }
            catch (Exception e)
            {
                Tuner.Log("网络获取存档失败");
            }


            if (local == null && net == null)
            {
                // 全新用户
                // 创建新用户
                Archive = ArchiveData.Create(userName);
                Archive.Save();
                PushSlot();
            }
            else if (local != null && net != null)
            {
                // 双端都有存档
                if (local.CreateDate == net.CreateDate)
                {
                    // 同一份存档，对比新旧
                    if (local.SlotVersion >= net.SlotVersion)
                    {
                        // 本地比较新推送到远程
                        Archive = local;
                        PushSlot();
                    }
                    else
                    {
                        // 远程比较新，保存到本地
                        Archive = net;
                        Archive.Save();
                    }
                }
                else
                {
                    try
                    {
                        // 不同存档，以服务器为准
                        // 备份原来的本地存档
                        local.Save(local.ArchiveName + "_" + local.CreateDate + "_bk");
                        Archive = net;
                        Archive.Save();
                    }
                    catch (Exception e)
                    {
                        Tuner.Log("本地存档与服务器存档不同，以服务器为准");
                    }
                }
            }
            else if (local == null && net != null)
            {
                // 只在服务器有存档
                Archive = net;
                Archive.Save();
            }
            else if (local != null && net == null)
            {
                // 只在本地有存档
                Archive = local;
                PushSlot();
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


        class SaveSlotReq
        {
            public string ArchiveName;
            public long CreateDate;
            public long SlotVersion;
            public string SlotData;
            public string VerifyCode;
        }

        // 推送存档到服务器
        public void PushSlot(Action<bool> pushCb = null)
        {
            // 上传存档
            PushSlot(Archive, pushCb);
        }

        // 推送存档到服务器
        public void PushSlot(ArchiveData archiveData, Action<bool> pushCb = null)
        {
            if (archiveData == null)
            {
                pushCb?.Invoke(false);
                return;
            }

            if (archiveData.ArchiveName == "slot")
            {
                return;
            }

            // 上传存档
            var slot = new SaveSlotReq
            {
                ArchiveName = archiveData.ArchiveName,
                CreateDate = archiveData.CreateDate,
                SlotVersion = archiveData.SlotVersion,
                SlotData = archiveData.ReadEncryption(),
                VerifyCode = MD5Helper.StringToMD5Hash("gixJ[4HoyE" + archiveData.ArchiveName + ";eeGLq4ntT6X2")
            };
            var postData = JsonConvert.SerializeObject(slot);

            // ObservableWebRequest.PostJson($"{DefaultRuntime.SlotServerURI}/SaveSlot", postData).Subscribe(
            ObservableWebRequest.PostJson($"{DefaultRuntime.SlotServerURI}/SaveVerify", postData).Subscribe(
                delegate(string s) { pushCb?.Invoke(true); },
                delegate(Exception exception)
                {
                    Tuner.Log("上传存档错误");
                    pushCb?.Invoke(false);
                });
        }
    }
}