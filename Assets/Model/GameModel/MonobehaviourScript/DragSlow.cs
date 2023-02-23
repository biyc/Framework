using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSlow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Action<float> callbackUpdate;
    private float min;
    private float max;
    private float total;
    private float delta;
    private float scale = 0.3f;

    private void Awake()
    {
        delta = 0;
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        SyncPositoin(eventData.delta);
    }

    private void SyncPositoin(Vector2 vector2)
    {
        var v = vector2.y * scale;
        if (delta + v > max || delta + v < min)
        {
            return;
        }
        delta += v;
        transform.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, v);
        callbackUpdate?.Invoke(Mathf.Abs((total - delta) / total));
    }

    public void OnEndDrag(PointerEventData eventData) { }

    public void SetData(float min, float max, Action<float> callbackUpdate)
    {
        this.min = min;
        this.max = max;
        this.callbackUpdate = callbackUpdate;
        total = Mathf.Abs(this.max - this.min);
    }

}