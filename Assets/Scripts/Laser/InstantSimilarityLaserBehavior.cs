using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSimilarityLaserBehavior : MonoBehaviour {

//	public int fromId, toId;
	public int similarityId;
	private LineRenderer laser;
	private MaterialPropertyBlock propertyBlock;

	public AnimationCurve alphaCurve;
	public float alpha;
	public Color mainColor;
	public float startTime;
	public float delayTime;

	private float endTime;
//	private float peakTime;

	void Start () {
		propertyBlock = new MaterialPropertyBlock ();
		laser = GetComponent<LineRenderer> ();
//		laser.GetPropertyBlock (propertyBlock);
//		propertyBlock.SetColor ("_Color", mainColor);
//		laser.SetPropertyBlock (propertyBlock);

		int keyLength = alphaCurve.keys.Length;
		endTime = alphaCurve.keys [keyLength - 1].time;
//		peakTime = alphaCurve.keys [1].time;
	}
	
	void Update () {
//		laser.SetPosition (0, InstantiateRocks.rocks [fromId].transform.position);
//		laser.SetPosition (1, InstantiateRocks.rocks [toId].transform.position);
//
//		if (!InstantiateRocks.musicEnded) {
//			if ((Time.time - startTime) <= endTime) {
//				mainColor.a = alphaCurve.Evaluate (Time.time - startTime);
//				laser.GetPropertyBlock (propertyBlock);
//				propertyBlock.SetColor ("_Color", mainColor);
//				laser.SetPropertyBlock (propertyBlock);
//			} else {
//				gameObject.SetActive (false);
//			}
//		}

		laser.SetPosition (0, InstantiateRocks.rocks [similarityId].transform.position);
		laser.SetPosition (1, UserAvatarBehavior.avatarPosition);

		if ((Time.time - startTime) <= endTime) {
			mainColor.a = alphaCurve.Evaluate (Time.time - startTime) * alpha;
			laser.GetPropertyBlock (propertyBlock);
			propertyBlock.SetColor ("_Color", mainColor);
			laser.SetPropertyBlock (propertyBlock);
		} else {
			gameObject.SetActive (false);
		}

	}
}
