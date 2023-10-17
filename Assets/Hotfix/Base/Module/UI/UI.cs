using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blaze.Manage.Data;
using Blaze.Resource.Asset;
using Blaze.Resource.Common;
using ETModel;
using UniRx;
using UnityEngine;

namespace ETHotfix
{
    /// <summary>
    /// UI 面板实例参数
    /// </summary>
    public class UIArgs
    {
        /// <summary>
        /// 窗口名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 预制体路径
        /// </summary>
        public string PrefebPath;

        /// <summary>
        /// UI所在层级
        /// </summary>
        public UILayerEnum Layer;

        /// <summary>
        /// 要向UI中附加的组件类型
        /// </summary>
        public Type ComponentType = typeof(UI);

        /// <summary>
        /// 加载成功后是否立即显示
        /// </summary>
        public bool IsShowOnLoad = true;

        /// <summary>
        /// 创建页面时候，是否需要生成MASK遮罩防止用户点击
        /// </summary>
        public bool IsUseCreateMask = true;

        /// <summary>
        /// 扩展信息
        /// </summary>
        public ExData Data = new ExData();

        // /// <summary>
        // /// 是否自动执行 Awake (默认自动触发)
        // /// </summary>
        // public bool IsAutoAwake = true;

        public class ExData
        {
            /// <summary>
            /// 人物ID
            /// </summary>
            public int RoleId = -1;
            
            /// <summary>
            /// 数据
            /// </summary>
            public object InfoData;
        }
    }


    /// <summary>
    /// 每个预制体都是一个 UI 节点，节点可以附加其它 UI
    /// </summary>
    public class UI : Entity
    {
        public string Name => GameObject.name;
        public bool DestoryGo = true;

        public GameObject GameObject { get; private set; }

        // 源预制体
        public PrefabObject PrefabGameObject;

        /// <summary>
        /// UI 当前所在的层
        /// </summary>
        public UILayerEnum CurLayer;


        /// <summary>
        /// 创建参数引用
        /// </summary>
        public UIArgs CreateArgs;

        /// <summary>
        /// 当前UI是否在显示
        /// </summary>
        public bool InShow => GameObject.activeSelf;

        public Action OnUIShowed;
        public Action OnUIHide;
        public Action OnUIClose;
        public Action OnUIDisposeBefore;
        public Action<UIArgs> OnFinish;
        public DataWatch<bool> WatchUIShowed = new DataWatch<bool>();
        
        #region 资源生命周期

        /// <summary>
        /// 页面关闭时需要释放的ASSET
        /// </summary>
        private List<UAssetDisposeInfo> _assets = new List<UAssetDisposeInfo>();

        private class UAssetDisposeInfo
        {
            public IUAsset Asset;
            public int AfterDispose;
        }

        private List<PrefabObject> _prefabObjects = new List<PrefabObject>();

        /// <summary>
        /// 托管 ASSET 资源给页面生命周期管理
        /// </summary>
        /// <param name="asset"></param>
        protected void AddAsset(IUAsset asset, int afterDispose = 0)
        {
            if (asset == null) return;
            asset.AddRef();
            var info = new UAssetDisposeInfo();
            info.Asset = asset;
            info.AfterDispose = afterDispose;
            _assets.Add(info);
        }

        protected void AddPrefabObjects(PrefabObject prefab)
        {
            if (prefab != null)
                _prefabObjects.Add(prefab);
        }

        #endregion

        #region 页面生命周期函数

        public virtual void InitUI(GameObject gameObject)
        {
            DisposeAllChildren();
            GameObject = gameObject;
        }

        /// <summary>
        /// 显示对象
        /// </summary>
        public virtual void Show()
        {
            GameObject.SetActive(true);
            OnUIShowed?.Invoke();
            WatchUIShowed.Complet(true);
        }

        /// <summary>
        /// 隐藏对象
        /// </summary>
        public virtual void Hide()
        {
            GameObject.SetActive(false);
            OnUIHide?.Invoke();
            WatchUIShowed.Complet(false);
        }

        /// <summary>
        /// 关闭当前页面
        /// </summary>
        public virtual void Close()
        {
            Dispose();
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            CreateArgs = null;

            // 通知观察者，当前页面关闭
            OnUIClose?.Invoke();

            // 打断 await 中的资源
            _awaitBreakPoint.End();
            _awaitBreakPoint = new AwaitBreakPoint();

            // 特殊对象，不返回到对象池中
            IsFromPool = false;

            OnUIDisposeBefore?.Invoke();
            OnUIShowed = null;
            OnUIDisposeBefore = null;
            OnUIClose = null;

            DisposeAllChildren();

            if (DestoryGo)
            {
                UnityEngine.Object.Destroy(GameObject);
                PrefabGameObject?.Destroy();
            }

            DestoryGo = true;
            PrefabGameObject = null;
            Parent = null;


            // 释放页面中托管的Asset资源
            _assets.ForEach(delegate(UAssetDisposeInfo disposeInfo)
            {
                if (disposeInfo.AfterDispose == 0)
                    disposeInfo.Asset?.RemoveRef();
                else
                {
                    Observable.Timer(TimeSpan.FromSeconds(disposeInfo.AfterDispose)).Subscribe(delegate(long l)
                    {
                        disposeInfo.Asset?.RemoveRef();
                    });
                }
            });

            _assets.Clear();
            // 释放页面中托管的PrefabObjects资源
            for (var i = 0; i < _prefabObjects.Count; i++)
            {
                _prefabObjects[i]?.Destroy();
            }

            _prefabObjects.Clear();

            base.Dispose();
        }
        

        #endregion

        #region 同层级位置控制

        public void SetAsFirstSibling()
        {
            GameObject.transform.SetAsFirstSibling();
        }

        public void SetAsLastSibling()
        {
            GameObject.transform.SetAsLastSibling();
        }

        #endregion


        #region AwaitBreakPoint

        private AwaitBreakPoint _awaitBreakPoint = new AwaitBreakPoint();

        protected async Task RelyAwait(Task task)
        {
            // 保存当前引用到局部，执行完毕后外部引用指向可能会改变，所以一定要保存引用
            var breakPoint = _awaitBreakPoint;
            // 执行正常任务
            await task;
            // 任务结束后，当前上下文是否已被释放
            await breakPoint;
        }

        protected async Task<T> RelyAwait<T>(Task<T> task)
        {
            var breakPoint = _awaitBreakPoint;
            // 执行正常任务
            var data = await task;
            // 任务结束后，当前上下文是否已被释放
            await breakPoint;
            return data;
        }

        #endregion

        #region 挂载到当前页面下的，子页面节点

        /// 挂载到当前页面下的子页面
        public Dictionary<GameObject, UI> children = new Dictionary<GameObject, UI>();

        /// <summary>
        /// 附加其它 UI 页面到当前页面
        /// </summary>
        /// <param name="ui"></param>
        public void Add(UI ui)
        {
            // 销毁之前的绑定实例
            if (children.ContainsKey(ui.GameObject) && children[ui.GameObject] != ui)
                children[ui.GameObject].Dispose();
            // 绑定到当前页面
            children[ui.GameObject] = ui;
            ui.Parent = this;
        }

        /// <summary>
        /// 销毁附加到当前页面的其它 UI
        /// </summary>
        /// <param name="go"></param>
        /// <param name="destoryGo"></param>
        public void Remove(UI ui, bool destoryGo = true) => Remove(ui.GameObject, destoryGo);

        /// <summary>
        /// 销毁附加到当前页面的其它 UI
        /// </summary>
        /// <param name="go"></param>
        /// <param name="destoryGo"></param>
        public void Remove(GameObject go, bool destoryGo = true)
        {
            if (!children.TryGetValue(go, out var ui))
                return;
            children.Remove(go);
            ui.DestoryGo = destoryGo;
            ui.Dispose();
        }


        /// <summary>
        /// 销毁所有附加的UI面板
        /// </summary>
        private void DisposeAllChildren()
        {
            foreach (var ui in children.Values)
                ui.Dispose();

            children.Clear();
        }

        #endregion
    }
}