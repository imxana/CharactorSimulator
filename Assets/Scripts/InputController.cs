using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
	// the UI panels
	public GameObject HelpPanel;
	public GameObject PackagePanel;
	public GameObject TipsPanel;
	public GameObject ItemRootPanel;

	public GameObject p2;

	// for items in reach
	public GameObject Reach;
	public bool isChecking = false;
	private bool picked = false;

	// for key maps
	private Dictionary<Actions, string> KeyMap = new Dictionary<Actions, string>();
	private int SelectedItemIndex = 0;

	const string ModName = "Misaki_SchoolUniform_summer";
	const string Mod2Name = "Yuko_SchoolUniform_Winter";


	enum Actions
	{
		// applied
		Item1,Item2,Item3,Item4,Item5,Item6,Item7,Item8,Item9,Item0,
		Inventory,TipsPanel,Reach,Pick,Check,Use,Help,Discard,Test,

		// test
		View,
		QuickTurn
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
		KeyMap.Add (Actions.View, "v"); // just for test
		KeyMap.Add (Actions.QuickTurn, "z"); // just for test
		KeyMap.Add (Actions.Test, "x"); // just for test

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

//		ItemsInReach = Reach.GetComponent<ReachScript> ().ItemsInReach;
	}

	void StartLater()
	{
		
	}

	// Update is called once per frame
	void Update () 
	{
		// HelpPanel On/Off
		if (Input.GetKeyDown (KeyMap[Actions.Help])) { KeyTipsForHelpPanel ();TipsPanel.SetActive (false); HelpPanel.SetActive (!HelpPanel.activeSelf); }
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
		if (Input.GetKeyDown(KeyMap[Actions.View])){ TalkView (); }
		// test funs
		if (Input.GetKeyDown(KeyMap[Actions.Test])){ TestAction (); }

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

	void FixedUpdate(){
		QuickTurn ();
		TalkPlayerLocSet ();
	}


	void ResetItemRootPanelSelected() { for (int i = 0; i < ItemRootPanel.transform.childCount; i++)  SetItemRootPanelSelected (i, false); }

	void SetItemRootPanelSelected(int i, bool b = true)
	{
		Transform itemPanel = ItemRootPanel.transform.GetChild (i);
		itemPanel.Find ("Selected").gameObject.SetActive (b);
	}


	void PickItem()
	{
		// prevent one item from being picked twice
		if (picked) return;
		picked = true;

		// At least one item in reach
		if (Reach.GetComponent<ReachScript> ().ItemsInReach.Count > 0) {
			for (int i = 0; i < Reach.GetComponent<ReachScript> ().ItemsInReach.Count; i++)
			{
				GameObject itemPrefab = Reach.GetComponent<ReachScript> ().ItemsInReach[i]; 

				// check inventory is full
				bool inventoryIsFull = GetComponent<Package> ().PackageAddItem (itemPrefab.GetComponent<CollectableItem>().ItemRefrence);

				// if adding, destroy in scene
				if (inventoryIsFull) {
					TipsPanel.transform.Find ("Text").GetComponent<Text>().text = "Inventory is full" + "\n" + RandomFace();
					TipsPanel.SetActive (true);
				} else {
					TipsPanel.SetActive (false);
					Reach.GetComponent<ReachScript> ().ItemsInReach.Remove (itemPrefab);
					Destroy (itemPrefab);
					break; // just pick one at a time
				};
			}
		}
	}

	// should lock some keys
	private bool isTalking = false;

	void TalkView()
	{
		Transform cameraTarget = transform.Find (ModName).Find("CameraTarget");

		if (isTalking) {
			cameraTarget.localPosition = Vector3.zero;
			cameraTarget.localEulerAngles = Vector3.zero;
			isTalking = false;
		} else {
			cameraTarget.localPosition = new Vector3 (0, -0.5f, 0.75f);
			cameraTarget.localEulerAngles = new Vector3 (0, -22.5f, 0);
			isTalking = true;
			willTalk = 2;
			isRotating = false;
		}
	}

	public float talkingDistance = 1.5f;
	private int willTalk = 0;
	private Vector3 talkTarget;

	// update in fixedupdate
	void TalkPlayerLocSet()
	{
		Transform pt = transform.Find (ModName);
		Transform p2t = p2.transform.Find (Mod2Name);
		if (willTalk == 2) {
			willTalk = 1;
			Vector3 Dir = p2t.position - pt.position;
			talkTarget = pt.position + Dir.normalized * (Dir.magnitude - talkingDistance);
		} else if (willTalk == 1) {
			pt.position = Vector3.Lerp (pt.position, talkTarget, Time.fixedDeltaTime * 10f);
			pt.LookAt (p2t);

			//p2t.LookAt (pt);
			NPCController c = p2.GetComponent<NPCController>();

			c.player = pt;

			c.LookAtSth = NPCController.LookState.LookToPlayer;

			if (Vector3.Distance(pt.position, talkTarget) < 0.1f) willTalk = 0;
		}



		// just flash to there
		//transform.Find (ModName).position = TargetPos;
	}

	private bool isRotating;
	private Vector3 turnTarget;
	public float turnspeed = 10f;

	// update in fixedupdate
	void QuickTurn()
	{
		Transform p = transform.Find (ModName);
		if (isRotating) {
			Quaternion quaDir = Quaternion.LookRotation(turnTarget, Vector3.up);
			p.rotation = Quaternion.Lerp (p.rotation, quaDir, Time.fixedDeltaTime * turnspeed);

			if (Quaternion.Angle (p.rotation, quaDir) < 1) {isRotating = false;}
		} else {
			if( Input.GetKeyDown(KeyMap[Actions.QuickTurn]) ){
				isRotating = true;
				//				turnTarget = new Vector3 (hor,0,ver);
				turnTarget = -p.forward;
			}
		}
	}

	void TestAction()
	{
		Debug.Log (transform.Find (ModName).GetComponent<Rigidbody> ());
		transform.Find (ModName).GetComponent<Rigidbody> ().velocity = Vector3.forward;
		transform.Find (ModName).GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
	}

	// not too boring
	string RandomFace()
	{
		string[] o = {" (`･ω･´)ゞ"," (̿?̿̿Ĺ̯̿̿?̿ ̿)̄ " ," ( ‾ʖ̫‾) ", " ( •̀ .̫ •́ )✧"," (｀◕‸◕´+)"," ( ∙̆ .̯ ∙̆ )"};
		return o[Random.Range(0, o.Length)];
	}

}
