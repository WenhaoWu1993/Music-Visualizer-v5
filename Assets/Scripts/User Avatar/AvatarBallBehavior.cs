using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarBallBehavior : MonoBehaviour {
	
	private float scale = 0f;

	void Start () {
		GetComponent<ParticleSystem> ().Play ();
	}

	void Update () {
		scale = (scale > 0.01f) ? (scale - 0.02f) : 0.01f;
		transform.localScale = new Vector3 (scale, scale, scale);
	}

	void Expand(float a) {
		scale = a;
	}
}
