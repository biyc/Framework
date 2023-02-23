using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIFrameShowComponentAwakeSystem : AwakeSystem<UIFrameShowComponent>
    {
        public override void Awake(UIFrameShowComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIFrameShowComponentUpdateSystem : UpdateSystem<UIFrameShowComponent>
    {
        public override void Update(UIFrameShowComponent self)
        {
            if (!self.InShow) return;
            self.Update();
        }
    }

    public class UIFrameShowComponent : UIBaseComponent
    {
        /// 当前UI页的配置实例
        public static UIArgs Args = new UIArgs()
        {
            Name = "UIFrameShow",
            PrefebPath = "Assets/Projects/UI/UICommon/UIFrameShow.prefab",
            ComponentType = typeof(UIFrameShowComponent),
            Layer = UILayerEnum.Top,
        };


        private UIFrameShowBind Bind = new UIFrameShowBind();
        int TimesEachSeconds;
        float currTime;

        public override void Awake()
        {
            // 一定要先执行基类的引用注入！！！
            base.Awake();
            Bind.InitUI(_curStage.transform);
            TimesEachSeconds = 0;
            currTime = 0f;
            Bind.text_FrameCount.text = Application.targetFrameRate + " FPS"; //初始值
        }

        public void Update()
        {
            currTime += Time.unscaledDeltaTime;
            TimesEachSeconds++;
            if (currTime >= 1f)
            {
                Bind.text_FrameCount.text =
                    TimesEachSeconds + " FPS " + Mathf.Round(10000f / TimesEachSeconds) / 10 + "ms";
                TimesEachSeconds = 0;
                currTime = 0f;
            }
        }
    }
}