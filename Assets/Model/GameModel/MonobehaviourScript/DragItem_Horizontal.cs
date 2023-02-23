/***
 *		Title：              水平拖拽
 *		Description：        只能水平方向上移动        
 *
 *		Data：               2019/4/24
 *		Version：            1.0
 *		Modify Recoder：
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem_Horizontal : DragItemBase
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 更新物品位置
    /// </summary>
    protected override void UpdateItemPosition(PointerEventData eventData)
    {
        var rt = gameObject.GetComponent<RectTransform>();

        //Debug.Log("触碰点 eventData.position = " + eventData.position);
        // 将屏幕点转换为世界点int矩形 (transform the screen point to world point int rectangle)

        Vector2 Pos = LocationLimit_New(eventData.position, ref offset);
        //Debug.Log("触碰点s Pos = " + Pos);

        Vector3 globalMousePos = new Vector3(0, 0, 0);
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Pos/*eventData.position*/, eventData.pressEventCamera,
            out globalMousePos))
        {

        }

        //控制y轴不变
        float ConstY = rt.position.y;
        Vector3 CurPos = globalMousePos + offset;
        CurPos.y = ConstY;
        rt.position = CurPos;
    }



}
