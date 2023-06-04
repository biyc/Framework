using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HomePageMonoComponent : MonoBehaviour
{
    private Touch oldTouch1; //上次触摸点1(手指1)  
    private Touch oldTouch2; //上次触摸点2(手指2)  

    Touch newTouch1;
    Touch newTouch2;


    private Transform _container;


    // private bool _isMove = false;

    private Vector3 _distance;


    private int _index = 1;


    //private const float _minScale = 1.5f;
    private float _minScale = 2f;
    private const float STANDSCREENHEIGHT = 1920;
    private const float STANDSCREENWIDTH = 1080;

    private bool _isFirstMove = false;

    private Camera _uiCamera;

    public void Awake()
    {
    }


    public void Init()
    {
        InitDev();
        _uiCamera = GameObject.FindWithTag("Placeholder20").GetComponent<Camera>();
        Debug.Log("版本号:" + Define.GameSettings?.GetVersion() + " res:" + Define.AssetBundleVersion);
        _container = transform.Find("Container");
        var screenYScaleFactor = Screen.height / STANDSCREENHEIGHT;
        var screenXScaleFactor = Screen.width / STANDSCREENWIDTH;
        var resultScale = Mathf.Min(screenYScaleFactor, screenXScaleFactor);
        Debug.Log($"当前屏幕分辨率{Screen.width}:{Screen.height}---标准分辨率{STANDSCREENWIDTH}:{STANDSCREENHEIGHT}");
        Debug.Log($"屏幕宽缩放:{screenXScaleFactor},高缩放{screenYScaleFactor},最终缩放值：{resultScale}");
        //Debug.Log("屏幕缩放：" + resultScale);
        _minScale *= resultScale;
        transform.GetComponent<ABModelLoad>().SetRecovery(Recovery);

        #region 手势识别

        Observable.EveryUpdate().Subscribe(_ =>
        {
            //没有触摸  
            if (Input.touchCount <= 0)
                return;


            //单指旋转
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
            //双指移动  && 缩放
            else if (2 == Input.touchCount)
            {
                newTouch1 = Input.GetTouch(0);
                newTouch2 = Input.GetTouch(1);

                var dir1 = newTouch1.position - oldTouch1.position;
                var dir2 = newTouch2.position - oldTouch2.position;
                var angle = Vector2.Angle(dir1, dir2);
                // if (angle == 0)
                //     Debug.LogError(dir1 + ":" + dir2);

                bool isMove = angle <= 50 && angle > 0;

                if (isMove && _isFirstMove)
                {
                    _isFirstMove = false;
                    SetParame(newTouch2.position, 1);
                }
                else
                {
                    _isFirstMove = true;
                }

                void SetParame(Vector2 screenPoint, int index)
                {
                    _index = index;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _container.parent.GetComponent<RectTransform>(), screenPoint, _uiCamera,
                        out Vector2 pos);
                    _distance = _container.localPosition - (Vector3)pos;
                }


                if (newTouch1.phase == TouchPhase.Moved && newTouch2.phase == TouchPhase.Moved && isMove)
                {
                    var touch = Input.GetTouch(_index);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _container.parent.GetComponent<RectTransform>(), touch.position, _uiCamera,
                        out Vector2 pos);
                    _container.localPosition = _distance + (Vector3)pos;
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
                if (!isMove)
                {
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

                    //Debug.LogError(scale.x + ":" + _minScale);
                    if (scale.x > _minScale)
                        _container.localScale = scale;
                    // _container.localScale = scale;
                }

                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        });

        #endregion

        transform.Find("Homepage/recovery").GetComponent<Button>().onClick.AddListener(Recovery);

        if (Define.IsExportProject)
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
                UnityCallAndroid._.OnUnityInitialCompleteFun();
        }
    }

    /// <summary>
    /// 复原
    /// </summary>
    public void Recovery()
    {
        _container.localScale = new Vector3(_minScale, _minScale, _minScale);
        _container.localEulerAngles = Vector3.zero;
        _container.localPosition = new Vector3(0, 0, -13000);
    }

    private void InitDev()
    {
        // var netPath = "http://192.168.8.6:8088/EditorWin64Dev/EditorWin64/PrefabBundles/" + name;
        //var baseNetPath = "http://192.168.8.6:8088/AndroidDev/Android/PrefabBundles/";
        transform.Find("Homepage/devBtn").gameObject.SetActive(Define.IsDev);
        var panel = transform.Find("DevPanel");
        var netrts = transform.Find("DevPanel/netStoryPanel").GetComponentsInChildren<Text>().ToList();
        var namerts = transform.Find("DevPanel/nameStoryPanel").GetComponentsInChildren<Text>().ToList();

        netrts.ForEach(m => Btn(m.transform,
            () => transform.Find("DevPanel/netInput").GetComponent<InputField>().text = m.text));
        namerts.ForEach(m => Btn(m.transform,
            () => transform.Find("DevPanel/nameInput").GetComponent<InputField>().text = m.text));

        // var light = GameObject.FindObjectOfType<Light>();
        // transform.Find("DevPanel/Light").GetComponent<InputField>().onEndEdit.AddListener(m =>
        // {
        //     if (float.TryParse(m, out float result))
        //     {
        //         result = Mathf.Clamp(result, 0, 1);
        //         light.intensity = result;
        //     }
        // });

        Btn(transform.Find("DevPanel/devBg"), () => panel.gameObject.SetActive(false));
        Btn(transform.Find("Homepage/devBtn"), () =>
        {
            panel.gameObject.SetActive(true);
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
        Btn(transform.Find("DevPanel/clickOk"), async () =>
        {
            var inputNetPath = transform.Find("DevPanel/netInput").GetComponent<InputField>().text;
            var inputName = transform.Find("DevPanel/nameInput").GetComponent<InputField>().text;
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

            // var path=""
            transform.GetComponent<ABModelLoad>().LoadObj(inputName, inputNetPath, inputNetPath, null);
            // transform.GetComponent<ABModelLoad>().LoadObj(inputName, String.Empty, inputNetPath, null);
            panel.gameObject.SetActive(false);
        });
    }

    public static void Btn(Transform go, Action onClick, bool isClear = true, bool onlyOnce = false)
    {
        // 获取按钮组件
        if (!go.TryGetComponent(out Button button))
            button = go.gameObject.AddComponent<Button>();

        // 根据设定清空按钮上的时间
        if (isClear)
            button.onClick.RemoveAllListeners();

        // 绑定按钮事件
        button.onClick.AddListener(delegate
        {
            onClick?.Invoke();
            if (onlyOnce)
                button.onClick.RemoveAllListeners();
        });
    }
}