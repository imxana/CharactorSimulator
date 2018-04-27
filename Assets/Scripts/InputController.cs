using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{

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
	private bool itemSelected = false;

	enum Actions
	{
		Item1,Item2,Item3,Item4,Item5,Item6,Item7,Item8,Item9,Item0,
		Backpack,TipsPanel,Reach,Pick,Check
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
		KeyMap.Add (Actions.Backpack,  "b");
		KeyMap.Add (Actions.TipsPanel, "t");
		KeyMap.Add (Actions.Reach,     "r");
		KeyMap.Add (Actions.Pick,      "e");
		KeyMap.Add (Actions.Check,   "q");
	}

	// Use this for initialization
	void Start () 
	{
		KeySet ();
		PackagePanel.SetActive(false);
//		Reach.SetActive(false);
		ItemsInReach = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// open the backpack
		if (Input.GetKeyDown (KeyMap[Actions.Backpack])) { PackagePanel.SetActive (!PackagePanel.activeSelf); }

		// open the tips panel
		if (Input.GetKeyDown (KeyMap[Actions.TipsPanel]))	{ TipsPanel.SetActive (!TipsPanel.activeSelf); }

		// render the reach range
		if (Input.GetKeyDown (KeyMap[Actions.Reach])){Reach.GetComponent<Renderer>().enabled = !Reach.GetComponent<Renderer>().enabled;}

		// collect one item
		if (Input.GetKeyDown (KeyMap[Actions.Pick])) { PickItem(); }
		if (Input.GetKeyDown (KeyMap[Actions.Pick])) { picked = false; }

		// set collecting state
		if (Input.GetKeyDown (KeyMap[Actions.Check])) { isChecking = true; }
		if (Input.GetKeyUp (KeyMap[Actions.Check]))  { isChecking = false; }


		if (Input.GetKeyDown(KeyMap[Actions.Item1])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (0, true); } 
		else if (Input.GetKeyDown(KeyMap[Actions.Item2])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (1, true); }
		else if (Input.GetKeyDown(KeyMap[Actions.Item3])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (2, true); }
		else if (Input.GetKeyDown(KeyMap[Actions.Item4])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (3, true); }
		else if (Input.GetKeyDown(KeyMap[Actions.Item5])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (4, true); }
		else if (Input.GetKeyDown(KeyMap[Actions.Item6])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (5, true); }
		else if (Input.GetKeyDown(KeyMap[Actions.Item7])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (6, true); }
		else if (Input.GetKeyDown(KeyMap[Actions.Item8])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (7, true); }
		else if (Input.GetKeyDown(KeyMap[Actions.Item9])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (8, true); }
		else if (Input.GetKeyDown(KeyMap[Actions.Item0])) { ResetItemRootPanelSelected (); SetItemRootPanelSelected (9, true); }
	}

	void ResetItemRootPanelSelected()
	{
		for (int i = 0; i < ItemRootPanel.transform.childCount; i++) 
		{
			SetItemRootPanelSelected (i, false);
			itemSelected = false;
		}
	}

	void SetItemRootPanelSelected(int i, bool b)
	{
		Transform itemPanel = ItemRootPanel.transform.GetChild (i);
		itemPanel.Find ("Selected").gameObject.SetActive (b);
		itemSelected = true;
	}


	void PickItem()
	{
		if (picked) {return;}
		if (ItemsInReach.Count > 0) {

			for (int i = 0; i < ItemsInReach.Count; i++)
			{
				// the top prefab
				GameObject top = ItemsInReach[i]; 

				// add in package (pick one at a time)
				bool isRemoved = GetComponent<Package> ().PackageAddItem (top);

				// if adding, destroy in scene
				if (isRemoved) {
					ItemsInReach.Remove (top);
					Destroy (top);
					break;
				} else {
					//GameObject TipsPanel = GetComponent<InputController> ().TipsPanel;
					TipsPanel.transform.Find ("Text").GetComponent<Text>().text = "Inventory is full";
					TipsPanel.SetActive (true);

				};

			}
		}
		picked = true;
	}
}
