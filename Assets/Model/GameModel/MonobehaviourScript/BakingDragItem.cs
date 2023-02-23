using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BakingDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler
{
    [SerializeField] protected int ID = 0;                                          //物品的id
    [SerializeField] protected string TargetTag = "Baking";                               //目标标记
    [SerializeField] protected string destryTag = "MakingTrash";                      //销毁标记
    [SerializeField] protected RectTransform rectTransform = new RectTransform();         //可活动范围 中心点对齐

    /// <summary>
    /// 目标父类
    /// </summary>
    [SerializeField] public Transform targetParent;
    [SerializeField] public Camera uiCamera;

    protected Vector3 offset = new Vector3();             //用来得到鼠标和图片的差值        
    /// <summary>
    /// 设置最上层
    /// </summary>
    private void SetTopLayer()
    {
        int count = gameObject.transform.parent.childCount - 1;//Panel移位
        gameObject.transform.SetSiblingIndex(count);//Panel移位
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var rt = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        bool isRect = RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector3 worldPoint);
        if (isRect)
        {
            //计算图片中心和鼠标点的差值
            offset = transform.GetComponent<RectTransform>().position - worldPoint;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //关闭自己的射线遮挡
        gameObject.transform.GetComponent<Image>().raycastTarget = false;
        SyncDraggedGameobjectPosition(eventData);
        SetTopLayer();
    }

    public void OnDrag(PointerEventData eventData)
    {        
        SyncDraggedGameobjectPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //关闭自己的射线遮挡
        gameObject.transform.GetComponent<Image>().raycastTarget = true;

        if (!string.IsNullOrEmpty(destryTag) && eventData.pointerEnter != null)
        {
            //放入垃圾桶
            if (eventData.pointerEnter.tag == destryTag)
            {
                Destroy(gameObject);
            }
            else
            {
                SyncDraggedGameobjectPosition(eventData);
            }
        }
    }
    private void SyncDraggedGameobjectPosition(PointerEventData eventData)
    {
        var selfRectTransform = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(selfRectTransform, eventData.position, eventData.pressEventCamera, out Vector3 worldPoint))
        {
            var targetPosition = worldPoint + offset;
            transform.GetComponent<RectTransform>().position = FuncOutOfEdge(gameObject, targetPosition);
        }
    }
    private Vector3 FuncOutOfEdge(GameObject gameobject, Vector3 vec)
    {
        Transform trans = null;
        float minX = 0;
        float minY = 0;
        float maxX = 0;
        float maxY = 0;
        var canvas = gameobject.GetComponentInParent<Canvas>();
        trans = gameobject.transform;
        if (trans != null)
        {
            float scaler = 0;
            var match = gameObject.GetComponentInParent<CanvasScaler>().matchWidthOrHeight;
            if (match == 0)
            {
                scaler = Screen.width / 1242.0f;
            }
            else if (match == 1)
            {
                scaler = Screen.height / 2688.0f;
            }

            var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(canvas.transform, trans);
            minX = bounds.extents.x * scaler;
            minY = bounds.extents.y * scaler + 500;
            maxX = Screen.width - bounds.extents.x * scaler;
            maxY = Screen.height - bounds.extents.y * scaler;            
        }
        if (trans != null)
        {
            var screenPoint = uiCamera.WorldToScreenPoint(vec);
            var x = screenPoint.x;
            var y = screenPoint.y;
            x = Mathf.Min(x, maxX);
            x = Mathf.Max(x, minX);
            y = Mathf.Min(y, maxY);
            y = Mathf.Max(y, minY);
            var worldPoint = uiCamera.ScreenToWorldPoint(new Vector3(x, y, 0));
            return worldPoint;
        }
        return vec;
    }
}
