using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blaze.Resource;
using Main;
using Sirenix.Utilities;
using UnityEngine;


public class ABModelLoad : MonoBehaviour
{
    private Transform _loading;
    private Transform _target;
    private Transform _container;


    private const string TARGETTAG = "TARGETTAG";
    private const float STANDSCREENHEIGHT = 2688;
    private const float STANDSCREENWIDTH = 1242;

    /// <summary>
    /// 当前应该显示的资源
    /// </summary>
    private string _currentName;

    private Action _recovery;

    private void Awake()
    {
        _loading = transform.Find("Loading");
        _container = transform.Find("Container");
    }

    public void SetRecovery(Action recovery) => _recovery = recovery;

    public void LoadObjWithFullPath(string resFullPath)
    {
        var name = resFullPath.Split('/').ToList().Last();
        var resPath = resFullPath.Replace(name, "");
        LoadObj(name, resPath, string.Empty, () =>
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                UnityCallAndroid._.OnModelLoadCompleteFun();
        });
    }

    public async Task LoadObj(string name, string resPath, string baseNetPath, Action cb)
    {
        // Debug.Log("netPath:" + PathHelper.Combine(baseNetPath, name));
        if (_target != null && name == _target.name)
        {
            cb?.Invoke();
            return;
        }

        if (_target != null)
        {
            var o = _target;
            _target = null;
            UnityEngine.Object.Destroy(o.gameObject);
        }

        _loading.Show();

        _currentName = name;

        if (!await Res.DownLoadModelAsset(name, resPath, baseNetPath))
        {
            _loading.Hide();
            cb?.Invoke();
            return;
        }

        var path = $"Assets/Projects/3d/Models/{name}/{name}.fbx";

        // await LoadTarget(assetPath);
        //.prefab  fbx


        var task = Res.InstantiateAsync(path, _container);
        task.GetAwaiter().OnCompleted(() =>
        {
            var m = task.Result;
            //在下载过程中点击了其他的物品
            if (_currentName != name)
            {
                UnityEngine.Object.Destroy(m.Target);
                return;
            }

            _target = m.Target.transform;
            _target.name = name;
            _target.tag = TARGETTAG;
            var box = _target.gameObject.AddComponent<BoxCollider>();
            var scaleZFactor = 1 / box.size.z;
            var scaleXFactor = 0.5f / box.size.x;
            if (box.size.z > 1)
                Debug.Log("该模型大于了最大高度1,强制缩放为高度1,缩放比：" + scaleZFactor);
            if (box.size.x > 0.5f)
                Debug.Log("该模型大于了最大宽度0.5,强制缩放为高度0.5,缩放比：" + scaleXFactor);
            Debug.Log("实际缩放比：" + Mathf.Min(scaleZFactor, scaleXFactor));

            var screenYScaleFactor = Screen.height / STANDSCREENHEIGHT;
            var screenXScaleFactor = Screen.width / STANDSCREENWIDTH;
            Debug.Log("屏幕缩放：" + Mathf.Min(screenYScaleFactor, screenXScaleFactor));
            _target.localScale = new Vector3(1000, 1000, 1000) * Mathf.Min(scaleZFactor, scaleXFactor);
                                //* Mathf.Min(screenYScaleFactor, screenXScaleFactor);
            GameObject.Destroy(box);
            //_target.localScale = new Vector3(1000, 1000, 1000);
            _target.GetComponentsInChildren<Transform>()
                .ForEach(tr => tr.gameObject.layer = LayerMask.NameToLayer("UI"));
            _recovery?.Invoke();
            _loading.Hide();
            cb?.Invoke();
        });
    }
}