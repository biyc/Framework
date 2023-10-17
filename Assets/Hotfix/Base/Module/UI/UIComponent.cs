using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blaze.Resource;
using Blaze.Resource.Common;
using Blaze.Utility;
using ETModel;

using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiComponentAwakeSystem : AwakeSystem<UIComponent>
    {
        public override void Awake(UIComponent self)
        {
            self.Awake();
        }
    }


    /// <summary>
    /// 负责UI的创建于分层的初始化
    /// </summary>
    public class UIComponent : Component
    {
        public Camera uiCamera;
        private GameObject _root;
        private readonly Dictionary<string, GameObject> _allLayers = new Dictionary<string, GameObject>();

        // 设计分辨率
        public static readonly float DesignWidth = 1242f;

        public static readonly float DesignHeight = 2688f;

        // 屏幕像素到 设计分辨率像素的缩放比
        public static float ScaleFactor = 0;


        private float _scalaRate;

        /// <summary>
        /// 初始化UI设置，建立层级结构
        /// </summary>
        public void Awake()
        {
            _allLayers.Clear();
            _root = GameObject.Find("Global/UI/");

            uiCamera = GameObject.Find("Global/Camera/UICamera").GetComponent<Camera>();

            // 纯色背景层~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // 最底层铺一张白色背景，防止相机清除出问题
            var colorBg = new GameObject();
            colorBg.AddComponent<Image>().material.color = Color.white;
            colorBg.name = "ColorBg";
            colorBg.transform.SetParent(_root.transform);
            colorBg.transform.localPosition = Vector3.zero;
            colorBg.layer = LayerMask.NameToLayer("UI");

            var _tempCanvas = colorBg.AddComponent<Canvas>();
            _tempCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            _tempCanvas.worldCamera = uiCamera;
            _tempCanvas.sortingOrder = 0;
            _tempCanvas.sortingLayerName = "homeSky";
            // 纯色背景层~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


            int i = 0;
            foreach (UILayerEnum item in Enum.GetValues(typeof(UILayerEnum)))
            {
                i++;
                string layer = item.ToString();
                GameObject _go = GameObject.Find($"Global/UI/{layer}");

                if (!_go)
                {
                    _go = new GameObject();
                    _go.name = layer;
                    _go.transform.SetParent(_root.transform);
                    _go.transform.localPosition = Vector3.zero;
                    _go.layer = LayerMask.NameToLayer("UI");

                    var canvas = _go.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = uiCamera;
                    canvas.sortingOrder = i * 1000;
                    canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels) (1 | 1 << 3 | 1 << 4);


                    var scale = _go.AddComponent<CanvasScaler>();
                    scale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scale.referenceResolution = new Vector2(DesignWidth, DesignHeight);

                    var nowScreenRate = (float) Screen.height / (float) Screen.width;
                    var deviceSplit = 1.77; // 16f / 9f;
                  //  Tuner.Log("now: " + nowScreenRate + "  16:9: " + deviceSplit);
                    if (nowScreenRate < deviceSplit)
                    {
                        // PAD 模式
                        scale.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                        scale.matchWidthOrHeight = 0.5f;
                        //scale.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                    }
                    else
                        // 手机模式
                        scale.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;

                    _go.AddComponent<GraphicRaycaster>();

                    if (item == UILayerEnum.Null || item == UILayerEnum.Hide)
                        _go.SetActive(false);
                }


                _allLayers.Add(layer, _go);
            }
        }


        public override void Dispose()
        {
            if (IsDisposed)
                return;

            base.Dispose();
        }


        #region UI 页面生命周期管理

        /// <summary>
        /// 设置ui显示层级
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="layer"></param>
        public void SetViewParent(UI ui, UILayerEnum layer)
        {
            if (layer == UILayerEnum.Null)
                return;
            SetViewParent(ui.GameObject, layer);
            ui.CurLayer = layer;
            if (layer != UILayerEnum.Hide) ui.SetAsLastSibling();
        }

        public void SetViewParent(GameObject obj, UILayerEnum layer)
        {
            RectTransform _rt = obj.GetComponent<RectTransform>();
            _rt.SetParent(_allLayers[layer.ToString()].transform);
            _rt.anchorMin = Vector2.zero;
            _rt.anchorMax = Vector2.one;
            _rt.offsetMax = Vector2.zero;
            _rt.offsetMin = Vector2.zero;
            _rt.pivot = new Vector2(0.5f, 0.5f);
            _rt.localScale = Vector3.one;
            _rt.localPosition = Vector3.zero;
            _rt.localRotation = Quaternion.identity;
        }

        public Transform GetLayerTransform(UILayerEnum layerEnum)
        {
            return _allLayers[layerEnum.ToString()].transform;
        }

        #endregion


        #region 页面创建

        private readonly Dictionary<GameObject, UIBaseComponent>
            _goBind = new Dictionary<GameObject, UIBaseComponent>();

        /// <summary>
        /// 消除缓存中的循环对象引用
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        public void CleanFromGo<T>() where T : UIBaseComponent
        {
            var clean = new List<GameObject>();
            _goBind.ForEach(delegate(KeyValuePair<GameObject, UIBaseComponent> pair)
            {
                if (pair.Value is T)
                {
                    pair.Value.Dispose();
                    clean.Add(pair.Key);
                }
            });
            clean.ForEach(delegate(GameObject o) { _goBind.Remove(o); });
        }

        public T CreateFromGo<T>(GameObject gameObject) where T : UIBaseComponent, new()
        {
            // 销毁前一个绑定在此对象上的脚本
            if (_goBind.ContainsKey(gameObject))
            {
                _goBind[gameObject].DestoryGo = false;
                _goBind[gameObject]?.Dispose();
            }

            SafetyLayer(gameObject);
            // 创建UI 对象实例
            var ui = Game.ObjectPool.Fetch<T>();
            // 建立新的绑定关系
            _goBind[gameObject] = ui;
            // 通过外部 GameObject 创建的UI对象，UI对象销毁时，不销毁 GameObject
            ui.DestoryGo = false;
            ui.InitUI(gameObject);
            Game.EventSystem.Awake(ui);

            return ui;
        }

        public async Task<T> CreateAsync<T>(UIArgs uiArgs, Transform parent = null) where T : UIBaseComponent
        {
            return await CreateAsync(uiArgs, parent) as T;
        }


        /// <summary>
        /// 如果正在显示，不做处理
        /// 如果在hide ，显示出来
        /// 其他情况就创建一个新的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<UI> CreateAsync(UIArgs uiArgs, Transform parent = null)
        {
            //打开遮罩，阻挡点击事件,只有一个控件，很快的
            UI uiEventMask = null;
            if (uiArgs.IsUseCreateMask)
                uiEventMask = CreateEventMask(uiArgs.Name);

            // Tuner.Log(uiArgs.Name);
            try
            {
                var ui = await CreatePrefab(uiArgs, parent);
                // 放到正确的分层
                SetViewParent(ui, uiArgs.Layer);
                if (uiArgs.IsShowOnLoad)
                    ui.Show();
                else
                    ui.Hide();
                uiEventMask?.Close();

                return ui;
            }
            catch (Exception e)
            {
                Debug.LogError($"{uiArgs.Name} UI 错误: \n {e.StackTrace}");
                throw new Exception($"{uiArgs.Name} UI 错误");
            }
        }

        /// <summary>
        /// 创建事件遮罩
        /// </summary>
        /// <returns></returns>
        private UI CreateEventMask(string name = "")
        {
            var uiArgs = UIEventMaskComponent.Args;
            var go = new GameObject();
            go.AddComponent<RectTransform>();
            go.name = uiArgs.Name + "-" + name;
            go.layer = LayerMask.NameToLayer("UI");

            // 创建UI 对象实例
            var ui = Game.ObjectPool.Fetch(uiArgs.ComponentType) as UI ?? Game.ObjectPool.Fetch<UI>();
            ui.InitUI(go);
            // 放到正确的分层
            SetViewParent(ui, uiArgs.Layer);
            return ui;
        }


        /// <summary>
        /// 通过预制体创建UI
        /// </summary>
        /// <param name="uiArgs"></param>
        /// <returns></returns>
        private async Task<UI> CreatePrefab(UIArgs uiArgs, Transform parent = null)
        {
            var fullpath = uiArgs.PrefebPath;
            GameObject go = null;
            PrefabObject prefabObject = null;
            if (!string.IsNullOrEmpty(fullpath))
            {
                prefabObject = await Res.InstantiateAsync(fullpath, parent);
                if (prefabObject.Target != null)
                    go = prefabObject.Target;
            }

            // 从预制体初始化失败时，手动创建对象
            if (go == null)
            {
                go = new GameObject();
                go.AddComponent<RectTransform>();
            }

            SafetyLayer(go);
            go.name = uiArgs.Name;
            go.layer = LayerMask.NameToLayer("UI");


            // 创建UI 对象实例
            var ui = Game.ObjectPool.Fetch(uiArgs.ComponentType) as UI ?? Game.ObjectPool.Fetch<UI>();
            ui.CreateArgs = uiArgs;
            ui.InitUI(go);
            ui.GameObject.SetActive(false);
            // 源预制体加载器放入UI对象中，方便预制体资源释放
            ui.PrefabGameObject = prefabObject;
            

            // InitUI 调用后，可以调用组件自身的 Awake 方法了
            Game.EventSystem.Awake(ui);

            return ui;
        }

        private void SafetyLayer(GameObject go)
        {
            // 处理页面里面的安全区层
            var safe = UGui.FindChild(go.transform, "SafetyLayer");
            if (safe != null)
            {
                // 根据实际层大小，计算 屏幕像素到 设计分辨率像素的缩放比
                if (ScaleFactor == 0)
                {
                    var rt = _allLayers[UILayerEnum.Bottom.ToString()].GetComponent<RectTransform>();
                    ScaleFactor = rt.rect.width / Screen.width;
                }

                var area = Screen.safeArea;
                // 锚点（左下）
                safe.Comp<RectTransform>().anchorMin = new Vector2(0f, 0f);
                safe.Comp<RectTransform>().anchorMax = new Vector2(0f, 0f);
                // 安全区大小
                safe.Comp<RectTransform>().SetSize(new Vector2(area.width * ScaleFactor, area.height * ScaleFactor));

                // 设置中心点位置
                safe.Comp<RectTransform>().anchoredPosition =
                    new Vector2(area.center.x * ScaleFactor, area.center.y * ScaleFactor);

                // Debug.Log("position " + area.position);
                // Debug.Log("size " + area.size);
                // Debug.Log("center " + area.center);
                // Debug.Log("min " + area.min);
                // Debug.Log("max " + area.max);
                // Debug.Log("ScaleFactor " + ScaleFactor);
            }
        }

        #endregion
    }
}