using UnityEngine;
using System.Collections;

public class LerpTurnScript : MonoBehaviour {
	
	public float  turnspeed = 10;

	private bool isRotating = false;
	private Vector3 targetDir;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetDir = -transform.forward;
	}



	void Rotating ()
	{
		//获取方向

		if (isRotating) {
			Quaternion quaDir = Quaternion.LookRotation(targetDir,Vector3.up);
			transform.rotation = Quaternion.Lerp (transform.rotation, quaDir, Time.fixedDeltaTime * turnspeed);

			if (Quaternion.Angle (transform.rotation, quaDir) < 1) {Debug.Log("Done");isRotating = false;}
		} else {
			if( Input.GetKeyDown(KeyCode.Space)){
				isRotating = true;
//				targetDir = new Vector3 (hor,0,ver);
				targetDir = -transform.forward;
			}
		}
	}

	void FixedUpdate(){
		Rotating();
	}

}