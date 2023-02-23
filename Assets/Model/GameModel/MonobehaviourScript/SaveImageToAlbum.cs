using System.Collections;
using System.IO;
using UnityEngine;
#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif
public class SaveImageToAlbum : MonoBehaviour
{
#if UNITY_IPHONE     //与调用ios 里面的保存相册接口
    // [DllImport("__Internal")]
    // private static extern void _SavePhoto(string readAddr);
    // [DllImport("__Internal")]
    // private static extern void _OpenUrl(string url);
#endif

    //下载完成
    private System.Action<string> callbackWithPath;

    public void SaveTexture2Album(Texture2D t, System.Action<string> callbackWithPath = null)
    {
        this.callbackWithPath = callbackWithPath;
        StartCoroutine(getTexture2d(t));
    }

    //传的参数是自己获取的图片
    private IEnumerator getTexture2d(Texture2D t)
    {
        //截图操作  
        yield return new WaitForEndOfFrame();

        byte[] bytes = t.EncodeToPNG();
        t.Compress(true);
        t.Apply();

        //获取系统时间  
        var timeNow = System.DateTime.Now;
        string filename = string.Format("image{0}{1}{2}{3}.png", timeNow.Day, timeNow.Hour, timeNow.Minute, timeNow.Second);
        string Path_save = "";
        string destination = "";
        //应用平台判断，路径选择  
        if (Application.platform == RuntimePlatform.Android)
        {
            destination = "/mnt/sdcard/DCIM/Screenshots";
            //判断路径是否存在
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            destination = destination + "/" + filename;
            Path_save = destination;
            //保存文件  
            Debug.Log("路径：" + Path_save);
            File.WriteAllBytes(Path_save, bytes);

            //// 安卓在这里需要去 调用原生的接口去 刷新一下，不然相册显示不出来
            using (AndroidJavaClass playerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = playerActivity.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    Debug.Log("scanFile:m_androidJavaObject ");
                    jo.Call("scanFile", Path_save);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            destination = Application.persistentDataPath;
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            destination = destination + "/" + filename;
            Path_save = destination;
            //保存文件    ios需要在本地保存文件后，然后将保存的图片的路径 传给ios 
            Debug.Log("路径保存：" + Path_save);
            File.WriteAllBytes(Path_save, bytes);
#if UNITY_IPHONE
            // _SavePhoto(Path_save);
#endif
        }

        callbackWithPath?.Invoke(Path_save);
        callbackWithPath = null;
    }

    public void OpenUrl(string url)
    {
#if UNITY_IPHONE
            // _OpenUrl(url);
#endif
    }

}
