using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��������Ч��
/// </summary>
public class ScrollViewAdsorption : MonoBehaviour
{
    /// <summary>
    /// ��������
    /// </summary>
    [Header("��������")]
    public List<Transform> Objs = new List<Transform>();

    /// <summary>
    /// �������
    /// </summary>
    ScrollRect Rect;
    /// <summary>
    /// ������
    /// </summary>
    float Content;
    /// <summary>
    /// ÿ�����󻬿�ֵ
    /// </summary>
    List<float> NormalizedPositions = new List<float>();
    /// <summary>
    /// Ŀ��ҳ��NormalizedPositon��ֵ
    /// </summary>
    float TargetNUPos;
    /// <summary>
    /// �Ƿ񻬶�
    /// </summary>
    bool IsDrag=true;
    /// <summary>
    /// ������Ŀ��ҳ�ĳ���ʱ��
    /// </summary>
    [Header("������Ŀ��ҳ�ĳ���ʱ��")]
    public float MoveTimes = 0.5f;
    /// <summary>
    /// ����ϵ��
    /// </summary>
    private float coe;
    // Start is called before the first frame update
    void Start()
    {
        Rect= GetComponent<ScrollRect>();
        //����ÿһ������ĵ�NormalizedPositonֵ
        //��ȡContent���
        Content = transform.Find("Viewport/Content").GetComponent<RectTransform>().rect.width;
        //for(int i=0;i<Objs.Count;i++)
        //{
        //    //��������Ϊԭ���������
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
    /// ��ק����
    /// </summary>
    public void StartDrag()
    {
        IsDrag = true;
    }


    /// <summary>
    /// ̧�ֶ���
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
        //�������У�ֻ��Ҫ�ѱ���s2 �� s1 ���������� ���� return s1.Value.CompareTo(s2.Value);
        //�������� Ŀǰ��˳��
        lst.Sort(delegate (KeyValuePair<Transform, float> s1, KeyValuePair<Transform, float> s2)
        {
            return s1.Value.CompareTo(s2.Value);
        });
        targetDistance.Clear();       
        //ѡ�����һ��
        foreach (var item in lst)
        {
            int i = Objs.IndexOf(item.Key);
            TargetNUPos = NormalizedPositions[i];
            return;
        }

    }

}
