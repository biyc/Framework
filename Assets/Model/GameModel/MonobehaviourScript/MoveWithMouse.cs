using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveWithMouse : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    private Vector3 temp;
    public float value;
    public float target;
    private Action callbackComplete;
    private Action<float> callbackUpdate;
    private Action callbackDragEnd;
    private Action callbackDragBegin;
    private bool flag = false;

    private void Start()
    {
        target = 100f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (flag)
        {
            return;
        }
        if (eventData == null || eventData.pointerEnter == null) return;

        temp = new Vector3(eventData.position.x, eventData.position.y, 0);
        if (eventData.pointerEnter.gameObject.name.Equals("Left"))
        {
            if (temp.x - startPos.x >= target)
            {
                EndDrag();
            }
        }
        else if (eventData.pointerEnter.gameObject.name.Equals("Right"))
        {
            if (startPos.x - temp.x >= target)
            {
                EndDrag();
            }
        }
        else if (eventData.pointerEnter.gameObject.name.Equals("Top"))
        {
            if (startPos.y - temp.y >= target)
            {
                EndDrag();
            }
        }
        else if (eventData.pointerEnter.gameObject.name.Equals("Bottom"))
        {
            if (temp.y - startPos.y >= target)
            {
                EndDrag();
            }
        }
        //startPos = temp;
        //callbackUpdate?.Invoke(value / target);
    }

    private void EndDrag()
    {
        callbackComplete?.Invoke();
        callbackComplete = null;
        callbackUpdate = null;
        flag = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
        callbackDragBegin?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
    public void RegisterData(float target, Action callbackComplete, Action callbackDragBegin = null, Action<float> callbackUpdate = null)
    {
        this.target = target;
        this.callbackComplete = callbackComplete;
        this.callbackDragBegin = callbackDragBegin;
        this.callbackUpdate = callbackUpdate;
    }
}