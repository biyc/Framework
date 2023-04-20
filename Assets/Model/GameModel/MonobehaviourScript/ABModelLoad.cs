using System;
using System.Collections;
using System.Collections.Generic;
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

    public async Task LoadObj(string resPath, string name, string baseNetPath = "")
    {
        // Debug.Log("netPath:" + PathHelper.Combine(baseNetPath, name));
        if (_target != null && name == _target.name)
            return;
        if (_target != null)
        {
            var o = _target;
            _target = null;
            UnityEngine.Object.Destroy(o.gameObject);
        }

        _loading.Show();

        _currentName = name;

        if (!await Res.DownLoadModelAsset(resPath, name, baseNetPath))
        {
            _loading.Hide();
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
            _target.localScale = new Vector3(1000, 1000, 1000);
            _target.GetComponentsInChildren<Transform>()
                .ForEach(tr => tr.gameObject.layer = LayerMask.NameToLayer("UI"));
            _recovery?.Invoke();
            _loading.Hide();
        });
    }
}