using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Blaze.Manage.Data;
using Blaze.Resource.AssetBundles;
using ETModel;
using UniRx;

public class UpDatePop : MonoBehaviour
{
    Text txt1;

    public static ICompleted<UpDatePop> Instans = new DataWatch<UpDatePop>();
    private void Awake()
    {
        txt1 = transform.Find("txt1").GetComponent<Text>();
        Instans.Complet(this);
        BundleHotfix._.OnTip += delegate(string tip, Action cb, Action failCb)
        {
            Message(cb, failCb);
            ShowResUpdate(tip);

        };
    }

    public void Message( Action okCb, Action failCb)
    {
        transform.Find("Cancel").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("Cancel").GetComponent<Button>().onClick.AddListener(() =>
        {
            Close();
            failCb?.Invoke();
        });
        transform.Find("Close").GetComponent<Button>().onClick.AddListener(() =>
        {
            Close();
            failCb?.Invoke();
        });
        transform.Find("Ok").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("Ok").GetComponent<Button>().onClick.AddListener(() =>
        {
            Close();
            okCb?.Invoke();
        });
    }

    /// <summary>
    /// 资源更新提示
    /// </summary>
    public void ShowResUpdate(string txt)
    {
        OpenAnimation();
        txt1.alignment = TextAnchor.MiddleLeft;
        //txt1.text = $"检测到约为{MB}MB的资源更新，共{fileCount}个文件，点击更新进行下载\n<size=33>当前为移动网络环境，建议使用WI-F网络进行下载</size>";
        // txt1.text = txt + "\n<size=33>当前为移动网络环境，建议使用WI-F网络进行下载</size>";
        txt1.text = txt;
        transform.Find("Ok/Text").GetComponent<Text>().text = "确认";
    }

    /// <summary>
    /// 版本更新提示
    /// </summary>
    public void ShowVersionUpdate(string txt)
    {
        OpenAnimation();
        txt1.text = txt;
        txt1.alignment = TextAnchor.MiddleCenter;
        transform.Find("Ok/Text").GetComponent<Text>().text = "确认";
    }

    void OpenAnimation()
    {
        var _aniTweener = transform.DOScale(1f, 0.3f);
        _aniTweener.SetEase(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.8f, 1.1f), new Keyframe(1, 1)));
        _aniTweener.SetAutoKill(false);
    }

    void Close()
    {
        transform.localScale = Vector3.zero;
    }
}