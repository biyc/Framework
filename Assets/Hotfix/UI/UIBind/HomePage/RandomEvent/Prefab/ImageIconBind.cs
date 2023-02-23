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

public class ImageIconBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_ImageIcon; 
	public Button button_ImageIcon; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_ImageIcon = transform.GetComponent<Image>(); 
		button_ImageIcon = transform.GetComponent<Button>(); 

	}
	#endregion 

}
