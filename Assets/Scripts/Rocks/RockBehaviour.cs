using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehaviour : MonoBehaviour {

	private MeshRenderer meshRenderer;
	private MaterialPropertyBlock propBlock;


	public float targetHeight;
	private float h_rangeRatio = 0.6f;

	public float targetBrightness;
	private float brightness = 0f;
	private float b_rangeRatio = 0f;

	public float phase;
	public float distance;

	public int ID; 

	public bool canLoadLaser = false;
	public bool followerActive = false;
//	private GameObject disimLaser, simLaser;

	void Start () {
		propBlock = new MaterialPropertyBlock ();
		meshRenderer = GetComponent<MeshRenderer> ();

		meshRenderer.GetPropertyBlock (propBlock);
		propBlock.SetColor ("_Color", new Color(brightness, brightness, brightness, 1));
		meshRenderer.SetPropertyBlock (propBlock);
	}
//
	void Update () {
		if (canLoadLaser) {
//			InstantSimilarityLaserBehavior[] behaviors = GetComponentsInChildren<InstantSimilarityLaserBehavior>();
//			foreach (InstantSimilarityLaserBehavior behavior in behaviors) {
//				behavior.startTime = Time.time;
//			}
//
			ProgressionLineBehavior progressionBehavior = GetComponentInChildren<ProgressionLineBehavior> ();
			progressionBehavior.startTime = Time.time;
			if (FinderTrigger.showSimilar) {
				InstantSimilarityLaserBehavior[] simBehaviors = GetComponentsInChildren<InstantSimilarityLaserBehavior> ();
				foreach (InstantSimilarityLaserBehavior simBehavior in simBehaviors) {
					//simBehavior.gameObject.SetActive (true);
					simBehavior.startTime = Time.time + simBehavior.delayTime;
				}
			} else {
				InstantDiSimilarityLaserBehavior[] disimBehaviors = GetComponentsInChildren<InstantDiSimilarityLaserBehavior> ();
				foreach (InstantDiSimilarityLaserBehavior disimBehavior in disimBehaviors) {
					//disimBehavior.gameObject.SetActive (true);
					disimBehavior.startTime = Time.time + disimBehavior.delayTime;
				}
			}
			canLoadLaser = false;
		}
//
		Phaser ();

		if (followerActive && !FinderTrigger.finderLaserLoading) {
			ParticleSystem ps = GetComponentInChildren<ParticleSystem> ();
			ps.gameObject.SetActive (false);
			followerActive = false;
		}

	}

	void Phaser() {
		if (brightness < targetBrightness) {
			brightness += 0.02f;
		}

		float c = ((1f - b_rangeRatio) * brightness / 2f) * Mathf.Sin(Time.time + phase) + ((1f + b_rangeRatio) * brightness / 2f); 
		meshRenderer.GetPropertyBlock (propBlock);
		propBlock.SetColor ("_Color", new Color (c, c, c, 1));
		meshRenderer.SetPropertyBlock (propBlock);

		float h = ((1f - h_rangeRatio) * targetHeight / 2f) * Mathf.Sin (Time.time + phase) + ((1f + h_rangeRatio) * targetHeight / 2f);
		Vector3 pos = transform.position;
		transform.position = new Vector3 (pos.x, h, pos.z);
	}
		

}
