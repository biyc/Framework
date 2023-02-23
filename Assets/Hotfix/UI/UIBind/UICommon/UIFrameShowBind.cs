//this file is auto created by QuickCode,you can edit it 
//do not need to care initialization of ui widget any more 
//------------------------------------------------------------------------------
/**
* @author :
* date    :
* purpose :
*/
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

public class UIFrameShowBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Text text_FrameCount; 
	public ContentSizeFitter contentsizefitter_FrameCount; 
	public Text text_Pool; 
	public ContentSizeFitter contentsizefitter_Pool; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		text_FrameCount = transform.Find("FrameCount")?.GetComponent<Text>(); 
		contentsizefitter_FrameCount = transform.Find("FrameCount")?.GetComponent<ContentSizeFitter>(); 
		text_Pool = transform.Find("Pool")?.GetComponent<Text>(); 
		contentsizefitter_Pool = transform.Find("Pool")?.GetComponent<ContentSizeFitter>(); 

	}
	#endregion 

}
