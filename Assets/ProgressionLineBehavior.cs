using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionLineBehavior : MonoBehaviour {

	private LineRenderer laser;
	private MaterialPropertyBlock propertyBlock;

	public AnimationCurve alphaCurve;
	public Color mainColor;
	public int beatId;
	public float startTime;
	private float endTime;
	public float alpha;

	void Start () {
		laser = GetComponent<LineRenderer> ();
		propertyBlock = new MaterialPropertyBlock ();

		endTime = alphaCurve.keys [alphaCurve.keys.Length - 1].time;
	}
	
	// Update is called once per frame
	void Update () {
		laser.SetPosition (0, InstantiateRocks.rocks[beatId].transform.position);
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
