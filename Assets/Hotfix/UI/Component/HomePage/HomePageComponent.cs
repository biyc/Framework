using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blaze.Manage.Csv.Enum;
using Blaze.Resource;
using Blaze.Resource.AssetBundles;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Resource.Common;
using Blaze.Utility;
using Blaze.Utility.Helper;
using DG.Tweening;
using ETModel;
using Hotfix.Game.Reddot;
using Hotfix.Game.Reddot.Data;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

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

        public float speed = 5f;
        private Touch oldTouch1; //上次触摸点1(手指1)  
        private Touch oldTouch2; //上次触摸点2(手指2)  

        Touch newTouch1;
        Touch newTouch2;
        private Transform _target;

        private Transform _container;


        /// <summary>
        /// 当前应该显示的资源
        /// </summary>
        private string _currentName;


        public override async void Awake()
        {
            base.Awake();
            // 初始化绑定
            Bind = new HomePageBind();
            Bind.InitUI(_curStage.transform);
            //InitRedPoint();
            _container = _curStage.transform.Find("Container");
            //  LoadObj("cheqian");

            //test
            _curStage.transform.GetComponentsInChildren<Button>().ToList()
                .ForEach(m =>
                {
                    if (m.name != "recovery")
                    {
                        async void Call()
                        {
                            Recovery();
                            await LoadObj(m.name);
                        }

                        m.onClick.AddListener(Call);
                    }
                });

            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (_target == null)
                    return;
                //没有触摸  
                if (Input.touchCount <= 0)
                    return;
                //单点拖动
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector2 deltaPos = touch.deltaPosition; //位置增量
                    if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
                        _container.Rotate(Vector3.down * deltaPos.x, Space.World); //绕y轴旋转
                    else
                        _container.Rotate(Vector3.right * deltaPos.y, Space.World); //绕x轴
                }
                //双指旋转  && 缩放
                else if (2 <= Input.touchCount)
                {
                    //缩放
                    newTouch1 = Input.GetTouch(0); //
                    newTouch2 = Input.GetTouch(1);
                    if (newTouch2.phase == TouchPhase.Began)
                    {
                        oldTouch2 = newTouch2;
                        oldTouch1 = newTouch1;
                        return;
                    }

                    float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                    float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
                    float offset = newDistance - oldDistance;


                    if (Mathf.Abs(offset) >= 3)
                    {
                        float scaleFactor = offset / 100f;
                        Vector3 localScale = _container.localScale;
                        Vector3 scale = new Vector3(localScale.x + scaleFactor,
                            localScale.y + scaleFactor,
                            localScale.z + scaleFactor);
                        if (scale.x > 0.3f && scale.x < 1.5f)
                        {
                            _container.localScale = scale;
                        }

                        oldTouch1 = newTouch1;
                        oldTouch2 = newTouch2;
                    }
                }
            });

            Bind.button_recovery.onClick.AddListener(Recovery);
        }

        /// <summary>
        /// 复原
        /// </summary>
        public void Recovery()
        {
            _container.localScale = Vector3.one;
            _container.localEulerAngles = Vector3.zero;
        }

        public async Task LoadObj(string name)
        {
            if (_target != null && name == _target.name)
                return;
            if (_target != null)
            {
                var o = _target;
                _target = null;
                GameObject.Destroy(o.gameObject);
            }

            _currentName = name;
            var path = $"Assets/Projects/Prefabs/{name}/{name}.fbx";

            // await LoadTarget(assetPath);
            //.prefab  fbx
            var task = Res.InstantiateAsync(path, _container);
            task.OnLoad(m =>
            {
                //在下载过程中点击了其他的物品
                if (_currentName != name)
                {
                    GameObject.Destroy(m.Target);
                    return;
                }

                _target = m.Target.transform;
                _target.name = name;
                _target.localScale = new Vector3(1000, 1000, 1000);
                //_target.localPosition = new Vector3(0, 0, -500);
                _target.gameObject.layer = LayerMask.NameToLayer("UI");
            });
        }


        public override void Dispose()
        {
            if (IsDisposed) return;
            base.Dispose();
        }
    }
}