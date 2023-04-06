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
	public Image image_bg__1_; 
	public Image image_recovery; 
	public Button button_recovery; 
	public Image image_devBtn; 
	public Button button_devBtn; 
	public Text text_Text; 
	public Image image_Container; 
	public Image image_loading0; 
	public Image image_loading1; 
	public Image image_loading2; 
	public Image image_loading3; 
	public Image image_loading4; 
	public Image image_loading5; 
	public Image image_loading6; 
	public Image image_loading7; 
	public Image image_loading8; 
	public Image image_loading9; 
	public Image image_loading10; 
	public Image image_loading11; 
	public Image image_loading12; 
	public Image image_loading13; 
	public Image image_loading14; 
	public Image image_loading15; 
	public Image image_loading16; 
	public Image image_loading17; 
	public Image image_loading18; 
	public Image image_loading19; 
	public Image image_loading20; 
	public Image image_loading21; 
	public Image image_loading22; 
	public Image image_loading23; 
	public Image image_loading24; 
	public Image image_DevPanel; 
	public Image image_devBg; 
	public Image image_Image; 
	public Image image_netInput; 
	public InputField inputfield_netInput; 
	public Text text_Placeholder; 
	public Text text_Text40; 
	public Text text_Text41; 
	public Image image_nameInput; 
	public InputField inputfield_nameInput; 
	public Text text_Placeholder44; 
	public Text text_Text45; 
	public Text text_Text__1_; 
	public Image image_clickOk; 
	public Button button_clickOk; 
	public Text text_Text49; 
	public Image image_nameStoryPanel; 
	public GridLayoutGroup gridlayoutgroup_nameStoryPanel; 
	public Text text_Text52; 
	public Text text_Text__1_53; 
	public Text text_Text__2_; 
	public Text text_Text__3_; 
	public Text text_Text__4_; 
	public Image image_netStoryPanel; 
	public GridLayoutGroup gridlayoutgroup_netStoryPanel; 
	public Text text_Text59; 
	public Text text_Text__1_60; 
	public Text text_Text__2_61; 
	#endregion 
public void InitUI(Transform trans) {transform = trans; this.InitUI();} 
	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_bg = transform.Find("Homepage/bg")?.GetComponent<Image>(); 
		image_bg__1_ = transform.Find("Homepage/bg (1)")?.GetComponent<Image>(); 
		image_recovery = transform.Find("Homepage/recovery")?.GetComponent<Image>(); 
		button_recovery = transform.Find("Homepage/recovery")?.GetComponent<Button>(); 
		image_devBtn = transform.Find("Homepage/devBtn")?.GetComponent<Image>(); 
		button_devBtn = transform.Find("Homepage/devBtn")?.GetComponent<Button>(); 
		text_Text = transform.Find("Homepage/devBtn/Text")?.GetComponent<Text>(); 
		image_Container = transform.Find("Container")?.GetComponent<Image>(); 
		image_loading0 = transform.Find("Loading/loading0")?.GetComponent<Image>(); 
		image_loading1 = transform.Find("Loading/loading1")?.GetComponent<Image>(); 
		image_loading2 = transform.Find("Loading/loading2")?.GetComponent<Image>(); 
		image_loading3 = transform.Find("Loading/loading3")?.GetComponent<Image>(); 
		image_loading4 = transform.Find("Loading/loading4")?.GetComponent<Image>(); 
		image_loading5 = transform.Find("Loading/loading5")?.GetComponent<Image>(); 
		image_loading6 = transform.Find("Loading/loading6")?.GetComponent<Image>(); 
		image_loading7 = transform.Find("Loading/loading7")?.GetComponent<Image>(); 
		image_loading8 = transform.Find("Loading/loading8")?.GetComponent<Image>(); 
		image_loading9 = transform.Find("Loading/loading9")?.GetComponent<Image>(); 
		image_loading10 = transform.Find("Loading/loading10")?.GetComponent<Image>(); 
		image_loading11 = transform.Find("Loading/loading11")?.GetComponent<Image>(); 
		image_loading12 = transform.Find("Loading/loading12")?.GetComponent<Image>(); 
		image_loading13 = transform.Find("Loading/loading13")?.GetComponent<Image>(); 
		image_loading14 = transform.Find("Loading/loading14")?.GetComponent<Image>(); 
		image_loading15 = transform.Find("Loading/loading15")?.GetComponent<Image>(); 
		image_loading16 = transform.Find("Loading/loading16")?.GetComponent<Image>(); 
		image_loading17 = transform.Find("Loading/loading17")?.GetComponent<Image>(); 
		image_loading18 = transform.Find("Loading/loading18")?.GetComponent<Image>(); 
		image_loading19 = transform.Find("Loading/loading19")?.GetComponent<Image>(); 
		image_loading20 = transform.Find("Loading/loading20")?.GetComponent<Image>(); 
		image_loading21 = transform.Find("Loading/loading21")?.GetComponent<Image>(); 
		image_loading22 = transform.Find("Loading/loading22")?.GetComponent<Image>(); 
		image_loading23 = transform.Find("Loading/loading23")?.GetComponent<Image>(); 
		image_loading24 = transform.Find("Loading/loading24")?.GetComponent<Image>(); 
		image_DevPanel = transform.Find("DevPanel")?.GetComponent<Image>(); 
		image_devBg = transform.Find("DevPanel/devBg")?.GetComponent<Image>(); 
		image_Image = transform.Find("DevPanel/Image")?.GetComponent<Image>(); 
		image_netInput = transform.Find("DevPanel/netInput")?.GetComponent<Image>(); 
		inputfield_netInput = transform.Find("DevPanel/netInput")?.GetComponent<InputField>(); 
		text_Placeholder = transform.Find("DevPanel/netInput/Placeholder")?.GetComponent<Text>(); 
		text_Text40 = transform.Find("DevPanel/netInput/Text")?.GetComponent<Text>(); 
		text_Text41 = transform.Find("DevPanel/netInput/Text")?.GetComponent<Text>(); 
		image_nameInput = transform.Find("DevPanel/nameInput")?.GetComponent<Image>(); 
		inputfield_nameInput = transform.Find("DevPanel/nameInput")?.GetComponent<InputField>(); 
		text_Placeholder44 = transform.Find("DevPanel/nameInput/Placeholder")?.GetComponent<Text>(); 
		text_Text45 = transform.Find("DevPanel/nameInput/Text")?.GetComponent<Text>(); 
		text_Text__1_ = transform.Find("DevPanel/nameInput/Text (1)")?.GetComponent<Text>(); 
		image_clickOk = transform.Find("DevPanel/clickOk")?.GetComponent<Image>(); 
		button_clickOk = transform.Find("DevPanel/clickOk")?.GetComponent<Button>(); 
		text_Text49 = transform.Find("DevPanel/clickOk/Text")?.GetComponent<Text>(); 
		image_nameStoryPanel = transform.Find("DevPanel/nameStoryPanel")?.GetComponent<Image>(); 
		gridlayoutgroup_nameStoryPanel = transform.Find("DevPanel/nameStoryPanel")?.GetComponent<GridLayoutGroup>(); 
		text_Text52 = transform.Find("DevPanel/nameStoryPanel/Text")?.GetComponent<Text>(); 
		text_Text__1_53 = transform.Find("DevPanel/nameStoryPanel/Text (1)")?.GetComponent<Text>(); 
		text_Text__2_ = transform.Find("DevPanel/nameStoryPanel/Text (2)")?.GetComponent<Text>(); 
		text_Text__3_ = transform.Find("DevPanel/nameStoryPanel/Text (3)")?.GetComponent<Text>(); 
		text_Text__4_ = transform.Find("DevPanel/nameStoryPanel/Text (4)")?.GetComponent<Text>(); 
		image_netStoryPanel = transform.Find("DevPanel/netStoryPanel")?.GetComponent<Image>(); 
		gridlayoutgroup_netStoryPanel = transform.Find("DevPanel/netStoryPanel")?.GetComponent<GridLayoutGroup>(); 
		text_Text59 = transform.Find("DevPanel/netStoryPanel/Text")?.GetComponent<Text>(); 
		text_Text__1_60 = transform.Find("DevPanel/netStoryPanel/Text (1)")?.GetComponent<Text>(); 
		text_Text__2_61 = transform.Find("DevPanel/netStoryPanel/Text (2)")?.GetComponent<Text>(); 

	}
	#endregion 

}
