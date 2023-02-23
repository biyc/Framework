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

public class TaxiBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_Taxi; 
	public Button button_Taxi; 
	public Image image_car1; 
	public Image image_car2; 
	public Image image_qipao; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_Taxi = transform.GetComponent<Image>(); 
		button_Taxi = transform.GetComponent<Button>(); 
		image_car1 = transform.Find("car1")?.GetComponent<Image>(); 
		image_car2 = transform.Find("car2")?.GetComponent<Image>(); 
		image_qipao = transform.Find("qipao")?.GetComponent<Image>(); 

	}
	#endregion 

}
