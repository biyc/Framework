using UnityEngine;
using UnityEngine.UI;

public class Fps_Display : MonoBehaviour
{

    //上一次更新帧率的时间;  
    private float m_LastUpdateShowTime = 0f;
    //更新帧率的时间间隔; 
    private float m_UpdateShowDeltaTime = 0.1f;
    //帧数;
    private int m_FrameUpdate = 0;
    private float m_FPS = 0;


    private System.DateTime firetime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

    private GUIStyle bb;

    [Range(0.1f, 60)]
    public float UpdateTime = 1;
    [Range(0, 150)]
    public int fontsize = 40;

    public Color fontcolor = new Color(1, 1, 0);

    public Text FPS_display;

    //是否打开帧数显示
    bool bOpenFPSDisplay = true;
    void Awake()
    {
        bb = new GUIStyle();
        //这是设置背景填充的  
        bb.normal.background = null;
        //设置字体颜色的    
        bb.normal.textColor = fontcolor;
        //当然，这是字体颜色   
        bb.fontSize = fontsize;
    }

    // Use this for initialization  
    void Start()
    {
        m_LastUpdateShowTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame  
    void Update()
    {
        if (!bOpenFPSDisplay)
        {
            return;
        }
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 120, 200, 200), "FPS：" + m_FPS, bb);
    }

}
