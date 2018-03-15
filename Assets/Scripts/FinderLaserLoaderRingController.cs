using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderLaserLoaderRingController : MonoBehaviour {

	public ParticleSystem particleSystem;
	private float t;

	private float lifeTime;

	private float sizeShadow;

	// Use this for initialization
	void Start () {
		lifeTime = particleSystem.main.duration;
		sizeShadow = particleSystem.main.startSize.constant;
	}
	
	// Update is called once per frame
	void Update () {

		if ((Time.time - t) > lifeTime) {
			//particleSystem.Stop ();
			var mainModule = particleSystem.main;
			sizeShadow += 1f;
			mainModule.startSize = sizeShadow;
			//particleSystem.Play ();
			t += lifeTime;
		}
	}
}
