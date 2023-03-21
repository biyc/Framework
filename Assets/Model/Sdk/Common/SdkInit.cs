using UnityEngine;

public class SdkInit : MonoBehaviour
{
    void Awake()
    {
        // 关闭收集工具
        UnityEngine.Analytics.Analytics.enabled = false;
        UnityEngine.Analytics.Analytics.deviceStatsEnabled = false;
        UnityEngine.Analytics.Analytics.initializeOnStartup = false;
        UnityEngine.Analytics.Analytics.limitUserTracking = false;
        UnityEngine.Analytics.PerformanceReporting.enabled = false;
    }

    void Start()
    {
    }
}