using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfCurve")]
public class SelfCurve : SerializedScriptableObject
{
    public Dictionary<string, AnimationCurve> Curves = new Dictionary<string, AnimationCurve>();
}