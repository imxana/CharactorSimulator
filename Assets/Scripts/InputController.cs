using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
	public GameObject HelpPanel;
	public GameObject PackagePanel;
	public GameObject TipsPanel;
	public GameObject ItemRootPanel;
	public GameObject Reach;

	// public to other script
	public List<GameObject> ItemsInReach;
	public bool isChecking = false;
	public bool picked = false;

	// for key maps
	private Dictionary<Actions, string> KeyMap = new Dictionary<Actions, string>();
	private int SelectedItemIndex = 0;

	enum Actions
	{
		Item1,Item2,Item3,Item4,Item5,Item6,Item7,Item8,Item9,Item0,
		Inventory,TipsPanel,Reach,Pick,Check,Use,Help,Discard
	}

	void KeySet()
	{
		KeyMap.Add (Actions.Item1, "1");
		KeyMap.Add (Actions.Item2, "2");
		KeyMap.Add (Actions.Item3, "3");
		KeyMap.Add (Actions.Item4, "4");
		KeyMap.Add (Actions.Item5, "5");
		KeyMap.Add (Actions.Item6, "6");
		KeyMap.Add (Actions.Item7, "7");
		KeyMap.Add (Actions.Item8, "8");
		KeyMap.Add (Actions.Item9, "9");
		KeyMap.Add (Actions.Item0, "0");
		KeyMap.Add (Actions.Inventory, "i");
		KeyMap.Add (Actions.TipsPanel, "t");
		KeyMap.Add (Actions.Reach, "r");
		KeyMap.Add (Actions.Pick, "e");
		KeyMap.Add (Actions.Check, "c");
		KeyMap.Add (Actions.Use, "f");
		KeyMap.Add (Actions.Help, "h");
		KeyMap.Add (Actions.Discard, "q");

	}

	void KeyTipsForHelpPanel()
	{
		string text = "Key Maps\n\n";
		int WrapCount = 0;
		foreach (KeyValuePair<Actions, string> pair in KeyMap) 
		{
			text += string.Format("{0}: {1}   ",pair.Key,pair.Value);
			WrapCount++;
			if (WrapCount % 2 == 0) {text += "\n";}
		}
		HelpPanel.transform.Find("Text").GetComponent<Text>().text = text;
	}

	// Use this for initialization
	void Start () 
	{
		KeySet ();
		KeyTipsForHelpPanel ();
		PackagePanel.SetActive(false);
		ItemsInReach = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// HelpPanel On/Off
		if (Input.GetKeyDown (KeyMap[Actions.Help])) { KeyTipsForHelpPanel (); HelpPanel.SetActive (!HelpPanel.activeSelf); }
		// Inventory On/Off
		if (Input.GetKeyDown (KeyMap[Actions.Inventory])) { PackagePanel.SetActive (!PackagePanel.activeSelf); }
		// TipsPanel On/Off
		if (Input.GetKeyDown (KeyMap[Actions.TipsPanel]))	{ TipsPanel.SetActive (!TipsPanel.activeSelf); }
		// Reach On/Off
		if (Input.GetKeyDown (KeyMap[Actions.Reach])){Reach.GetComponent<Renderer>().enabled = !Reach.GetComponent<Renderer>().enabled;}
		// collecting one item
		if (Input.GetKeyDown (KeyMap[Actions.Pick])) { PickItem(); }
		if (Input.GetKeyDown (KeyMap[Actions.Pick])) { picked = false; }
		// set collecting state (for updating so fast that picking one item twice)
		if (Input.GetKeyDown (KeyMap[Actions.Check])) { isChecking = true; }
		if (Input.GetKeyUp (KeyMap[Actions.Check]))  { isChecking = false; }
		// use/put item
		if (Input.GetKeyDown(KeyMap[Actions.Use])) {GetComponent<Package>().ItemUse(SelectedItemIndex);}
		// discard item
		if (Input.GetKeyDown(KeyMap[Actions.Discard])) {GetComponent<Package>().ItemDiscard(SelectedItemIndex);}
		// item keys
		if (Input.GetKeyDown(KeyMap[Actions.Item1])) { SelectedItemIndex = 0; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item2])) { SelectedItemIndex = 1; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item3])) { SelectedItemIndex = 2; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item4])) { SelectedItemIndex = 3; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item5])) { SelectedItemIndex = 4; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item6])) { SelectedItemIndex = 5; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item7])) { SelectedItemIndex = 6; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item8])) { SelectedItemIndex = 7; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item9])) { SelectedItemIndex = 8; }
		else if (Input.GetKeyDown(KeyMap[Actions.Item0])) { SelectedItemIndex = 9; }
		ResetItemRootPanelSelected (); 
		SetItemRootPanelSelected (SelectedItemIndex, true); 

		// wheel to select item
		if (Input.GetAxis("Mouse ScrollWheel") < 0) { SelectedItemIndex = (SelectedItemIndex+1)%10; }
		if (Input.GetAxis("Mouse ScrollWheel") > 0) { SelectedItemIndex = (SelectedItemIndex+9)%10; }
	}

	void ResetItemRootPanelSelected()
	{
		for (int i = 0; i < ItemRootPanel.transform.childCount; i++) 
		{
			SetItemRootPanelSelected (i, false);
		}
	}

	void SetItemRootPanelSelected(int i, bool b = true)
	{
		Transform itemPanel = ItemRootPanel.transform.GetChild (i);
		itemPanel.Find ("Selected").gameObject.SetActive (b);
	}


	void PickItem()
	{
		// prevent from picking twice
		if (picked) return;
		picked = true;

		// At least one item in reach
		if (ItemsInReach.Count > 0) {
			for (int i = 0; i < ItemsInReach.Count; i++)
			{
				GameObject itemPrefab = ItemsInReach[i]; 

				// check inventory is full
				bool inventoryIsFull = GetComponent<Package> ().PackageAddItem (itemPrefab.GetComponent<CollectableItem>().ItemRefrence);

				// if adding, destroy in scene
				if (inventoryIsFull) {
					TipsPanel.transform.Find ("Text").GetComponent<Text>().text = "Inventory is full" + "\n" + RandomFace();
					TipsPanel.SetActive (true);
				} else {
					ItemsInReach.Remove (itemPrefab);
					Destroy (itemPrefab);
					break; // just pick one at a time
				};
			}
		}
	}

	// not too boring
	string RandomFace()
	{
		string[] o = {" (`･ω･´)ゞ"," (̿?̿̿Ĺ̯̿̿?̿ ̿)̄ " ," ( ‾ʖ̫‾) ", " ( •̀ .̫ •́ )✧"," (｀◕‸◕´+)"," ( ∙̆ .̯ ∙̆ )"};
		return o[Random.Range(0, o.Length)];
	}

}
