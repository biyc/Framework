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

public class IntroduceWindowBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_bg; 
	public Text text_txt_a2_4; 
	public Image image_closeBtn; 
	public Button button_closeBtn; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_bg = transform.Find("bg")?.GetComponent<Image>(); 
		text_txt_a2_4 = transform.Find("bg/txt_a2_4")?.GetComponent<Text>(); 
		image_closeBtn = transform.Find("bg/closeBtn")?.GetComponent<Image>(); 
		button_closeBtn = transform.Find("bg/closeBtn")?.GetComponent<Button>(); 

	}
	#endregion 

}
