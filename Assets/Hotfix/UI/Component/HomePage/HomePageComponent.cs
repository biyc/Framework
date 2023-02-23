using System.Linq;
using Blaze.Manage.Csv.Enum;
using Blaze.Resource;
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


        #region 功能逻辑相关

        private HomePageBind Bind;

        public float speed = 5f;
        private Touch oldTouch1; //上次触摸点1(手指1)  
        private Touch oldTouch2; //上次触摸点2(手指2)  

        Touch newTouch1;
        Touch newTouch2;
        private Transform _target;

        private Transform _container;

        private bool _isY;
        private bool _isSet;

        public override void Awake()
        {
            base.Awake();
            // 初始化绑定
            Bind = new HomePageBind();
            Bind.InitUI(_curStage.transform);
            //InitRedPoint();
            _container = _curStage.transform.Find("Container");
            LoadObj("cheqian");

            //test
            _curStage.transform.GetComponentsInChildren<Button>().ToList()
                .ForEach(m =>
                {
                    if (m.name != "recovery")
                    {
                        m.onClick.AddListener(() => LoadObj(m.name));
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
                    if (!_isSet)
                    {
                        _isY = Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y);
                        _isSet = true;
                    }

                    if (_isY)
                        _container.Rotate(Vector3.down * deltaPos.x, Space.World); //绕y轴旋转
                    else
                        _container.Rotate(Vector3.right * deltaPos.y, Space.World); //绕x轴
                }
                else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    _isSet = false;
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

        public void LoadObj(string name)
        {
            if (_target != null && name == _target.name)
                return;
            if (_target != null)
            {
                var o = _target;
                _target = null;
                GameObject.Destroy(o.gameObject);
            }

            var obj = Res.InstantiateAsync($"Assets/Projects/Prefabs/{name}/{name}.fbx", _container);

            obj.OnLoad(m =>
            {
                _target = m.Target.transform;
                _target.name = name;
                _target.localScale = new Vector3(1000, 1000, 1000);
                //_target.localPosition = new Vector3(0, 0, -500);
                _target.gameObject.layer = LayerMask.NameToLayer("UI");
            });
        }


        /// <summary>
        /// 初始化本页的红点
        /// </summary>
        private void InitRedPoint()
        {
            // // 绑定红点与监听状态
            // void ListenRed(RedType type, GameObject obj)
            // {
            //     var temp = RedManager._.GetPoint(type);
            //     if (temp != null)
            //         temp.OnMessage += delegate(RedData data) { obj.SetActive(data.IsLight); };
            // }
            //
            // ListenRed(RedType.Chapter, Bind.go_m_RedPonitChapter);
            // ListenRed(RedType.Sign, Bind.go_m_RedPonitSig);
            // ListenRed(RedType.Mail, Bind.go_m_RedPonitMail);
            // ListenRed(RedType.Phone, Bind.go_m_RedPonitPhone);
            // ListenRed(RedType.Mission, Bind.go_m_RedPonitMission);
        }


        public override void Dispose()
        {
            if (IsDisposed) return;
            base.Dispose();
        }

        #endregion
    }
}