using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
	public ItemObject ItemRefrence;
	public Material ItemMat;
	public Material ItemMatInReach;
	public Material ItemMatInReach2;

	public Material TipsCollectable;
	public Material TipsFull;


	private MeshRenderer mesh;
	private bool inReach;
	private bool justSpawned;

	private GameObject player;


	// Use this for initialization
	void Start ()
	{
		mesh = GetComponent<MeshRenderer> (); 
		inReach = false;
		justSpawned = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		Material mat = inReach ? ItemMatInReach : ItemMat;
//		SwitchMateria (mat);
	}

	void OnTriggerEnter(Collider other)
	{
		inReach = other.CompareTag ("InReach");
		if (inReach)
		{
			player = other.GetComponent<ReachScript> ().refrence;
//			player.GetComponent<InputController>().ItemsInReach.Add (gameObject);
			other.GetComponent<ReachScript> ().ItemsInReach.Add (gameObject);
			SwitchMateria (ItemMatInReach);
		}
	}

	void OnTriggerExit(Collider other)
	{
		inReach = other.CompareTag ("InReach");
		if (inReach)
		{
			player = other.GetComponent<ReachScript>().refrence;
//			player.GetComponent<InputController>().ItemsInReach.Remove (gameObject);
			other.GetComponent<ReachScript>().ItemsInReach.Remove (gameObject);

			SwitchMateria(ItemMat);
		}
	}

	void OnTriggerStay(Collider other)
	{
		inReach = other.CompareTag ("InReach");
		if (inReach)
		{
			player = other.GetComponent<ReachScript> ().refrence;
			if (player.GetComponent<InputController> ().isChecking) {
				SwitchMateria (ItemMatInReach2);
			} else {
				SwitchMateria (ItemMatInReach);
			}
		}
	}



	public void SwitchMateria(Material mat)
	{
		mesh.material = mat;

		// set the key tips
		bool i = mat == ItemMatInReach;
		transform.Find ("TipsQuad").gameObject.SetActive(i);

		// check if package is full for item
		bool isFull = player.GetComponent<Package> ().PackageAddItem (ItemRefrence, false);
		if (isFull) {
			SwitchTipsMateria (TipsFull);	
		} else {
			SwitchTipsMateria (TipsCollectable);
		}
	}

	public void SwitchTipsMateria(Material mat)
	{
		transform.Find ("TipsQuad").GetComponent<MeshRenderer> ().material = mat;
	}


}
