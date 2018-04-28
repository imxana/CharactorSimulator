using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Package : MonoBehaviour 
{
	// the items show in she scene
	public GameObject exmapleItem;
	public GameObject exmapleItem2;

	public GameObject currentItemsRoot; // the ui root
	public GameObject packageItemsRoot; // the ui root

	private int maxCurrentItems = 10;
	private int maxInventoryRow = 4; // maxInventoryItems = maxCurrentItems * maxInventoryRow;

	private Transform currentItemsTemplate;
	private Transform PackageItemsTemplate;

	private List<ItemObject> CurrentItemsObjects;
	private string[] keyMaps = {"1","2","3","4","5","6","7","8","9","0"};

	// Use this for initialization
	void Start () 
	{
		CurrentItemsObjects = new List<ItemObject>(maxCurrentItems);
		currentItemsTemplate = currentItemsRoot.transform.Find ("ItemTemplate");
		PackageItemsTemplate = packageItemsRoot.transform.Find ("ItemTemplate");

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
		// init currentItem panel
		float padding = 5.0f;
		var oldPos = currentItemsTemplate.GetComponent<RectTransform> ().localPosition;
		CurrentItemsObjects.Add(ScriptableObject.CreateInstance<ItemObject>());

		// from index 1
		for (int i = 1; i < maxCurrentItems ; i++) 
		{
			// clone the template
			GameObject clone = Instantiate(currentItemsTemplate.gameObject);
			clone.transform.SetParent(currentItemsRoot.transform);

			// set clone postion
			oldPos.x += (clone.GetComponent<RectTransform>().rect.width + padding*2);
			clone.GetComponent<RectTransform>().localPosition = oldPos;

			// set clone attr
			Text uitext = clone.transform.Find ("Num").GetComponent<Text>();
			uitext.text = keyMaps[i];

			// set empty item
			ItemObject item = ScriptableObject.CreateInstance<ItemObject>();
			CurrentItemsObjects.Add(item);

			oldPos = clone.GetComponent<RectTransform> ().localPosition;
		}

		// init packageItem panel

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
			Transform itemPanel = currentItemsRoot.transform.GetChild (i);

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
		CurrentItemsObjects [index].objectPrefab = objectPrefab;
		CurrentItemsObjects [index].quantity = ItemData.quantity;
		CurrentItemsObjects [index].objectImage = ItemData.objectImage;
		CurrentItemsObjects [index].itemLogic = ItemData.itemLogic;
	}




	public bool PackageAddItem(ItemObject item)
	{
		//ItemObject item = itemPrefab.GetComponent<CollectableItem>().ItemRefrence;
		GameObject itemPrefab = item.objectPrefab;
		int Emptyindex = CheckCurrentItemPanelEmpty();
		int Existindex = CheckCurrentItemPanelExist(item);

		// item uncountable
//		if (item.itemLogic.itemType == ItemType.Material)
		if (item.quantity == 0)
		{
			// check empty item panel
			if (Emptyindex < maxCurrentItems)
			{
				SetItemToCurrent (Emptyindex, itemPrefab);
				RenderCurrentItems ();
				return false;
			}
			// package full
			else 
			{
				return true;
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
				return false;
			}
			// check empty item panel
			else if (Emptyindex < maxCurrentItems)
			{
				SetItemToCurrent (Emptyindex, itemPrefab);
				RenderCurrentItems ();
				return false;
			}
			// package full
			else 
			{
				return true;
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
		return 999;
	}

	public void ItemUse(int i)
	{
		// empty, just return
		if (CurrentItemsObjects [i].objectImage == null) return;

		// use effect
		ItemObjectLogic itemLogic = CurrentItemsObjects [i].itemLogic;
		itemLogic.UseItem(transform.Find ("Misaki_SchoolUniform_summer"), CurrentItemsObjects [i]);

		// reduce from panel
		if (CurrentItemsObjects [i].quantity > 1) 
		{
			CurrentItemsObjects [i].quantity -= 1;
		} 
		else 
		{
			CurrentItemsObjects [i] = ScriptableObject.CreateInstance<ItemObject> ();
		}
		RenderCurrentItems();
	}

	public void ItemDiscard(int i)
	{
		// nothing to discard
		if (CurrentItemsObjects [i].objectImage == null) return;

		int OneTimeDiscard = 3;
		ItemObjectLogic itemLogic = CurrentItemsObjects [i].itemLogic;
		Transform tf = transform.Find ("Misaki_SchoolUniform_summer");

		// discard to the plane and reduce from panel
		if (CurrentItemsObjects [i].quantity > OneTimeDiscard) {
			itemLogic.DiscardItem (tf, CurrentItemsObjects [i], OneTimeDiscard);
			CurrentItemsObjects [i].quantity -= OneTimeDiscard;
		} 
		else if(CurrentItemsObjects [i].quantity > 0)
		{
			itemLogic.DiscardItem (tf, CurrentItemsObjects [i], CurrentItemsObjects [i].quantity);
			CurrentItemsObjects [i] = ScriptableObject.CreateInstance<ItemObject> ();
		}
		else if(CurrentItemsObjects [i].quantity == 0) // uncountable
		{
			itemLogic.DiscardItem (tf, CurrentItemsObjects [i]);
			CurrentItemsObjects [i] = ScriptableObject.CreateInstance<ItemObject> ();
		}
		else
		{
			CurrentItemsObjects [i] = ScriptableObject.CreateInstance<ItemObject> ();
		}
		RenderCurrentItems();
	}
}
