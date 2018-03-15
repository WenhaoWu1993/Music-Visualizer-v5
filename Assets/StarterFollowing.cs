using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterFollowing : MonoBehaviour {

	public GameObject who;
	public ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ps.isPlaying) {
			transform.position = who.transform.position;
			Debug.Log ("Hey I'm playing");
		}
	}
}
