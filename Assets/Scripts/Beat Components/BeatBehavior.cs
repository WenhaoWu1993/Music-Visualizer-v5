using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBehavior : MonoBehaviour {

	public bool delayForCast;
	private float delayedTime;

	public GameObject beatBurstPrefab;
	private List<GameObject> beatBursts = new List<GameObject> ();

	public GameObject beatPreloaderPrefab;
	private List<GameObject> beatPreloaders = new List<GameObject> ();

	public GameObject avatarBall;
	public GameObject avatarSplash;

	private float t;
	private float timeGap = 1f;
	private float intensity;
	public static int beatCount = 0;


	void Start () {
		delayedTime = delayForCast ? 500f : 0f;

	}
	
	void Update () {

		if (InstantiateRocks.musicStarted && beatCount < InstantiateRocks.totalBeats) {
			normalBehavior ();
		}
	}

	void normalBehavior() {
		int currentTime = Mathf.FloorToInt (Time.time * 1000) - InstantiateRocks.musicStartTime;
		if (currentTime > MusicData.timeStamp [beatCount] + delayedTime) {
			float amp = MusicData.amp [beatCount];
			intensity = 30f * Maths.map (amp, 0f, 1f, 0.05f, 1f);
			//userRingColor.a = amp;
			avatarBall.SendMessage("Expand", amp);
			if (amp > 0.3f)
				avatarSplash.SendMessage ("splash", amp);

			//************************** beat bursts **************************//
			GameObject burst = Instantiate (beatBurstPrefab) as GameObject;
			ParticleSystem burstPs = burst.GetComponent<ParticleSystem> ();
			burst.transform.position = new Vector3 (RockData.positions [beatCount].x, 0f, RockData.positions [beatCount].z);

			//burst radius -> size of rock (time gap of the beat)
			var shapeModule = burstPs.shape;
			shapeModule.radius = RockData.sizes [beatCount];

			//burst velocity (height) -> height of rock (amp of the beat)
			var velocityModule = burstPs.velocityOverLifetime;
			velocityModule.zMultiplier = RockData.positions [beatCount].y * 3.2f;

			beatBursts.Add (burst);

			//remove dead beat bursts
			for (int i = beatBursts.Count - 1; i >= 0; i--) {
				GameObject b = beatBursts [i];
				if (!(b.GetComponent<ParticleSystem> ().isPlaying)) {
					beatBursts.Remove (b);
					Destroy (b);
				}
			}
			//************************** beat bursts **************************//

			//************************** beat preloaders **************************//
			if (beatCount + 1 < InstantiateRocks.totalBeats) {
				GameObject preloader = Instantiate (beatPreloaderPrefab) as GameObject;
				ParticleSystem preloaderPs = preloader.GetComponent<ParticleSystem> ();
				preloader.transform.position = new Vector3 (RockData.positions [beatCount + 1].x, 0f, RockData.positions [beatCount + 1].z);

				float gap = (MusicData.timeStamp [beatCount + 1] - MusicData.timeStamp [beatCount]) / 1000f;
				var mainModule = preloaderPs.main;
				mainModule.duration = gap;
				mainModule.startLifetime = gap;

				PreloaderBehavior behavior = preloader.GetComponent<PreloaderBehavior> ();
				behavior.expandSpeed = RockData.sizes [beatCount + 1] / gap;
				behavior.startTime = Time.time;

				preloaderPs.Play ();
				beatPreloaders.Add (preloader);
			}

			for (int i = beatPreloaders.Count - 1; i >= 0; i--) {
				GameObject bp = beatPreloaders [i];
				if (!(bp.GetComponent<ParticleSystem> ().isEmitting)) {
					beatPreloaders.Remove (bp);
					Destroy (bp);
				}
			}
			//************************** beat preloaders **************************//

			InstantiateRocks.rocks[beatCount].SetActive(true);
			InstantiateRocks.rocks [beatCount].GetComponent<RockBehaviour> ().canLoadLaser = true;
			if (FinderTrigger.finderLaserLoading) {
				ParticleSystem ps = InstantiateRocks.rocks [beatCount].GetComponentInChildren<ParticleSystem> (true);
				ps.gameObject.SetActive (true);
				InstantiateRocks.rocks [beatCount].GetComponent<RockBehaviour> ().followerActive = true;
				FinderTrigger.finderLength++;
			}

			beatCount++;
		}


	}

}
