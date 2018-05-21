using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

	public Transform player;

	public enum LookState {
		LookToPlayer, // lerp followed
		LookToDefault, // slow return
		Stand
	}
	public LookState LookAtSth = LookState.Stand;

	private Transform reference;
	private Vector3 defaultLook;


	// Use this for initialization
	void Start () {
		reference = transform.Find ("Yuko_SchoolUniform_Winter");
		defaultLook = reference.forward;
	}
	
	// Update is called once per frame
	void Update () {



	}

	void FixedUpdate()
	{
		if (LookAtSth == LookState.LookToPlayer) {
			if (player == null) {
				LookAtSth = LookState.LookToDefault;
			} else {
				// sth wrong: mod will be in continuous rotation
//				Quaternion quaDir = Quaternion.LookRotation(player.transform.position-reference.position, Vector3.up);
//				transform.rotation = Quaternion.Lerp (reference.rotation, quaDir, Time.fixedDeltaTime * 2f);
//				if (Quaternion.Angle (reference.rotation, quaDir) < 1) {LookAtSth = LookState.Stand;}

				transform.LookAt (player); // just instantly followed

				if (Vector3.Distance (transform.position, player.position) > 2) 
				{
					LookAtSth = LookState.LookToDefault;
				}
			}
		} else if (LookAtSth == LookState.LookToDefault) {
			Quaternion quaDir = Quaternion.LookRotation(defaultLook, Vector3.up);
			transform.rotation = Quaternion.Lerp (reference.rotation, quaDir, Time.fixedDeltaTime * 2f);

			if (Quaternion.Angle (reference.rotation, quaDir) < 1)
			{
				LookAtSth = LookState.Stand;
			}
		}
	}



}
