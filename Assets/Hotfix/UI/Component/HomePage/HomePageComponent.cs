using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blaze.Resource;
using Blaze.Utility.Helper;
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

        // private bool _isMove = false;

        private Vector3 _distance;


        private int _index = 1;

        private Transform _loading;

        
        #region Singleton

        private static HomePageComponent Instance;

        public static HomePageComponent _ => Instance;

        #endregion
        
        public override async void Awake()
        {
            base.Awake();
            Instance = this;
            // 初始化绑定
            Bind = new HomePageBind();
            Bind.InitUI(_curStage.transform);
            InitDev();
            //InitRedPoint();
            Debug.Log("版本号:" + Define.GameSettings?.GetVersion() + " res:" + Define.AssetBundleVersion);
            _container = _curStage.transform.Find("Container");
            _loading = _curStage.transform.Find("Loading");


            #region 手势识别

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
                    if (Math.Abs(Mathf.Abs(deltaPos.x) + Mathf.Abs(deltaPos.y) - 1) < 0.0001f) return;
                    if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
                        _container.Rotate(Vector3.down * deltaPos.x, Space.World); //绕y轴旋转
                    else if (Mathf.Abs(deltaPos.x) <= Mathf.Abs(deltaPos.y))
                        _container.Rotate(Vector3.right * deltaPos.y, Space.World); //绕x轴
                    //Debug.Log(deltaPos.x + " : " + deltaPos.y);
                }
                //双指旋转  && 缩放
                else if (2 == Input.touchCount)
                {
                    newTouch1 = Input.GetTouch(0);
                    newTouch2 = Input.GetTouch(1);

                    //移动
                    //** 如果两个手指同时按下滑动， 会同时触发began
                    //如果两个手指不是同时按下，有先后，只会触发第二个手指的began,
                    //坑： a,b手指，a先按下，b再按下，此时滑动会触发b的began,而a不会触发。
                    //此时抬起a手指，b保持按着，再按下a手指，此时滑动只会触发a的began，不会触发b的
                    if (newTouch1.phase == TouchPhase.Began && newTouch2.phase == TouchPhase.Began)
                        SetParame(newTouch2.position, 1);
                    else if (newTouch2.phase == TouchPhase.Began)
                        SetParame(newTouch2.position, 1);
                    else if (newTouch1.phase == TouchPhase.Began)
                        SetParame(newTouch1.position, 0);

                    void SetParame(Vector2 screenPoint, int index)
                    {
                        _index = index;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            _container.parent.GetRectTransform(), screenPoint, BUI.GetUICamera(),
                            out Vector2 pos);
                        _distance = _container.localPosition - (Vector3) pos;
                    }

                    if (newTouch1.phase == TouchPhase.Moved && newTouch2.phase == TouchPhase.Moved)
                    {
                        var touch = Input.GetTouch(_index);
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            _container.parent.GetRectTransform(), touch.position, BUI.GetUICamera(),
                            out Vector2 pos);
                        _container.localPosition = _distance + (Vector3) pos;
                    }


                    //缩放

                    if (newTouch2.phase == TouchPhase.Began)
                    {
                        oldTouch2 = newTouch2;
                        oldTouch1 = newTouch1;
                        return;
                    }

                    var oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                    var newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
                    var offset = newDistance - oldDistance;


                    if (!(Mathf.Abs(offset) >= 3)) return;
                    var scaleFactor = offset / 100f;
                    Vector3 localScale = _container.localScale;
                    Vector3 scale = new Vector3(localScale.x + scaleFactor,
                        localScale.y + scaleFactor,
                        localScale.z + scaleFactor);
                    // if (scale.x > 0)
                    // {
                    //     _container.localScale = scale;
                    //     Debug.Log("Scale:" + scale.x);
                    // }

                    if (scale.x > 2.08f)
                        _container.localScale = scale;
                    // _container.localScale = scale;
                    oldTouch1 = newTouch1;
                    oldTouch2 = newTouch2;
                }
            });

            #endregion

            Bind.button_recovery.onClick.AddListener(Recovery);
        }

        /// <summary>
        /// 复原
        /// </summary>
        public void Recovery()
        {
            _container.localScale = new Vector3(2.08f, 2.08f, 2.08f);
            _container.localEulerAngles = Vector3.zero;
            _container.localPosition = new Vector3(0, 0, -13000);
        }


        public async Task LoadObj(string resPath, string name, string baseNetPath = "")
        {
            // Debug.Log("netPath:" + PathHelper.Combine(baseNetPath, name));
            if (_target != null && name == _target.name)
                return;
            if (_target != null)
            {
                var o = _target;  
                _target = null;
                UnityEngine.Object.Destroy(o.gameObject);
            }

            _loading.Show();

            _currentName = name;

            if (!await Res.DownLoadModelAsset( resPath,name, baseNetPath))
            {
                _loading.Hide();
                return;
            }

            var path = $"Assets/Projects/3d/Models/{name}/{name}.fbx";

            // await LoadTarget(assetPath);
            //.prefab  fbx


            var task = Res.InstantiateAsync(path, _container);
            task.OnLoad(m =>
            {
                // if (m == null)
                // {
                //     _loading.Hide();
                //     return; 
                // }

                //在下载过程中点击了其他的物品
                if (_currentName != name)
                {
                    UnityEngine.Object.Destroy(m.Target);
                    return;
                }

                _target = m.Target.transform;
                _target.name = name;
                _target.tag = TARGETTAG;
                _target.localScale = new Vector3(1000, 1000, 1000);
                _target.GetComponentsInChildren<Transform>()
                    .ForEach(tr => tr.gameObject.layer = LayerMask.NameToLayer("UI"));
                Recovery();
                _loading.Hide();
            });
        }


        private void InitDev()
        {
            // var netPath = "http://192.168.8.6:8088/EditorWin64Dev/EditorWin64/PrefabBundles/" + name;
            //var baseNetPath = "http://192.168.8.6:8088/AndroidDev/Android/PrefabBundles/";
            Bind.button_devBtn.gameObject.SetActive(Define.IsDev);
            var panel = _curStage.transform.Find("DevPanel");
            var netrts = Bind.gridlayoutgroup_netStoryPanel.transform.GetComponentsInChildrenNoRoot<Text>();
            var namerts = Bind.gridlayoutgroup_nameStoryPanel.transform.GetComponentsInChildrenNoRoot<Text>();

            netrts.ForEach(m => Btn(m.gameObject, () => Bind.inputfield_netInput.text = m.text));
            namerts.ForEach(m => Btn(m.gameObject, () => Bind.inputfield_nameInput.text = m.text));

            Btn(Bind.image_devBg, () => panel.Hide());
            Btn(Bind.button_devBtn, () =>
            {
                panel.Show();
                var localNet = PlayerPrefs.GetString("net").Split('|').ToList();
                for (var i = 0; i < netrts.Count; i++)
                {
                    netrts[i].gameObject.SetActive(i < localNet.Count);
                    if (i < localNet.Count)
                        netrts[i].text = localNet[i];
                }

                var localName = PlayerPrefs.GetString("name").Split('|').ToList();
                for (var i = 0; i < namerts.Count; i++)
                {
                    namerts[i].gameObject.SetActive(i < localName.Count);
                    if (i < localName.Count)
                        namerts[i].text = localName[i];
                }
            });
            Btn(Bind.button_clickOk, () =>
            {
                var inputNetPath = Bind.inputfield_netInput.text;
                var inputName = Bind.inputfield_nameInput.text;
                // if (string.IsNullOrEmpty(inputNetPath) || string.IsNullOrEmpty(inputName)) return;
                if (string.IsNullOrEmpty(inputName)) return;
                var localNet = PlayerPrefs.GetString("net").Split('|').ToList();
                if (localNet.Contains(inputNetPath))
                    localNet.Remove(inputNetPath);
                localNet.Insert(0, inputNetPath);

                if (localNet.Count == 4)
                    localNet.RemoveAt(localNet.Count - 1);
                var localName = PlayerPrefs.GetString("name").Split('|').ToList();
                if (localName.Contains(inputName))
                    localName.Remove(inputName);
                localName.Insert(0, inputName);
                if (localName.Count == 6)
                    localName.RemoveAt(localName.Count - 1);

                var strNet = "";
                for (var i = 0; i < localNet.Count; i++)
                {
                    if (i != localNet.Count - 1)
                        strNet += localNet[i] + "|";
                    else
                        strNet += localNet[i];
                }

                var strName = "";
                for (var i = 0; i < localName.Count; i++)
                {
                    if (i != localName.Count - 1)
                        strName += localName[i] + "|";
                    else
                        strName += localName[i];
                }

                PlayerPrefs.SetString("name", strName);
                PlayerPrefs.SetString("net", strNet);

                LoadObj("", inputName, inputNetPath);
                panel.Hide();
            });
        }

        public override void Dispose()
        {
            if (IsDisposed) return;
            base.Dispose();
        }
    }
}