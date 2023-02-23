using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Slideshow")]
public class Slideshow : MonoBehaviour
{
    /// <summary>
    /// 显示轮播图的窗口
    /// </summary>
    public Transform window;

    /// <summary>
    /// 显示标记的位置
    /// </summary>
    public Transform bar;

    /// <summary>
    /// 未选择时的标记
    /// </summary>
    public Sprite unselectMark;

    /// <summary>
    /// 选择时的标记
    /// </summary>
    public Sprite selectMark;

    /// <summary>
    /// 轮播的图片
    /// </summary>
    private List<GameObject> allImgs = new List<GameObject>();

    /// <summary>
    /// 图片的大小
    /// </summary>
    [SerializeField] private Vector2 _imgSize;

    /// <summary>
    /// 标记的大小
    /// </summary>
    [SerializeField] private Vector2 _markSize;

    /// <summary>
    /// 当前显示的下标值
    /// </summary>
    private int _curIndex;

    /// <summary>
    /// 转场时间
    /// </summary>
    private float _cutToTime = 0.3f;

    /// <summary>
    /// 停留时间
    /// </summary>
    private float _remainTime = 3;

    /// <summary>
    /// 是否播放动画
    /// </summary>
    private bool _isPlaying = false;

    /// <summary>
    /// 停留计时
    /// </summary>
    private float _remainTimer = 0;

    private RectTransform _leftObj;
    private RectTransform _conterObj;
    private RectTransform _rightObj;

    /// <summary>
    /// 转场动画
    /// </summary>
    private Tweener _tweener = null;

    private void Update() {
        if (_isPlaying && _tweener == null) {
            _remainTimer += Time.deltaTime;
            if (_remainTimer >= _remainTime) {
                _tweener = DOTween.To(() => 0f, t => {
                    _leftObj.anchoredPosition = new Vector2(-_imgSize.x - t, 0);
                    _conterObj.anchoredPosition = new Vector2(-t, 0);
                    _rightObj.anchoredPosition = new Vector2(_imgSize.x - t, 0);
                }, _imgSize.x, _cutToTime).SetEase(Ease.Linear);
                _tweener.onComplete += () => {
                    RectTransform cache = _leftObj;
                    _leftObj = _conterObj;
                    _conterObj = _rightObj;
                    _rightObj = cache;
                    SetCurIndex((_curIndex + 1) % allImgs.Count);
                    allImgs[(_curIndex + 1) % allImgs.Count].transform.SetParent(_rightObj, false);

                    _tweener = null;
                };


                _remainTimer = 0;
            }
        }
    }

    /// <summary>
    /// 设置当前的下标值
    /// </summary>
    /// <param name="index"></param>
    private void SetCurIndex(int index) {
        bar.GetChild(_curIndex).GetComponent<Image>().sprite = unselectMark;
        bar.GetChild(index).GetComponent<Image>().sprite = selectMark;
        _curIndex = index;
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="imgs"></param>
    public void SetImgs(List<GameObject> imgs) {
        allImgs = imgs;
        InitShow();
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="imgsParent"></param>
    public void SetImgs(Transform imgsParent) {
        for (int i = 0; i < imgsParent.childCount; i++) {
            allImgs.Add(imgsParent.GetChild(i).gameObject);
        }

        InitShow();
    }

    private void InitShow() {
        DestroyAllChild(window);
        DestroyAllChild(bar);
        if (allImgs.Count == 0) {
            _isPlaying = false;
            return;
        } else if (allImgs.Count == 1) {
            GameObject img = CreateObj(_imgSize, window);
            allImgs[0].transform.SetParent(img.transform, false);
            _conterObj = img.GetComponent<RectTransform>();
            GameObject mark = CreateObj(_markSize, bar);
            mark.AddComponent<Image>().sprite = selectMark;
            _curIndex = 0;

            _isPlaying = false;
        } else {
            GameObject conterImg = CreateObj(_imgSize, window);
            allImgs[0].transform.SetParent(conterImg.transform, false);
            _conterObj = conterImg.GetComponent<RectTransform>();

            GameObject leftImg = CreateObj(_imgSize, window);
            allImgs[allImgs.Count - 1].transform.SetParent(leftImg.transform, false);
            _leftObj = leftImg.GetComponent<RectTransform>();
            _leftObj.anchoredPosition = new Vector2(-_imgSize.x, 0);

            GameObject rightImg = CreateObj(_imgSize, window);
            allImgs[1].transform.SetParent(rightImg.transform, false);
            _rightObj = rightImg.GetComponent<RectTransform>();
            _rightObj.anchoredPosition = new Vector2(_imgSize.x, 0);


            for (int i = 0; i < allImgs.Count; i++) {
                GameObject mark = CreateObj(_markSize, bar);
                mark.AddComponent<Image>().sprite = unselectMark;
            }

            bar.GetChild(0).GetComponent<Image>().sprite = selectMark;

            _curIndex = 0;

            _isPlaying = true;
        }
    }

    /// <summary>
    /// 创建一个对象
    /// </summary>
    /// <returns></returns>
    private GameObject CreateObj(Vector2 size, Transform parent) {
        GameObject obj = new GameObject("obj", typeof(RectTransform), typeof(CanvasRenderer));
        obj.transform.SetParent(parent, false);
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        SetSize(rectTransform, size);
        rectTransform.anchoredPosition = Vector2.zero;
        return obj;
    }

    /// <summary>
    /// 销毁对象的所有孩子
    /// </summary>
    /// <param name="trans"></param>
    private void DestroyAllChild(Transform trans) {
        if (trans.childCount < 0) return;
        int count = trans.childCount;
        while (count > 0) {
            UnityEngine.Object.DestroyImmediate(trans.GetChild(0).gameObject);
            count--;
        }
    }

    private void SetSize(RectTransform trans, Vector2 newSize) {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax +
                          new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
}