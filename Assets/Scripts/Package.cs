using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Package : MonoBehaviour 
{
	public GameObject itemsRoot; // the ui root
	public GameObject exmapleItem;
	public GameObject exmapleItem2;

	private int maxCurrentItems = 10;
	private GameObject itemObjectTemplate;
	private List<ItemObject> CurrentItemsObjects;
	private string[] keyMaps = {"1","2","3","4","5","6","7","8","9","0"};

	// Use this for initialization
	void Start () 
	{
		CurrentItemsObjects = new List<ItemObject>(maxCurrentItems);
		itemObjectTemplate = GameObject.Find ("ItemTemplate");

		initGUI ();

		ItemsInitInsert ();
		RenderCurrentItems ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log (CheckCurrentItemPanelEmpty());
	}



	void initGUI()
	{
		float padding = 5.0f;
		var oldPos = itemObjectTemplate.GetComponent<RectTransform> ().localPosition;
		CurrentItemsObjects.Add(new ItemObject ());

		// from index 1
		for (int i = 1; i < maxCurrentItems ; i++) 
		{
			// clone the template
			GameObject clone = Instantiate(itemObjectTemplate);
			clone.transform.SetParent(itemsRoot.transform);

			// set clone postion
			oldPos.x += (clone.GetComponent<RectTransform>().rect.width + padding*2);
			clone.GetComponent<RectTransform>().localPosition = oldPos;

			// set clone attr
			Text uitext = clone.transform.Find ("Num").GetComponent<Text>();
			uitext.text = keyMaps[i];

			// set empty item
			ItemObject item = new ItemObject ();
			CurrentItemsObjects.Add(item);

			oldPos = clone.GetComponent<RectTransform> ().localPosition;
		}
	}

	void ItemsInitInsert()
	{
		if (exmapleItem2 != null) { for (int i = 0; i< 9 ;i++) SetItemToCurrent (i, exmapleItem2); }
		if (exmapleItem != null) { SetItemToCurrent (8, exmapleItem); }
	}
		
	void RenderCurrentItems()
	{
		for (int i = 0; i < maxCurrentItems; i++) 
		{
			// get the paneltemplate copy
			Transform itemPanel = itemsRoot.transform.GetChild (i);

			// get the data from currentItem list
			ItemObject itemData = CurrentItemsObjects [i];

			// set number of the item
			string number = itemData.quantity == 0 ?  ""  : itemData.quantity + "";
			itemPanel.Find ("Count").GetComponent<Text> ().text = number;

			// set image of the item
			Image image = itemPanel.Find ("Image").GetComponent<Image> ();
			image.overrideSprite = itemData.objectImage;
		}
	}

	void SetItemToCurrent(int index, GameObject objectPrefab)
	{			
		ItemObject ItemData = objectPrefab.GetComponent<CollectableItem>().ItemRefrence;
		//CurrentItemsObjects [index].objectPrefab = objectPrefab;
		CurrentItemsObjects [index].quantity = ItemData.quantity;
		CurrentItemsObjects [index].objectImage = ItemData.objectImage;
//		Debug.LogFormat ("99 {0},{1}",index,CurrentItemsObjects [index].objectPrefab);
	}

//	void SetItemToCurrent(int index, GameObject objectPrefab, int quantity, Sprite objectImage)
//	{
//		CurrentItemsObjects [index].objectPrefab = exmapleItem;
//		CurrentItemsObjects [index].quantity = quantity;
//		CurrentItemsObjects [index].objectImage = objectImage;
//	}


	public bool PackageAddItem(GameObject itemPrefab)
	{
		ItemObject item = itemPrefab.GetComponent<CollectableItem>().ItemRefrence;
		int Emptyindex = CheckCurrentItemPanelEmpty();
		int Existindex = CheckCurrentItemPanelExist(item);

		// item uncountable
		if (item.quantity == 0)
		{
			// check empty item panel
			if (Emptyindex < maxCurrentItems)
			{
				SetItemToCurrent (Emptyindex, itemPrefab);
				RenderCurrentItems ();
				return true;
			}
			// package full
			else 
			{
				return false;
			}
		}

		// item countable
		else
		{
			// check the same item
			if (Existindex < maxCurrentItems)
			{
				CurrentItemsObjects [Existindex].quantity += item.quantity;
				RenderCurrentItems ();
				return true;
			}
			// check empty item panel
			else if (Emptyindex < maxCurrentItems)
			{
				SetItemToCurrent (Emptyindex, itemPrefab);
				RenderCurrentItems ();
				return true;
			}
			// package full
			else 
			{
				return false;
			}
		}
	}

	int CheckCurrentItemPanelEmpty()
	{
		// check empty item panel
		for(int i = 0; i < maxCurrentItems; i++)
		{
			// current panel should not contain the prefab, change to the image....
			if (CurrentItemsObjects [i].objectImage == null) { return i; }
		}

		// for full
		Debug.Log ("Backpack full!");


		return 999;
	}

	int CheckCurrentItemPanelExist(ItemObject item)
	{
		// check if same item existed
		for(int i = 0; i < maxCurrentItems; i++)
		{
			// same image should be the same thing...
			if (CurrentItemsObjects[i].objectImage == item.objectImage) { return i; }
		}
		Debug.Log ("Items Exists");
		return 999;
	}
}
