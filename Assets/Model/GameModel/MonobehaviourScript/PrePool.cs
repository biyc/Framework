using System.Collections.Generic;
using UnityEngine;

public class PrePool : MonoBehaviour
{
    public List<GameObject> list;

    public GameObject Get(string name)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].name == name)
            {
                return list[i];
            }
        }

        return null;
    }
}