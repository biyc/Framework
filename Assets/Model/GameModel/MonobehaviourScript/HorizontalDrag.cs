using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HorizontalDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{


    [SerializeField] protected int ID = 0;                 //物品的id
    [SerializeField] protected float TargetX = 0;            //目標x值
    [SerializeField] protected float BeginX = 0;             //開始x值

    #region 委托 实例化

    public Item_EndDragHandle EndDragHandleEvent;
    public Item_DragHandle DragHandleEvent;
    public Item_BeginDragHandle BeginDragHandleEvent;

    #endregion


    /// <summary>
    /// 下一步 回调
    /// </summary>
    public Action NextCallback;


    protected bool isDrag = true;                         //是否可以拖拽
    public bool IsDrag
    {
        get { return isDrag; }
        set { isDrag = value; }
    }

    private float locY = 0.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateLoc() {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;

        locY = gameObject.GetComponent<RectTransform>().position.y;

        UpdateItemPosition(eventData);

        if (BeginDragHandleEvent != null)
        {
            BeginDragHandleEvent(ID, gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;
        UpdateItemPosition(eventData);
        if (DragHandleEvent != null)
        {
            DragHandleEvent(ID, gameObject);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;

        UpdateItemPosition(eventData);

        if (EndDragHandleEvent != null)
        {
            EndDragHandleEvent(ID, gameObject);
        }
    }

    /// <summary>
    /// 更新物品位置
    /// </summary>
    protected virtual void UpdateItemPosition(PointerEventData eventData)
    {
        var rt = gameObject.GetComponent<RectTransform>();

        // 将屏幕点转换为世界点int矩形 (transform the screen point to world point int rectangle)
        Vector3 globalMousePos = new Vector3(0, 0, 0);
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera,
            out globalMousePos))
        {
            //globalMousePos = LocationLimit(globalMousePos, out offset);
        }
        globalMousePos.y = locY;
        
        Vector3 reLoc = rt.position;

        rt.position = globalMousePos;



        Vector3 localPos = rt.localPosition;

        Debug.Log("rt.localPosition.x =" + rt.localPosition.x);
        if (rt.localPosition.x <= BeginX)
        {
            localPos.x = BeginX;
            rt.localPosition = localPos;
            return;
        }
        else if (rt.localPosition.x >= TargetX)
        {
            //
            localPos.x = TargetX;
            //通知开始下一步
            if (NextCallback != null)
            {
                NextCallback();
                isDrag = false;
            }
            rt.localPosition = localPos;
            return;

        }
    }
}
