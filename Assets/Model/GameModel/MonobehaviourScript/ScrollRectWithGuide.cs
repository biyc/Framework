using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Main;
using UniRx;
using UnityEngine.EventSystems;

public class ScrollRectWithGuide : ScrollRect, IPointerUpHandler,IPointerDownHandler
{
    public Action<PointerEventData> BeginDragCb;
    public Action ClickCb;
    private bool _isDrag = false;


    protected override void Awake()
    {
        base.Awake();
        this.onValueChanged.AddListener(v =>
        {
            if (_backBtn != null)
                BackBtnControl(v.y);
        });
    }


    #region 滑动引导

    private System.IDisposable _ob;
    private Tweener _tween;
    private int count = 0;
    private int _maxGuideCount = 2;

    /// <summary>
    /// 提示要向下移动
    /// </summary>
    /// <param name="delayTime"></param>
    /// <param name="dis"></param>
    /// <param name="time"></param>
    public void StartGuideDown(float delayTime = 5, float dis = 400, float time = 2f)
    {
        if (count >= _maxGuideCount)
            return;
        _ob = Observable.Timer(TimeSpan.FromSeconds(delayTime))
            .Subscribe(_ =>
            {
                count++;
                var rect = this.content.transform.GetComponent<RectTransform>();
                _tween = rect.DOAnchorPosY(rect.anchoredPosition.y + dis, time);
                _tween.onComplete = () => StartGuideDown(5, dis, time);
            });
    }

    /// <summary>
    /// 提示要向右滑动
    /// </summary>
    public void StartGuideRight(float delayTime = 5, float dis = 400, float time = 2f)
    {
        if (count >= _maxGuideCount)
            return;
        _ob = Observable.Timer(TimeSpan.FromSeconds(delayTime))
            .Subscribe(_ =>
            {
                count++;
                var rect = this.content.transform.GetComponent<RectTransform>();
                _tween = rect.DOAnchorPosX(rect.anchoredPosition.x - dis, time);
                _tween.onComplete = () => StartGuideRight(5, dis, time);
            });
    }

    private void PauseGuide()
    {
        _tween?.Kill();
        _ob?.Dispose();
    }

    public void ClearGuide()
    {
        count = 0;
        PauseGuide();
    }

    #endregion

    #region 返回按钮控制

    /// <summary>
    /// 返回按钮对象
    /// </summary>
    private Transform _backBtn;

    /// <summary>
    /// 返回按钮单独淡出
    /// </summary>
    CanvasGroup _backGroup;

    /// <summary>
    /// 定时器
    /// </summary>
    System.IDisposable _backBtnTimer;

    /// <summary>
    /// 上一帧v值
    /// </summary>
    float _vOld = 0;

    public void RegisterBackBtn(GameObject backBtn)
    {
        _backBtn = backBtn.transform;
        _backGroup = _backBtn.GetCanvasGroup();
        _backGroup.alpha = 0;
    }

    /// <summary>
    /// 返回按钮显示控制
    /// </summary>
    void BackBtnControl(float v)
    {
        float f = Mathf.Abs(v - _vOld);
        _vOld = v;
        if (f < 0.0001f)
            return;
        else
        {
            //滑动显示按钮
            if (_backBtn)
                _backBtn.Show();
            else
                return;
            DOTween.To(() => _backGroup.alpha, x => _backGroup.alpha = x, 1, 0.75f);
            //清除计时器
            if (_backBtnTimer != null)
            {
                _backBtnTimer.Dispose();
                _backBtnTimer = null;
            }

            _backBtnTimer = Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ =>
            {
                //Debug.LogError("消失");
                _backBtn.DOKill();
                DOTween.To(() => _backGroup.alpha, x => _backGroup.alpha = x, 0, 0.75f)
                    .onComplete = () => { _backBtn.Hide(); };
            });
        }
    }

    #endregion


    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        _isDrag = true;
        PauseGuide();
        BeginDragCb?.Invoke(eventData);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PauseGuide();
        _ob = null;
        _tween = null;
        _backBtnTimer?.Dispose();
        _backBtnTimer = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //   ClickCb?.Invoke();
        Debug.LogError("dd");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isDrag)
            ClickCb?.Invoke();
        else
            _isDrag = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }
}