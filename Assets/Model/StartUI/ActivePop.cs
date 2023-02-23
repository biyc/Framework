using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Blaze.Manage.Data;
using Blaze.Resource.AssetBundles;
using Blaze.Resource.Task;
using ETModel;

public class ActivePop : MonoBehaviour
{
    public Text input;
    public Text errInfo;
    public Text title;

    private string link;
    public static ICompleted<ActivePop> Instans = new DataWatch<ActivePop>();

    private void Awake()
    {
        // Debug
        transform.localScale = Vector3.zero;
        Instans.Complet(this);


        // string txt = transform.Find("m_ActivationPanel/m_Activation/InputField/Text").GetComponent<Text>().text;
        // if (txt == null || txt.Equals(""))
        // {
        //     Debug.Log("kkk没有输入任何内容");
        //     //TopPop.Create("没有输入任何内容");
        //     return;
        // }


        // txt1 = transform.Find("txt1").GetComponent<Text>();
    }

    private Action _nextCb;

    /// <summary>
    /// 打开激活界面
    /// </summary>
    /// <param name="nextCb"></param>
    public void OpenActiveation(Action nextCb)
    {
        gameObject.SetActive(true);
        _nextCb = nextCb;
        try
        {
            MonoScheduler.DispatchMain(delegate { nextCb(); });
            return;
            
        }
        catch (Exception e)
        {
        }

        OpenAnimation();
    }


    // 点击激活按钮
    public void Ok()
    {

    }

    /// <summary>
    /// 获取激活码
    /// </summary>
    public void GetActiveCode()
    {
        if (link != null)
            Application.OpenURL(link);
        // Close();
    }


    void OpenAnimation()
    {
        var _aniTweener = transform.DOScale(1f, 0.3f);
        _aniTweener.SetEase(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.8f, 1.1f), new Keyframe(1, 1)));
        _aniTweener.SetAutoKill(false);

        // DOTween.To(
        //     value => transform.Find("m_ActivationPanel/m_Activation").GetComponent<RectTransform>().localScale =
        //         new Vector3(value, value, 1), 0, 1, 0.15f);
    }

    void Close()
    {
        transform.localScale = Vector3.zero;
    }
}