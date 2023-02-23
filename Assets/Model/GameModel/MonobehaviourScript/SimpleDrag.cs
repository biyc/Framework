using System;
using UnityEngine;
using UnityEngine.EventSystems;

//只要有拖拽就分发事件
public class SimpleDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Action callback;
    private Action callbackOnBeginDrag;
    private bool flag;
    private Vector3 offset;
    public Func<GameObject, Vector3, Vector3> funcOutOfEdge;//超出边界时，返回修正过后的位置

    public void OnPointerDown(PointerEventData eventData)
    {
        var rt = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        bool isRect = RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector3 worldPoint);
        if (isRect)
        {
            //计算图片中心和鼠标点的差值
            offset = gameObject.GetComponent<RectTransform>().position - worldPoint;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        flag = true;
        SyncDraggedGameobjectPosition(eventData);
        callbackOnBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SyncDraggedGameobjectPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SyncDraggedGameobjectPosition(eventData);
        if (flag)
        {
            callback?.Invoke();
            flag = false;
        }
    }

    private void SyncDraggedGameobjectPosition(PointerEventData eventData)
    {
        var selfRectTransform = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(selfRectTransform, eventData.position, eventData.pressEventCamera, out Vector3 worldPoint))
        {
            var targetPosition = worldPoint + offset;
            if (funcOutOfEdge != null)
            {
                targetPosition = funcOutOfEdge.Invoke(gameObject, targetPosition);
            }
            //Debug.Log($"screen point:{eventData.position},world point:{globalMousePos}");
            transform.GetComponent<RectTransform>().position = targetPosition;
        }
    }

    public void RegisterCallback(Action callback, Func<GameObject, Vector3, Vector3> funcOutOfEdge, Action callbackOnBeginDrag)
    {
        this.callback = callback;
        this.funcOutOfEdge = funcOutOfEdge;
        this.callbackOnBeginDrag = callbackOnBeginDrag;
    }
}
