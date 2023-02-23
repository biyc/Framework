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

public class PrivacyWindowBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_bg; 
	public Image image_closeBtn; 
	public Text text_txt_b1_10; 
	public ScrollRect scrollrect_Scroll_View; 
	public Image image_Viewport; 
	public Mask mask_Viewport; 
	public Text text_Content; 
	public ContentSizeFitter contentsizefitter_Content; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_bg = transform.Find("bg")?.GetComponent<Image>(); 
		image_closeBtn = transform.Find("bg/closeBtn")?.GetComponent<Image>(); 
		text_txt_b1_10 = transform.Find("bg/txt_b1_10")?.GetComponent<Text>(); 
		scrollrect_Scroll_View = transform.Find("bg/Scroll View")?.GetComponent<ScrollRect>(); 
		image_Viewport = transform.Find("bg/Scroll View/Viewport")?.GetComponent<Image>(); 
		mask_Viewport = transform.Find("bg/Scroll View/Viewport")?.GetComponent<Mask>(); 
		text_Content = transform.Find("bg/Scroll View/Viewport/Content")?.GetComponent<Text>(); 
		contentsizefitter_Content = transform.Find("bg/Scroll View/Viewport/Content")?.GetComponent<ContentSizeFitter>(); 

	}
	#endregion 

}
