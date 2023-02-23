
using Blaze.Core;
using Hotfix.Game.Common.Logic;
using Hotfix.Game.Reddot;

namespace Hotfix.Game.Common
{
    /// <summary>
    /// 女魔头全局游戏管理器
    /// </summary>
    public class CommonManager : Singeton<CommonManager>
    {

        private bool _isInit = false;
        
        /// <summary>
        /// 初始化模块( splash 加载页面显示后执行)
        /// </summary>
        public void Initialize()
        {
            // 检查是否已初始化
            if (_isInit)
                return;
            _isInit = true;
            // 初始背包
           // LGraph._.Init();
            // 红点
            RedManager._.Init();
            // 内部弹窗通知
            InnerNotice._.Init();
            // 签到
           // SigninManager._.Initialize();

        }


    }
}