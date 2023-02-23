
using UnityEngine;

namespace App.Blaze.Utility.Helper
{
    public class FindHelper
    {
        /// <summary>
        /// 查找场景中所有的物体
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static GameObject FindChild(string objName)
        {
            GameObject go = null;
            foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.name == objName)
                {
                    go = obj;
                    return go;
                }
            }
            return go;
        }
        
        /// <summary>
        ///递归查找子物体
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static GameObject FindChild(Transform trans, string childName)
        {
            Transform child = trans.Find(childName);
            if (child != null)
            {
                return child.gameObject;
            }
            int count = trans.childCount;
            GameObject go = null;
            for (int i = 0; i < count; ++i)
            {
                child = trans.GetChild(i);
                go = FindChild(child, childName);
                if (go != null)
                    return go;
            }
            return null;
        }
        
        /// <summary>
        /// 根据名字在父类查找子物体
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="childName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindChild<T>(Transform trans, string childName) where T : Component
        {
            GameObject go = FindChild(trans, childName);
            if (go == null)
                return null;
            return go.GetComponent<T>();
        }

        /// <summary>
        /// 查找父物体下的所有子物体再修改 layer
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="layName"></param>
        /// <returns></returns>
        public static void GetAllChild(Transform parent,string layName)
        {
            Transform trans;
            if (parent.childCount < 0) return;
            for (int i = 0; i < parent.childCount; i++)
            {
                trans = parent.GetChild(i);
                trans.gameObject.layer = LayerMask.NameToLayer(layName);
                GetAllChild(trans,layName);
            }
        }
    }
}