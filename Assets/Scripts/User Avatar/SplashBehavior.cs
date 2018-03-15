using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBehavior : MonoBehaviour {

	private ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
	}

	void splash(float r) {
		var shape = ps.shape;
		shape.radius = r;
		ps.Play ();
	}
}
