using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
	public ItemObject ItemRefrence;
	public Material ItemMat;
	public Material ItemMatInReach;
	public Material ItemMatInReach2;

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
			player =  other.GetComponent<ReachScript>().refrence;
			player.GetComponent<InputController>().ItemsInReach.Add (gameObject);
			SwitchMateria (ItemMatInReach);

			// output the items count inrange
			PrintCountInReach (player);
		}

	}

	void OnTriggerExit(Collider other)
	{
		inReach = other.CompareTag ("InReach");
		if (inReach)
		{
			player =  other.GetComponent<ReachScript>().refrence;
			player.GetComponent<InputController>().ItemsInReach.Remove (gameObject);
			SwitchMateria( ItemMat);

			// output the items count inrange
			PrintCountInReach (player);
		}
	}

	void OnTriggerStay(Collider other)
	{
		inReach = other.CompareTag ("InReach");
		if (inReach)
		{
			player = other.GetComponent<ReachScript>().refrence;
			if (player.GetComponent<InputController> ().isChecking) {
				SwitchMateria(ItemMatInReach2);
			} else
				SwitchMateria(ItemMatInReach);
		}
	}



	public void SwitchMateria(Material mat)
	{
		mesh.material = mat;

		// set the key tips
		bool i = mat == ItemMatInReach;
		transform.Find ("TipsQuad").gameObject.SetActive(i);
	}


	public void PrintCountInReach(GameObject player)
	{
		//Debug.LogFormat ("Items in reach: {0}", player.GetComponent<InputController> ().ItemsInReach.Count);
	}
}
