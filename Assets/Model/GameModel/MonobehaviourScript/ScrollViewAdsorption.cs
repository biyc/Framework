using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 滑块吸附效果
/// </summary>
public class ScrollViewAdsorption : MonoBehaviour
{
    /// <summary>
    /// 吸附对象
    /// </summary>
    [Header("吸附对象")]
    public List<Transform> Objs = new List<Transform>();

    /// <summary>
    /// 滑块对象
    /// </summary>
    ScrollRect Rect;
    /// <summary>
    /// 滑块宽度
    /// </summary>
    float Content;
    /// <summary>
    /// 每个对象滑块值
    /// </summary>
    List<float> NormalizedPositions = new List<float>();
    /// <summary>
    /// 目标页的NormalizedPositon的值
    /// </summary>
    float TargetNUPos;
    /// <summary>
    /// 是否滑动
    /// </summary>
    bool IsDrag=true;
    /// <summary>
    /// 缓动到目标页的持续时间
    /// </summary>
    [Header("缓动到目标页的持续时间")]
    public float MoveTimes = 0.5f;
    /// <summary>
    /// 比例系数
    /// </summary>
    private float coe;
    // Start is called before the first frame update
    void Start()
    {
        Rect= GetComponent<ScrollRect>();
        //计算每一个对象的的NormalizedPositon值
        //获取Content宽度
        Content = transform.Find("Viewport/Content").GetComponent<RectTransform>().rect.width;
        //for(int i=0;i<Objs.Count;i++)
        //{
        //    //算出以左边为原点的新坐标
        //    float x = Objs[i].transform.localPosition.x - (-Content / 2f);
        //    NormalizedPositions.Add(x / Content);
        //    Debug.LogError(NormalizedPositions[i]);
        //}
        NormalizedPositions.Add(0);
        NormalizedPositions.Add(0.43f);
        NormalizedPositions.Add(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDrag)
        {
            return;
        }
        if(Mathf.Abs(Rect.horizontalNormalizedPosition- TargetNUPos)<0.01)
        {
            Rect.horizontalNormalizedPosition = TargetNUPos;
            return;
        }
        coe += Time.deltaTime / MoveTimes;
        Rect.horizontalNormalizedPosition = Mathf.Lerp(Rect.horizontalNormalizedPosition, TargetNUPos, coe);
    }

    /// <summary>
    /// 拖拽动作
    /// </summary>
    public void StartDrag()
    {
        IsDrag = true;
    }


    /// <summary>
    /// 抬手动作
    /// </summary>
    public void EndDrag()
    {
        IsDrag = false;
        coe = 0;
        float h = Rect.horizontalNormalizedPosition;
        Dictionary<Transform, float> targetDistance = new Dictionary<Transform, float>();
        for (int i=0; i < Objs.Count;i++)
        {
            targetDistance.Add(Objs[i], Mathf.Abs(NormalizedPositions[i] - h));
        }
        List<KeyValuePair<Transform, float>> lst = new List<KeyValuePair<Transform, float>>(targetDistance);
        //倒叙排列：只需要把变量s2 和 s1 互换就行了 例： return s1.Value.CompareTo(s2.Value);
        //进行排序 目前是顺序
        lst.Sort(delegate (KeyValuePair<Transform, float> s1, KeyValuePair<Transform, float> s2)
        {
            return s1.Value.CompareTo(s2.Value);
        });
        targetDistance.Clear();       
        //选出最近一个
        foreach (var item in lst)
        {
            int i = Objs.IndexOf(item.Key);
            TargetNUPos = NormalizedPositions[i];
            return;
        }

    }

}
