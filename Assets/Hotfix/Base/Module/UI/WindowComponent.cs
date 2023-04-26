
using ETHotfix;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class WindowComponent : UIBaseComponent
    {
        public override void InitUI(GameObject gameObject)
        {
            base.InitUI(gameObject);
            // 页面创建出来的时候，就创建黑色背景遮罩
            CreatMask();
        }
        public override void Show()
        {
            base.Show();
            Init();
            OpenAnimation();
        }

        /// <summary>
        /// 黑色背景遮罩
        /// </summary>
        void CreatMask()
        {
            var mask = new GameObject("mask");
            mask.transform.parent = _curStage.transform;
            mask.transform.SetAsFirstSibling();
            var img = mask.AddComponent<Image>();
            var rect = mask.transform.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            img.color = new Color(0, 0, 0, 0.3f);
            Btn(img.transform.gameObject, ClickMask);
            // img.transform.GetButton().onClick.AddListener(() => );
        }

        protected virtual void ClickMask() {
            Close();
        }

        protected virtual void Init()
        {

        }
        protected virtual void OpenAnimation()
        {

        }
        protected virtual void DoClose()
        {
            CloseAnimation();
        }
        protected virtual void CloseAnimation()
        {
            Close();
        }


    }
}