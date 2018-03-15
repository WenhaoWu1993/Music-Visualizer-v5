using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour {

	//public static bool introGuiFaded = false;
	public GameObject gui;
	private CanvasGroup canvasGroup;
	// Use this for initialization
	void Start () {
		canvasGroup = GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (BarLoading.userIsReady) {
			canvasGroup.alpha -= 0.02f;

			if (canvasGroup.alpha <= 0.0f) {
				//EmitterRingManager.introGuiFaded = true;
				InstantiateRocks.introGuiFaded = true;
				//gui.SetActive (false);
				Destroy(gui);
				//Debug.Log ("am i still working?");
				//InstantiateRocks.startMusic = true;
			}
		}
	}
}
