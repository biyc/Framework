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

public class ExpressBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_Express; 
	public Button button_Express; 
	public Image image_Express1; 
	public Image image_Express2; 
	public Image image_Express3; 
	public Image image_Express4; 
	public Image image_Express5; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_Express = transform.GetComponent<Image>(); 
		button_Express = transform.GetComponent<Button>(); 
		image_Express1 = transform.Find("Express1")?.GetComponent<Image>(); 
		image_Express2 = transform.Find("Express2")?.GetComponent<Image>(); 
		image_Express3 = transform.Find("Express3")?.GetComponent<Image>(); 
		image_Express4 = transform.Find("Express4")?.GetComponent<Image>(); 
		image_Express5 = transform.Find("Express5")?.GetComponent<Image>(); 

	}
	#endregion 

}
