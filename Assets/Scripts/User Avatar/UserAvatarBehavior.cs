using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAvatarBehavior : MonoBehaviour {

	public Transform cameraTransform;
	private Vector3 targetPosition;
	public static Vector3 avatarPosition;
	private Vector3 originalPosition;
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		avatarPosition = originalPosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 cameraRotation = cameraTransform.rotation.eulerAngles;
		//avatarPosition = Quaternion.Euler (0f, cameraRotation.y, 0f) * originalPosition;
		targetPosition = Quaternion.Euler(0f, cameraRotation.y, 0f) * originalPosition;
		avatarPosition += (targetPosition - transform.position) * 0.01f;
		transform.position = avatarPosition;

		//transform.position += (avatarPosition - transform.position) * 0.01f;
		//transform.position = avatarPosition;
	}
}
