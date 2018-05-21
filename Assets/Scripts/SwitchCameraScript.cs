using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCameraScript : MonoBehaviour {

	public Camera camera1;
	public Camera camera2;

	public void SwitchCamera()
	{

		Rect tempRect = camera1.rect;
		float tempDepth = camera1.depth;

		camera1.rect = camera2.rect;
		camera1.depth = camera2.depth;

		camera2.rect = tempRect;
		camera2.depth = tempDepth;

		string[] s = {"Main View", "Player2 View"};

		Text text = transform.Find ("Text").GetComponent<Text> ();
		text.text = text.text == s[1] ? s[0] : s[1];
	}
}
