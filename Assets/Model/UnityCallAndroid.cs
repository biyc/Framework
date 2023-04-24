using System.Collections;
using System.Collections.Generic;
using Blaze.Core;
using UnityEngine;

public class UnityCallAndroid : Singeton<UnityCallAndroid>
{
    AndroidJavaClass jc;

    AndroidJavaObject jo;
#if UNITY_IOS
    [DllImport("__Internal")] static extern void OnUnityInitialCompleteFun_iOS();
    [DllImport("__Internal")] static extern void OnModelLoadCompleteFun_iOS();
#endif


    /// <summary>
    /// Android初始化Unity完成回调
    /// </summary>
    public void OnUnityInitialCompleteFun()
    {
        Debug.Log("初始化Unity完成回调");
#if UNITY_IOS
        OnUnityInitialCompleteFun_iOS();
#elif UNITY_ANDROID
        NewJavaObject();
        jo.Call("onUnityInitializedFunction");
#endif
    }

    /// <summary>
    /// Android初始化Unity完成回调
    /// </summary>
    public void OnModelLoadCompleteFun()
    {
        Debug.Log("模型加载完成回调");
#if UNITY_IOS
        OnModelLoadCompleteFun_iOS();
#elif UNITY_ANDROID
        NewJavaObject();
        jo.Call("onModelLoadCompleteFunction");
#endif
    }

    private void NewJavaObject()
    {
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }
}