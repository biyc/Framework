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

public class HomePageBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_bg; 
	public Image image_recovery; 
	public Button button_recovery; 
	public Text text_Text; 
	public Image image_Container; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_bg = transform.Find("Homepage/bg")?.GetComponent<Image>(); 
		image_recovery = transform.Find("Homepage/recovery")?.GetComponent<Image>(); 
		button_recovery = transform.Find("Homepage/recovery")?.GetComponent<Button>(); 
		text_Text = transform.Find("Homepage/recovery/Text")?.GetComponent<Text>(); 
		image_Container = transform.Find("Container")?.GetComponent<Image>(); 

	}
	#endregion 

}
