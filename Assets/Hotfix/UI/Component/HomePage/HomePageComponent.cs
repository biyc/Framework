using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blaze.Resource;
using ETModel;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace ETHotfix
{
    [ETModel.ObjectSystem]
    public class HomepageComponentAwakeSystem : AwakeSystem<HomePageComponent>
    {
        public override void Awake(HomePageComponent self)
        {
            self.Awake();
        }
    }


    public class HomePageComponent : UIBaseComponent
    {
        /// <summary>
        /// 当前UI页的配置实例
        /// </summary>
        public static UIArgs Args = new UIArgs()
        {
            Name = "HomePage",
            PrefebPath = "Assets/Projects/UI/HomePage/HomePage.prefab",
            ComponentType = typeof(HomePageComponent),
            Layer = UILayerEnum.Bottom,
            IsShowOnLoad = false,
            IsUseCreateMask = false
        };


        private HomePageBind Bind;
        public override async void Awake()
        {
            base.Awake();
            // 初始化绑定
            Bind = new HomePageBind();
            Bind.InitUI(_curStage.transform);
            //InitRedPoint();
            Debug.Log("版本号:" + Define.GameSettings?.GetVersion() + " res:" + Define.AssetBundleVersion);
        }
        
        public override void Dispose()
        {
            if (IsDisposed) return;
            base.Dispose();
        }
    }
}