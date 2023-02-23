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

public class BroomBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_Icon; 
	public Image image_yun1; 
	public Image image_yun2; 
	public Image image_yun3; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_Icon = transform.Find("Icon")?.GetComponent<Image>(); 
		image_yun1 = transform.Find("yun1")?.GetComponent<Image>(); 
		image_yun2 = transform.Find("yun2")?.GetComponent<Image>(); 
		image_yun3 = transform.Find("yun3")?.GetComponent<Image>(); 

	}
	#endregion 

}
