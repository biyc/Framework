using Blaze.Manage.Download;
using Blaze.Resource;
using Blaze.Resource.Task;
using Blaze.Utility;
using ETModel.Manage.Tools;

namespace ETModel
{
    [ObjectSystem]
    public class BlazeComponentAwakeSystem: AwakeSystem<BlazeComponent>
    {
        public override void Awake(BlazeComponent self)
        {
            self.Awake();
        }
    }

    public class BlazeComponent: Component
    {
        public void Awake()
        {
            Tuner.Info(Co._("           love:red:b;Blaze Engine Launched!:darkrad:b;love:red:b;"));

            // 设置基础参数
            BlazeConfig.Config();
            // 携程系统
            MonoScheduler.Initialize();
            // 初始化下载器
            DownloadManager._.Initialize();
            // 装载资源系统
            ResManager._.Initialize();
            // 公共工具（服务器时间等）
            ToolsManager._.Initialize();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            DownloadManager._.OnDestroy();
        }
    }
}