using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticFinderBehavior : MonoBehaviour {

	public GameObject finderLaserPrefab;
	private GameObject[] finderLasers;
	private int finderLength;
	private int populationSize = 10;
	private float lifeSpan = 2f;
	public float startTime;

	//for lasers
	//public static int startId;
	public AnimationCurve alphaCurve;
	//public static AnimationCurve alphaCurve;
	public Color colorEditor;
	public static Color mainColor;

	private float[] scores;
	private float totalScore;

	void Awake () {
		mainColor = colorEditor;
		//Debug.Log (FinderTrigger.finderLength);
		finderLength = FinderTrigger.finderLength; // how many "beats" is this finder comparing to, not population size;
		//alphaCurve = alphaCurveEditor;
		finderLasers = new GameObject[populationSize];
		scores = new float[populationSize];
		NewGeneration (true);

		//Debug.Log ("genetic finder active");

	}

	void Update () {
		if (Time.time - startTime > lifeSpan) {
			NewGeneration (false);
			startTime += lifeSpan;
		}
		mainColor.a = alphaCurve.Evaluate (Time.time - startTime) * 0.1f;
	}

	void NewGeneration(bool firstGen) {
		if (firstGen) {
			for (int i = 0; i < finderLasers.Length; i++) {
				finderLasers [i] = Instantiate (finderLaserPrefab, transform) as GameObject;
				int targetId = Mathf.FloorToInt (Random.Range (0f, InstantiateRocks.totalBeats));
				finderLasers [i].GetComponent<GeneticLaserBehavior> ().targetId = targetId;

				for (int j = 0; j < finderLength; j++) {
					int id1 = FinderTrigger.finderStartId + j;
					int id2 = targetId + j;
					float score;
					if (id1 >= InstantiateRocks.totalBeats || id2 >= InstantiateRocks.totalBeats) {
						score = 0f;
					} else {
						score = id1 >= id2 ? SimilarityData.similarityData [id1] [id2] : SimilarityData.similarityData [id2] [id1];
					}
					scores [i] += score;
				}

				totalScore += scores [i];
			}


		} else {
			float totalScoreScout = totalScore;
			totalScore = 0f;
			for (int i = 0; i < finderLasers.Length; i++) {
				
				int targetId;
				float randomNumber = Random.Range (0f, 1f);
				if ((randomNumber - (scores [i] / totalScoreScout)) < 0f) {
					//targetId = finderLasers [i].GetComponent<GeneticLaserBehavior> ().targetId;
					targetId = SimilarityData.TopsAndLeasts[finderLasers [i].GetComponent<GeneticLaserBehavior> ().targetId, Mathf.FloorToInt(Random.Range(0, 3))];
				} else {
					//targetId = Mathf.FloorToInt (Random.Range (0f, InstantiateRocks.totalBeats));
					targetId = SimilarityData.TopsAndLeasts[finderLasers [i].GetComponent<GeneticLaserBehavior> ().targetId, Mathf.FloorToInt(Random.Range(3, 6))];
				}

				Destroy (finderLasers [i]);

				finderLasers [i] = Instantiate (finderLaserPrefab, transform) as GameObject;
				finderLasers [i].GetComponent<GeneticLaserBehavior> ().targetId = targetId;

				scores [i] = 0f;

				for (int j = 0; j < finderLength; j++) {
					int id1 = FinderTrigger.finderStartId + j;
					int id2 = targetId + j;
					float score;
					if (id1 >= InstantiateRocks.totalBeats || id2 >= InstantiateRocks.totalBeats) {
						score = 0f;
					} else {
						score = id1 >= id2 ? SimilarityData.similarityData [id1] [id2] : SimilarityData.similarityData [id2] [id1];
					}
					scores [i] += score;
				}

				totalScore += scores [i];
			}
			
		}
	}
}
