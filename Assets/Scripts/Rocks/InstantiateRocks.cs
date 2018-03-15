using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRocks : MonoBehaviour {

	//public float musicLength;
	public static float minDistance = 30f;
	public static float maxDistance = 1000f;
	public static float minSize = 1f;
	public static float maxSize = 16f;
	public static float minHeight = 0f;
	public static float maxHeight = 100f;

	private AudioSource audioSource;
	public static int musicStartTime = 50;
	public static bool musicStarted = false;
	public static bool musicEnded = false;
	public GameObject rockPrefab;
	public static GameObject[] rocks;
	public static int totalBeats;

	public static bool introGuiFaded = false;

	public static GameObject SimilarityLaserPrefab;
	public static GameObject DiSimilarityLaserPrefab;
	public GameObject GeneticLoaderFollowerPrefab;
	public GameObject progressionLinePrefab;

	public Color disimColor, simColor;

	void Awake() {
		SimilarityLaserPrefab = Resources.Load ("SimilarityLaserPrefab") as GameObject;
		DiSimilarityLaserPrefab = Resources.Load ("DiSimilarityLaserPrefab") as GameObject;
		//progressionLinePrefab = Resources.Load ("ProgressionLinePrefab") as GameObject;

		audioSource = GetComponent<AudioSource> ();
		int totalScope = 44100 * Mathf.FloorToInt(audioSource.clip.length) / 1024;
		totalBeats = MusicData.timeStamp.Length;
		rocks = new GameObject[totalBeats];
		RockData.positions = new Vector3[totalBeats];
		RockData.sizes = new float[totalBeats];
		LineColorManager.lineColors = new Color[totalBeats];

		float phase = 0f;

		int beatId = 0;
		for (int i = 0; i < totalScope; i++) {
			if (i == MusicData.frameId [beatId]) {
				GameObject rock = Instantiate (rockPrefab) as GameObject;
				rock.name = beatId.ToString ();
				RockGenerator rg = rock.GetComponent<RockGenerator> ();
				RockBehaviour rb = rock.GetComponent<RockBehaviour> ();
				rb.ID = beatId;

				float amp = MusicData.amp [beatId];
				float acc = MusicData.acc [beatId];
				float gap = MusicData.timeGap [beatId];



				float angle = (i * Mathf.PI * 2 / totalScope) + Mathf.PI * 0.5f;
				rb.distance = Maths.map (amp, 0f, 1f, maxDistance, minDistance);
				rb.targetHeight = Maths.map (amp, 0f, 1f, minHeight, maxHeight);
				rb.targetBrightness = 1f;

				rg.origPosition = new Vector3 (rb.distance * Mathf.Cos (angle), rb.targetHeight, rb.distance * Mathf.Sin (angle));
				rg.size = Maths.map (gap, 0.0f, 1.0f, minSize, maxSize);
				rg.createNormalRock ();
				Destroy (rock.GetComponent<RockGenerator> ());
				Destroy (rock.GetComponent<MeshGenerator> ());
				rock.transform.parent = transform;
				//rock.tag = beatId.ToString();
				rocks [beatId] = rock;
				rock.SetActive (false);
				Mesh mesh = rock.GetComponent<MeshFilter> ().mesh;
				rock.GetComponent<MeshCollider> ().sharedMesh = mesh;

				float phaseDelta = Maths.map (acc, -1f, 1f, -Mathf.PI, Mathf.PI);
				phase += phaseDelta;
				rb.phase = phase;

				RockData.positions [beatId] = rg.origPosition;
				RockData.sizes [beatId] = rg.size;

				//similarity lines
				float delayTime = 0f;
				for (int topId = 0; topId < 3; topId++) {
					int similarId = SimilarityData.TopsAndLeasts [beatId, topId];
					if (similarId < beatId) {
						float score = SimilarityData.similarityData [beatId] [similarId];
						if (score > 0.9f) {
							GameObject simLaser = Instantiate (SimilarityLaserPrefab) as GameObject;
							simLaser.transform.parent = rock.transform;
							InstantSimilarityLaserBehavior simBehavior = simLaser.GetComponent<InstantSimilarityLaserBehavior> ();
							simBehavior.similarityId = similarId;
							simBehavior.delayTime = delayTime;
							simBehavior.mainColor = simColor;
							simBehavior.alpha = score;
							//simLaser.SetActive (false);
							delayTime += 0.1f;
						}
					}
				}

				//dissimilar lines
				float delayTime2 = 0f;
				for (int topId = 5; topId >= 3; topId--) {
					int disimlarId = SimilarityData.TopsAndLeasts [beatId, topId];
					if (disimlarId < beatId) {
						float score = SimilarityData.similarityData [beatId] [disimlarId];
						if (score < 0.1f) {
							GameObject disimLaser = Instantiate (DiSimilarityLaserPrefab) as GameObject;
							disimLaser.transform.parent = rock.transform;
							InstantDiSimilarityLaserBehavior disimBehavior = disimLaser.GetComponent<InstantDiSimilarityLaserBehavior> ();
							disimBehavior.similarityId = disimlarId;
							disimBehavior.delayTime = delayTime2;
							disimBehavior.mainColor = disimColor;
							disimBehavior.alpha = 1f - score;
							//disimLaser.SetActive (false);
							delayTime2 += 0.1f;
						}
					}
				}

				//instantiate progression line
				GameObject progressionLine = Instantiate(progressionLinePrefab) as GameObject;
				progressionLine.GetComponent<ProgressionLineBehavior> ().alpha = amp;
				progressionLine.GetComponent<ProgressionLineBehavior> ().beatId = beatId;
				progressionLine.transform.parent = rock.transform;
				//Vector3 lp = progressionLine.transform.localPosition;
				//--------------------------------------------------------------------


				//instantiate genetic loading followers
				GameObject geneticLoaderFollower = Instantiate(GeneticLoaderFollowerPrefab) as GameObject;
				geneticLoaderFollower.transform.parent = rock.transform;
				geneticLoaderFollower.transform.position = rock.transform.position;
				ParticleSystem gps = geneticLoaderFollower.GetComponent<ParticleSystem> ();
				var shapeModule = gps.shape;
				shapeModule.radius = rg.size;
				var mainModule = gps.main;
				mainModule.startSize = rg.size * 3f;
				geneticLoaderFollower.SetActive (false);
				//--------------------------------------------------------------------

				if ((beatId + 1) < totalBeats) {
					beatId++;
				}


			}
				
		}
			
	}

	void Update () {
		if (!musicStarted && introGuiFaded) {
			audioSource.Play ();
			musicStarted = true;
			musicStartTime = Mathf.FloorToInt (Time.time * 1000);
		}

		musicEnded = (musicStarted && !audioSource.isPlaying);
		//showLasers = showLaserRepresentitive;
	}

}
