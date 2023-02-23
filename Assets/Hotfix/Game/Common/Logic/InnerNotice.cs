using Blaze.Core;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Csv.Poco;
using ETHotfix;


namespace Hotfix.Game.Common.Logic
{
    /// <summary>
    /// 游戏内部通知
    /// </summary>
    public class InnerNotice : Singeton<InnerNotice>
    {
        /// <summary>
        /// 观察家具解锁
        /// </summary>
        // public IDataWatch<ResInfo> WatchFurnitureUnlock = new DataWatch<ResInfo>();
        public void Init()
        {
            ResIndex._.WatchReward.OnIneresting += delegate(ResInfo info)
            {
                string tip = "";

                switch (info.GetTypeName())
                {
                    case ResType.Furniture:
                        // 家具解锁
                        Log.Info("家具解锁:" + tip);
                        break;
                    case ResType.RoleRoom:
                        // 解锁小屋装扮房型
                        Log.Info("解锁小屋装扮房型:");
                        break;
                    // case ResType.Room:
                    default:
                        break;
                }
            };
           

        }
    }
}