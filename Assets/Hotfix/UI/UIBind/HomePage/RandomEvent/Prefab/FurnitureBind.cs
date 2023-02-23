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

public class FurnitureBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_Furniture; 
	public Button button_Furniture; 
	public Image image_Furniture1; 
	public Image image_Furniture2; 
	public Image image_Furniture3; 
	public Image image_Furniture4; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_Furniture = transform.GetComponent<Image>(); 
		button_Furniture = transform.GetComponent<Button>(); 
		image_Furniture1 = transform.Find("Furniture1")?.GetComponent<Image>(); 
		image_Furniture2 = transform.Find("Furniture2")?.GetComponent<Image>(); 
		image_Furniture3 = transform.Find("Furniture3")?.GetComponent<Image>(); 
		image_Furniture4 = transform.Find("Furniture4")?.GetComponent<Image>(); 

	}
	#endregion 

}
