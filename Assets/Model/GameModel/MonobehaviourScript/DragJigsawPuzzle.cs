using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragJigsawPuzzle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Action<PointerEventData> actionDragBegin;
    public Action<PointerEventData> actionDragEnd;
    public Func<GameObject, Vector3, Vector3> funcOutOfEdge;//超出边界时，返回修正过后的位置

    Vector3 offset;
    public enum DraggedType
    {
        Self, Parent
    }
    public DraggedType draggedType = DraggedType.Self;
    private void SyncDraggedGameobjectPosition(PointerEventData eventData)
    {
        var selfRectTransform = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(selfRectTransform, eventData.position, eventData.pressEventCamera, out Vector3 worldPoint))
        {
            //Debug.Log($"screen point:{eventData.position},world point:{globalMousePos}");
            var targetPosition = worldPoint + offset;
            if (funcOutOfEdge != null)
            {
                targetPosition = funcOutOfEdge.Invoke(gameObject, targetPosition);
            }
            switch (draggedType)
            {
                case DraggedType.Self:
                    transform.GetComponent<RectTransform>().position = targetPosition;
                    break;
                case DraggedType.Parent:
                    transform.parent.GetComponent<RectTransform>().position += (targetPosition - transform.GetComponent<RectTransform>().position);
                    break;
                default:
                    break;
            }
        }
    }

    public bool Dragging { get; private set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Dragging = true;
        SyncDraggedGameobjectPosition(eventData);
        actionDragBegin?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SyncDraggedGameobjectPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Dragging = false;
        SyncDraggedGameobjectPosition(eventData);
    }

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
}
