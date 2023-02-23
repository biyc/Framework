using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragEndEvent : MonoBehaviour, IEndDragHandler, IDragHandler
{
    private Action callback;

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        callback?.Invoke();
    }

    public void RegisterEvent(Action callback)
    {
        this.callback = callback;
    }
}
