/***
 *		Title：          拖拽物品基类
 *		Description：
 *
 *		Data：           2019/4/9
 *		Version：        1.0
 *		Modify Recoder：
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

#region 定义委托

/// <summary>
/// 结束拖拽
/// </summary>
/// <param name="id"></param>
public delegate void Item_EndDragHandle(int id, GameObject Item);

/// <summary>
/// 拖拽中
/// </summary>
/// <param name="id"></param>
public delegate void Item_DragHandle(int id, GameObject Item);

/// <summary>
/// 拖拽开始
/// </summary>
/// <param name="id"></param>
public delegate void Item_BeginDragHandle(int id, GameObject Item);

#endregion


public class DragItemBase : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] protected int ID = 0;                 //物品的id
    [SerializeField] protected string TargetTag = "";      //目标标记


    protected Vector3 offset = new Vector3();             //用来得到鼠标和图片的差值

    #region 委托 实例化

    /// <summary>
    /// 进入目标区域
    /// </summary>
    public Action<int> EnterTargetAreaComeback;
    public Action<int> LeaveTargetAreaComeback;
    

    public Item_EndDragHandle EndDragHandleEvent;
    public Item_DragHandle DragHandleEvent;
    public Item_BeginDragHandle BeginDragHandleEvent;

    #endregion


    protected bool isDrag = true;                         //是否可以拖拽
    public bool IsDrag
    {
        get { return isDrag; }
        set { isDrag = value; }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;
        GetOffset(eventData);
        UpdateItemPosition(eventData);

        if (!string.IsNullOrEmpty(TargetTag)) {
            gameObject.GetComponent<Image>().raycastTarget = false;
        }

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

        if (!string.IsNullOrEmpty(TargetTag))
        {
            gameObject.GetComponent<Image>().raycastTarget = true;
            if (eventData.pointerEnter.tag == TargetTag)
            {
                Debug.Log("进入目标区域！！！");
                if (EnterTargetAreaComeback != null)
                    EnterTargetAreaComeback(ID);
            }
            else {
                if (LeaveTargetAreaComeback != null)
                    LeaveTargetAreaComeback(ID);
            }
                
        }

        if (EndDragHandleEvent != null)
        {
            EndDragHandleEvent(ID, gameObject);
        }

    }

    /// <summary>
    /// 计算偏移量
    /// </summary>
    /// <param name="eventData"></param>
    protected void GetOffset(PointerEventData eventData)
    {
        var rt = gameObject.GetComponent<RectTransform>();
        Vector3 mouseUguiPos = new Vector3();   //定义一个接收返回的ugui坐标
        bool isRect = RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out mouseUguiPos);
        offset = Vector3.zero;
        if (isRect)   //如果在
        {
            //计算图片中心和鼠标点的差值
            offset = rt.position - mouseUguiPos;
        }
    }


    /// <summary>
    /// 更新物品位置
    /// </summary>
    protected virtual void UpdateItemPosition(PointerEventData eventData)
    {
        var rt = gameObject.GetComponent<RectTransform>();

        //Debug.Log("触碰点 eventData.position = "+ eventData.position);

        Vector2 Pos = LocationLimit_New(eventData.position, ref offset);

        // 将屏幕点转换为世界点int矩形 (transform the screen point to world point int rectangle)
        Vector3 globalMousePos = new Vector3(0, 0, 0);
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Pos, eventData.pressEventCamera,
            out globalMousePos))
        {
            //globalMousePos = LocationLimit(globalMousePos, out offset);
        }

        rt.position = globalMousePos + offset;
    }

    /// <summary>
    /// 限制位置在屏幕范围内
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    protected virtual Vector3 LocationLimit(Vector3 pos, out Vector3 item_Offset)
    {
        float pivotX = gameObject.GetComponent<RectTransform>().pivot.x;
        float pivotY = gameObject.GetComponent<RectTransform>().pivot.y;
        float sizeDeltaX = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float sizeDeltaY = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        float minX = pivotX * sizeDeltaX;
        float minY = pivotY * sizeDeltaY;
        float maxX = Screen.width - (sizeDeltaX - minX);
        float maxY = Screen.height - (sizeDeltaY - minY);

        item_Offset = new Vector3();
        if (pos.y < minY)
        {
            pos.y = minY;
            item_Offset.y = 0f;
        }
        else if (pos.y > maxY)
        {
            pos.y = maxY;
            item_Offset.y = 0f;
        }

        if (pos.x < minX)
        {
            pos.x = minX;
            item_Offset.x = 0f;
        }
        else if (pos.x > maxX)
        {
            pos.x = maxX;
            item_Offset.x = 0f;
        }

        return pos;
    }

    /// <summary>
    /// 限制位置在屏幕范围内宽 1/10 高1/20
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="item_Offset"></param>
    /// <returns></returns>
    protected Vector2 LocationLimit_New(Vector2 pos, ref Vector3 item_Offset)
    {
        float minX = Screen.width / 10.0f;
        float maxX = Screen.width - minX;

        float minY = Screen.height / 20.0f;
        float maxY = Screen.height - minY;

        if (pos.x < minX)
        {
            pos.x = minX;
            item_Offset.y = 0;
        }
        else if (pos.x > maxX)
        {
            pos.x = maxX;
            item_Offset.y = 0;
        }

        if (pos.y < minY)
        {
            pos.y = minY;
            item_Offset.x = 0;
        }
        else if (pos.y > maxY)
        {
            pos.y = maxY;
            item_Offset.x = 0;
        }

        return pos;
    }

}
