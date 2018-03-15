using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLoading : MonoBehaviour {

	public static bool userIsReady = false;

	public float duration;

	private RectTransform rectTransform;
	private bool gazeOn;
	private float startTime;
	private float targetLength;
	private float step;

	void Start () {
		rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2 (0, 40);
		gazeOn = false;
		targetLength = 200.0f;
		step = targetLength / duration;
	}
	
	void Update () {
		if (gazeOn && !userIsReady) {
			loading ();
		}
	}

	void loading() {
		float w = (Time.time - startTime) * step;
		if (w >= 200.0f) {
			w = 200.0f;
			userIsReady = true;
		}
		rectTransform.sizeDelta = new Vector2 (w, 40);
	}

	public void gazeEnter() {
		gazeOn = true;
		startTime = Time.time;
	}

	public void gazeExit() {
		gazeOn = false;
		rectTransform.sizeDelta = new Vector2 (0, 40);
	}
}
