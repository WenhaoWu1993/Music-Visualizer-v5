using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderTrigger : MonoBehaviour {

//	public Transform rayTransform;
//	private RaycastHit hit;
//	private Ray rockFindingRay;

	public static bool finderLaserLoading = false;
	public static bool showSimilar = true;

	public static int finderLength = 0;

	//public static int geneticStarterId;

	private int tapCount = 0;
	private float[] tapTime = new float[2];
	private float oneTapTime;
	private bool canDoOneClick = true;

	public GameObject geneticStarterPrefab;
	public GameObject geneticStarter;

	public GameObject geneticFinderPrefab;
	public GameObject geneticFinder;

	private int finderStartIdScout;
	public static int finderStartId;

//	private string clickTest;
	// Use this for initialization
	void Start () {
		geneticStarter = Instantiate (geneticStarterPrefab) as GameObject;
		geneticStarter.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		//double tap
		if (InstantiateRocks.musicStarted) {
			if (Input.GetMouseButtonUp (0)) {

				tapTime [tapCount] = Time.time;
				oneTapTime = Time.time;
				tapCount++;

				if (tapCount == 2) {
					if ((tapTime [1] - tapTime [0]) <= 0.5f) {
						showSimilar = !showSimilar;

					}
					tapCount = 0;
				}

				canDoOneClick = true;
				
			}

			//one tap
			if (tapCount == 1 && (Time.time - oneTapTime) > 0.5f && canDoOneClick) {

				if (!finderLaserLoading) {
					geneticStarter.SetActive (true);
					geneticStarter.transform.parent = InstantiateRocks.rocks [BeatBehavior.beatCount].transform;
					geneticStarter.transform.position = InstantiateRocks.rocks [BeatBehavior.beatCount].transform.position;
					finderStartIdScout = BeatBehavior.beatCount;
					//geneticStarter.GetComponent<ParticleSystem> ().Play ();
				} else {
					geneticStarter.SetActive (false);
					//geneticStarter.GetComponent<ParticleSystem> ().Stop();
					if (geneticFinder != null) {
						Destroy (geneticFinder);
					}
					geneticFinder = Instantiate(geneticFinderPrefab) as GameObject;
					geneticFinder.GetComponent<GeneticFinderBehavior> ().startTime = Time.time;
					finderStartId = finderStartIdScout;
				}
				finderLaserLoading = !finderLaserLoading;
				tapCount = 0;
				canDoOneClick = false;
			}

			if (finderLaserLoading && finderLength > 20) {
				geneticStarter.SetActive (false);
				if (geneticFinder != null) {
					Destroy (geneticFinder);
				}
				geneticFinder = Instantiate (geneticFinderPrefab) as GameObject;
				geneticFinder.GetComponent<GeneticFinderBehavior> ().startTime = Time.time;
				finderStartId = finderStartIdScout;
				finderLaserLoading = false;
				finderLength = 0;
			}
		}
			
	}
}
