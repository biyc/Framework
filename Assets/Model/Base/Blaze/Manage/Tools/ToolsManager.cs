using Blaze.Core;

namespace ETModel.Manage.Tools
{
    /// <summary>
    /// 公共工具
    /// </summary>
    public class ToolsManager : Singeton<ToolsManager>
    {
        public void Initialize()
        {
            // 同步服务器时间
            NetTime._.Sync();
        }
    }
}