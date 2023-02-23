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

public class PostManBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_PostMan; 
	public Button button_PostMan; 
	public Image image_Icon0; 
	public Image image_Icon1; 
	public Image image_Icon2; 
	public Image image_Icon3; 
	public Image image_qipao; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_PostMan = transform.GetComponent<Image>(); 
		button_PostMan = transform.GetComponent<Button>(); 
		image_Icon0 = transform.Find("Icon0")?.GetComponent<Image>(); 
		image_Icon1 = transform.Find("Icon1")?.GetComponent<Image>(); 
		image_Icon2 = transform.Find("Icon2")?.GetComponent<Image>(); 
		image_Icon3 = transform.Find("Icon3")?.GetComponent<Image>(); 
		image_qipao = transform.Find("qipao")?.GetComponent<Image>(); 

	}
	#endregion 

}
