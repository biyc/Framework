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

public class DevModifyBind
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_img_Open; 
	public Button button_img_Open; 
	public Text text_Text; 
	public Image image_img_Close; 
	public Button button_img_Close; 
	public Image image_m_Panel; 
	public Image image_img_Ok; 
	public Button button_img_Ok; 
	public Text text_Text9; 
	public Text text_txt_Tips; 
	public HorizontalLayoutGroup horizontallayoutgroup_Tainer; 
	public Image image_Dropdown; 
	public Dropdown dropdown_Dropdown; 
	public Text text_Label; 
	public Image image_Arrow; 
	public Image image_Template; 
	public ScrollRect scrollrect_Template; 
	public Image image_Viewport; 
	public Mask mask_Viewport; 
	public Toggle toggle_Item; 
	public Image image_Item_Background; 
	public Image image_Item_Checkmark; 
	public Text text_Item_Label; 
	public Image image_Scrollbar; 
	public Scrollbar scrollbar_Scrollbar; 
	public Image image_Handle; 
	public Image image_Dropdown2; 
	public Dropdown dropdown_Dropdown2; 
	public Text text_Label29; 
	public Image image_Arrow30; 
	public Image image_Template31; 
	public ScrollRect scrollrect_Template32; 
	public Image image_Viewport33; 
	public Mask mask_Viewport34; 
	public Toggle toggle_Item35; 
	public Image image_Item_Background36; 
	public Image image_Item_Checkmark37; 
	public Text text_Item_Label38; 
	public Image image_Scrollbar39; 
	public Scrollbar scrollbar_Scrollbar40; 
	public Image image_Handle41; 
	public Image image_InputField; 
	public InputField inputfield_InputField; 
	public Text text_Placeholder; 
	public Text text_Text45; 
	public Text text_Tip; 
	public Text text_Text47; 
	public GameObject go_m_Panel; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_img_Open = transform.Find("img_Open")?.GetComponent<Image>(); 
		button_img_Open = transform.Find("img_Open")?.GetComponent<Button>(); 
		text_Text = transform.Find("img_Open/Text")?.GetComponent<Text>(); 
		image_img_Close = transform.Find("img_Close")?.GetComponent<Image>(); 
		button_img_Close = transform.Find("img_Close")?.GetComponent<Button>(); 
		image_m_Panel = transform.Find("m_Panel")?.GetComponent<Image>(); 
		image_img_Ok = transform.Find("m_Panel/img_Ok")?.GetComponent<Image>(); 
		button_img_Ok = transform.Find("m_Panel/img_Ok")?.GetComponent<Button>(); 
		text_Text9 = transform.Find("m_Panel/img_Ok/Text")?.GetComponent<Text>(); 
		text_txt_Tips = transform.Find("m_Panel/txt_Tips")?.GetComponent<Text>(); 
		horizontallayoutgroup_Tainer = transform.Find("m_Panel/Tainer")?.GetComponent<HorizontalLayoutGroup>(); 
		image_Dropdown = transform.Find("m_Panel/Tainer/Dropdown")?.GetComponent<Image>(); 
		dropdown_Dropdown = transform.Find("m_Panel/Tainer/Dropdown")?.GetComponent<Dropdown>(); 
		text_Label = transform.Find("m_Panel/Tainer/Dropdown/Label")?.GetComponent<Text>(); 
		image_Arrow = transform.Find("m_Panel/Tainer/Dropdown/Arrow")?.GetComponent<Image>(); 
		image_Template = transform.Find("m_Panel/Tainer/Dropdown/Template")?.GetComponent<Image>(); 
		scrollrect_Template = transform.Find("m_Panel/Tainer/Dropdown/Template")?.GetComponent<ScrollRect>(); 
		image_Viewport = transform.Find("m_Panel/Tainer/Dropdown/Template/Viewport")?.GetComponent<Image>(); 
		mask_Viewport = transform.Find("m_Panel/Tainer/Dropdown/Template/Viewport")?.GetComponent<Mask>(); 
		toggle_Item = transform.Find("m_Panel/Tainer/Dropdown/Template/Viewport/Content/Item")?.GetComponent<Toggle>(); 
		image_Item_Background = transform.Find("m_Panel/Tainer/Dropdown/Template/Viewport/Content/Item/Item Background")?.GetComponent<Image>(); 
		image_Item_Checkmark = transform.Find("m_Panel/Tainer/Dropdown/Template/Viewport/Content/Item/Item Checkmark")?.GetComponent<Image>(); 
		text_Item_Label = transform.Find("m_Panel/Tainer/Dropdown/Template/Viewport/Content/Item/Item Label")?.GetComponent<Text>(); 
		image_Scrollbar = transform.Find("m_Panel/Tainer/Dropdown/Template/Scrollbar")?.GetComponent<Image>(); 
		scrollbar_Scrollbar = transform.Find("m_Panel/Tainer/Dropdown/Template/Scrollbar")?.GetComponent<Scrollbar>(); 
		image_Handle = transform.Find("m_Panel/Tainer/Dropdown/Template/Scrollbar/Sliding Area/Handle")?.GetComponent<Image>(); 
		image_Dropdown2 = transform.Find("m_Panel/Tainer/Dropdown2")?.GetComponent<Image>(); 
		dropdown_Dropdown2 = transform.Find("m_Panel/Tainer/Dropdown2")?.GetComponent<Dropdown>(); 
		text_Label29 = transform.Find("m_Panel/Tainer/Dropdown2/Label")?.GetComponent<Text>(); 
		image_Arrow30 = transform.Find("m_Panel/Tainer/Dropdown2/Arrow")?.GetComponent<Image>(); 
		image_Template31 = transform.Find("m_Panel/Tainer/Dropdown2/Template")?.GetComponent<Image>(); 
		scrollrect_Template32 = transform.Find("m_Panel/Tainer/Dropdown2/Template")?.GetComponent<ScrollRect>(); 
		image_Viewport33 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Viewport")?.GetComponent<Image>(); 
		mask_Viewport34 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Viewport")?.GetComponent<Mask>(); 
		toggle_Item35 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Viewport/Content/Item")?.GetComponent<Toggle>(); 
		image_Item_Background36 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Viewport/Content/Item/Item Background")?.GetComponent<Image>(); 
		image_Item_Checkmark37 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Viewport/Content/Item/Item Checkmark")?.GetComponent<Image>(); 
		text_Item_Label38 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Viewport/Content/Item/Item Label")?.GetComponent<Text>(); 
		image_Scrollbar39 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Scrollbar")?.GetComponent<Image>(); 
		scrollbar_Scrollbar40 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Scrollbar")?.GetComponent<Scrollbar>(); 
		image_Handle41 = transform.Find("m_Panel/Tainer/Dropdown2/Template/Scrollbar/Sliding Area/Handle")?.GetComponent<Image>(); 
		image_InputField = transform.Find("m_Panel/Tainer/InputField")?.GetComponent<Image>(); 
		inputfield_InputField = transform.Find("m_Panel/Tainer/InputField")?.GetComponent<InputField>(); 
		text_Placeholder = transform.Find("m_Panel/Tainer/InputField/Placeholder")?.GetComponent<Text>(); 
		text_Text45 = transform.Find("m_Panel/Tainer/InputField/Text")?.GetComponent<Text>(); 
		text_Tip = transform.Find("Tip")?.GetComponent<Text>(); 
		text_Text47 = transform.Find("Text")?.GetComponent<Text>(); 
		go_m_Panel = transform.Find("m_Panel")?.gameObject; 

	}
	#endregion 

}
