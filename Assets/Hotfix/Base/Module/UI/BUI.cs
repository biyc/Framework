using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class BUI
    {
        public static void SetViewParent(GameObject obj, UILayerEnum layer)
        {
            ETHotfix.Game.Scene.GetComponent<UIComponent>().SetViewParent(obj,layer);
        }

        /// <summary>
        /// 切换UI
        /// </summary>
        /// <param name="uiArgs"></param>
        /// <returns></returns>
        public static Task<UI> Switch(UIArgs uiArgs)
        {
            return ETHotfix.Game.Scene.GetComponent<UIComponent>().CreateAsync(uiArgs);
        }


        public static Task<T> Switch<T>(UIArgs uiArgs) where T : UIBaseComponent
        {
            return ETHotfix.Game.Scene.GetComponent<UIComponent>().CreateAsync<T>(uiArgs);
        }

        /// <summary>
        /// 从参数创建UI面板
        /// </summary>
        /// <param name="uiArgs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<UI> Create(UIArgs uiArgs, Transform parent = null)
        {
            return ETHotfix.Game.Scene.GetComponent<UIComponent>().CreateAsync(uiArgs,parent);
        }

        /// <summary>
        /// 从参数创建UI面板
        /// </summary>
        /// <param name="uiArgs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<T> Create<T>(UIArgs uiArgs, Transform parent = null) where T : UIBaseComponent
        {
            return ETHotfix.Game.Scene.GetComponent<UIComponent>().CreateAsync<T>(uiArgs,parent);
        }

        /// <summary>
        /// 通过已有的 对象 创建UI组件
        /// 向 GameObject 挂载 UIBaseComponent
        /// </summary>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateFromGo<T>(GameObject gameObject) where T : UIBaseComponent, new()
        {
            return ETHotfix.Game.Scene.GetComponent<UIComponent>().CreateFromGo<T>(gameObject);
        }

        public static void CleanFromGo<T>() where T : UIBaseComponent
        {
            ETHotfix.Game.Scene.GetComponent<UIComponent>().CleanFromGo<T>();
        }

        public static Camera GetUICamera()
        {
            return ETHotfix.Game.Scene.GetComponent<UIComponent>().uiCamera;
        }



        /// <summary>
        /// 从管理器中移除UI面板
        /// </summary>
        /// <param name="ui"></param>
        [Obsolete("方法过时，请直接调用页面中的 Close() 方法来销毁页面")]
        public static void Remove(UI ui)
        {
            ui.Close();
        }
    }
}