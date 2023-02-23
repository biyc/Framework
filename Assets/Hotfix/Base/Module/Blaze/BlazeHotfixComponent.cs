using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blaze.Manage.Locale;
using Blaze.Manage.Spring;
using ETModel;
using Hotfix.Base.Module.Blaze.Audio;

namespace ETHotfix
{
    [ObjectSystem]
    public class BlazeComponentAwakeSystem: AwakeSystem<BlazeHotfixComponent>
    {
        public override void Awake(BlazeHotfixComponent self)
        {
            self.Awake();
        }
    }

    public class BlazeHotfixComponent: Component
    {
        public void Awake()
        {
            // 启动多语言模块
            LocaleManager._.Initialize();
            // 启动声音管理器
            AudioManager._.Initialize();

            SpringManager._.Initialize();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            // 关闭多语言模块
            LocaleManager._.OnDestroy();
        }
    }
}