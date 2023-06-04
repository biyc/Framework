using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blaze.Resource;
using ETModel;
using Main;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;


public class ABModelLoad : MonoBehaviour
{
    private Transform _loading;
    private Transform _target;
    private Transform _container;


    private const string TARGETTAG = "TARGETTAG";


    /// <summary>
    /// 当前应该显示的资源
    /// </summary>
    private string _currentName;

    private Action _recovery;

    private void Awake()
    {
        _loading = transform.Find("Loading");
        _loading.Show();
        _container = transform.Find("Container");
    }

    public void SetRecovery(Action recovery) => _recovery = recovery;

    public void LoadObjWithFullPath(string resFullPath)
    {
        var name = resFullPath.Split('/').ToList().Last();
        var resPath = resFullPath.Replace(name, "");
        LoadObj(name, resPath, string.Empty, () =>
        {
            if (Define.IsExportProject)
            {
                if (Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.IPhonePlayer)
                    UnityCallAndroid._.OnModelLoadCompleteFun();
            }
        });
    }

    public async Task LoadObj(string name, string resPath, string baseNetPath, Action cb)
    {
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
            _target.localScale = new Vector3(1000, 1000, 1000);
            _target.GetComponentsInChildren<Transform>()
                .ForEach(tr => tr.gameObject.layer = LayerMask.NameToLayer("UI"));
            _recovery?.Invoke();
            _loading.Hide();
            cb?.Invoke();
        });
    }
}