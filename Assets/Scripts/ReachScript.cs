using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachScript : MonoBehaviour
{
	public GameObject refrence; // now for player0
	public List<GameObject> ItemsInReach  = new List<GameObject>();
	public List<GameObject> NPCInReach;

	void Start (){
		ItemsInReach  = new List<GameObject>();
	}
}
	