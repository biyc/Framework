
using System;
using System.Linq;
using System.Threading.Tasks;
using Blaze.Resource;
using ETModel;
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

        private const string TARGETTAG = "TARGETTAG";

        private bool _isMove = false;

        private Vector3 _distance;

        public override async void Awake()
        {
            base.Awake();
            // 初始化绑定
            Bind = new HomePageBind();
            Bind.InitUI(_curStage.transform);
            //InitRedPoint();
            _container = _curStage.transform.Find("Container");
            // _container.gameObject.AddComponent<ItemDrag>();

           // LoadObj("cheqian");

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

                
                // if (Input.GetMouseButtonDown(0))
                // {
                //     var isHit = Physics.Raycast(BUI.GetUICamera().ScreenPointToRay(Input.mousePosition),
                //         out RaycastHit hitInfo,
                //         Mathf.Infinity);
                //     if (isHit && hitInfo.transform.CompareTag(TARGETTAG))
                //     {
                //         //移动
                //         _isMove = true;
                //         RectTransformUtility.ScreenPointToLocalPointInRectangle(
                //             _container.parent.GetRectTransform(), Input.mousePosition, BUI.GetUICamera(),
                //             out Vector2 pos);
                //         _distance = _container.localPosition - (Vector3) pos;
                //     }
                // }
                //
                // if (Input.GetMouseButton(0) && _isMove)
                // {
                //     RectTransformUtility.ScreenPointToLocalPointInRectangle(
                //         _container.parent.GetRectTransform(), Input.mousePosition, BUI.GetUICamera(),
                //         out Vector2 pos);
                //     _container.localPosition = (Vector3) pos + _distance;
                // }
                //
                //
                // if (Input.GetMouseButtonUp(0))
                //     _isMove = false;
                //没有触摸  
                if (Input.touchCount <= 0)
                    return;


                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    var isHit = Physics.Raycast(BUI.GetUICamera().ScreenPointToRay(Input.mousePosition),
                        out RaycastHit hitInfo,
                        Mathf.Infinity);
                    if (isHit && hitInfo.transform.CompareTag(TARGETTAG))
                    {
                        //移动
                        _isMove = true;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            _container.parent.GetRectTransform(), Input.mousePosition, BUI.GetUICamera(),
                            out Vector2 pos);
                        _distance = _container.localPosition - (Vector3) pos;
                    }
                    else
                    {
                        _isMove = false;
                    }
                }

                //单点拖动
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (_isMove)
                    {
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            _container.parent.GetRectTransform(), Input.mousePosition, BUI.GetUICamera(),
                            out Vector2 pos);
                        _container.localPosition = (Vector3) pos + _distance;
                    }
                    else
                    {
                        Touch touch = Input.GetTouch(0);
                        Vector2 deltaPos = touch.deltaPosition; //位置增量
                        if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
                            _container.Rotate(Vector3.down * deltaPos.x, Space.World); //绕y轴旋转
                        else
                            _container.Rotate(Vector3.right * deltaPos.y, Space.World); //绕x轴   
                    }
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
                        if (scale.x > 0)
                        {
                            _container.localScale = scale;
                            Debug.Log("Scale:" + scale.x);
                        }

                        // if (scale.x > 0.3f && scale.x < 1.5f)
                        // {
                        //     _container.localScale = scale;
                        // }
                        _container.localScale = scale;
                        oldTouch1 = newTouch1;
                        oldTouch2 = newTouch2;
                    }
                }

                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && _isMove)
                    _isMove = false;
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
            _container.localPosition=new Vector3(0,0,-13000);
        }

        public async Task LoadObj(string name)
        {
            if (_target != null && name == _target.name)
                return;
            if (_target != null)
            {
                var o = _target;
                _target = null;
                UnityEngine.Object.Destroy(o.gameObject);
            }
            
            _currentName = name;
            var path = $"Assets/Projects/Prefabs/{name}/{name}.fbx";
            
            // await LoadTarget(assetPath);
            //.prefab  fbx

            try
            {
                var task = Res.InstantiateAsync(path, _container);
                task.OnLoad(m =>
                {
                    //在下载过程中点击了其他的物品
                    if (_currentName != name)
                    {
                        UnityEngine.Object.Destroy(m.Target);
                        return;
                    }
                
                    _target = m.Target.transform;
                    _target.name = name;
                    _target.tag = TARGETTAG;
                    //_target.GetComponent<MeshCollider>().convex = true;
                    _target.gameObject.AddComponent<DoubleSideMeshCollider>();
                    _target.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.None;
                    _target.localScale = new Vector3(1000, 1000, 1000);
                    //_target.localPosition = new Vector3(0, 0, -500);
                    _target.gameObject.layer = LayerMask.NameToLayer("UI");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }


        public override void Dispose()
        {
            if (IsDisposed) return;
            base.Dispose();
        }
    }
}