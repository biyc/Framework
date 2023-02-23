using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DecorateItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Vector3 targetPosition;
    private Action<Vector3> callbackOnDrag;
    private Action<Vector3> callbackOnEndDrag;
    private Action callbackOnBeginDrag;
    private bool flag;
    private Vector3 offset;
    public Func<GameObject, Vector3, Vector3> funcOutOfEdge;//超出边界时，返回修正过后的位置
    public Func<GameObject, Vector3, bool> syncDragPosition;
    public float scaleParam = 1.0f;

    private bool AllowSync(PointerEventData eventData)
    {
        var sync = true;
        if (syncDragPosition != null)
        {
            var selfRectTransform = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(selfRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPosition);
            sync = syncDragPosition.Invoke(gameObject, localPosition);
        }
        return sync;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var rt = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector2 localPosition);
        if (isRect)
        {
            //计算图片中心和鼠标点的差值
            offset = gameObject.GetComponent<RectTransform>().localPosition - new Vector3(localPosition.x, localPosition.y, 0) / scaleParam;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        flag = true;
        if (AllowSync(eventData))
        {
            SyncDraggedGameobjectPosition(eventData);
        }
        callbackOnBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (AllowSync(eventData))
        {
            SyncDraggedGameobjectPosition(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (AllowSync(eventData))
        {
            SyncDraggedGameobjectPosition(eventData);
        }
        if (flag)
        {
            callbackOnEndDrag?.Invoke(targetPosition);
            flag = false;
        }
    }

    private void SyncDraggedGameobjectPosition(PointerEventData eventData)
    {
        var selfRectTransform = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(selfRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPosition))
        {
            targetPosition = new Vector3(localPosition.x, localPosition.y, 0) / scaleParam + offset;
            Debug.LogWarning($"同步位置前 eventPosition:{eventData.position},localPosition:{localPosition},targetPosition:{targetPosition},offset:{offset}");
            if (funcOutOfEdge != null)
            {
                targetPosition = funcOutOfEdge.Invoke(gameObject, targetPosition);
            }
            Debug.LogWarning($"同步位置后 eventPosition:{eventData.position},localPosition:{localPosition},targetPosition:{targetPosition},offset:{offset}");
            transform.GetComponent<RectTransform>().localPosition = targetPosition;
            callbackOnDrag?.Invoke(transform.GetComponent<RectTransform>().localPosition);
        }
    }

    public void RegisterCallback(Action<Vector3> callbackOnEndDrag, Func<GameObject, Vector3, Vector3> funcOutOfEdge, Action callbackOnBeginDrag, Action<Vector3> callbackOnDrag)
    {
        this.callbackOnEndDrag = callbackOnEndDrag;
        this.funcOutOfEdge = funcOutOfEdge;
        this.callbackOnBeginDrag = callbackOnBeginDrag;
        this.callbackOnDrag = callbackOnDrag;
    }
}