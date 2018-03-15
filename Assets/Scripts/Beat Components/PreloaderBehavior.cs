using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloaderBehavior : MonoBehaviour {
	public float startTime;
	public float expandSpeed;

	void Update () {
		float scale = (Time.time - startTime) * expandSpeed;
		transform.localScale = new Vector3 (scale, scale, scale);
	}
}
