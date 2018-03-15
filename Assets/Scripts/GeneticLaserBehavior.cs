using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticLaserBehavior : MonoBehaviour {

	private MaterialPropertyBlock propertyBlock;
	private LineRenderer laser;

	public int targetId;

	void Start () {
		propertyBlock = new MaterialPropertyBlock ();
		laser = GetComponent<LineRenderer> ();
	}
	
	void Update () {
		setColor ();
		setPosition ();
	}

	void setColor() {
		laser.GetPropertyBlock (propertyBlock);
		propertyBlock.SetColor ("_Color", GeneticFinderBehavior.mainColor);
		laser.SetPropertyBlock (propertyBlock);
	}

	void setPosition() {
		laser.SetPosition (0, InstantiateRocks.rocks [FinderTrigger.finderStartId].transform.position);
		laser.SetPosition (1, InstantiateRocks.rocks [targetId].transform.position);
	}
}
