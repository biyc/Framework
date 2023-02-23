using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    /// <summary>
    /// 屏蔽全局点击的遮罩
    /// </summary>
    public class UIEventMaskComponent : UIBaseComponent
    {
        /// <summary>
        /// 当前UI页的配置实例
        /// </summary>
        public static UIArgs Args = new UIArgs()
        {
            Name = "UIEventMask",
            ComponentType = typeof(UIEventMaskComponent),
            Layer = UILayerEnum.Top,
            IsUseCreateMask = false
        };

        public override void InitUI(GameObject gameObject)
        {
            base.InitUI(gameObject);
            var msk = _curStage.AddComponent<Image>();
            msk.color = Color.clear;
            msk.maskable = true;
            msk.raycastTarget = true;
        }
    }
}