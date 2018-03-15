using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {
	public float[] topVerticesAngles;

	public Vector3[] createTopLayerPseudoVertices(int details, float radius, float[] range) {
		Vector3[] psv = new Vector3[details];
		topVerticesAngles = new float[details];

		float slice = Mathf.PI * 2.0f / details;
		for (int i = 0; i < psv.Length; i++) {
			float anglemax = -i * slice;
			float anglemin = -(i + 1) * slice;
			float angle = Random.Range (anglemin, anglemax);
			topVerticesAngles [i] = angle;
			float r = Random.Range (radius * 0.9f, radius * 1.1f);

			float x = r * Mathf.Cos (angle);
			float y = Random.Range (range [0], range [1]);
			float z = r * Mathf.Sin (angle);

			psv [i] = new Vector3 (x, y, z);
		}

		return psv;
	}

	public Vector3[] createSecondLayerPseudoVertices(float[] angles, float radius, float[] range) {
		Vector3[] psv = new Vector3[angles.Length];

		for (int i = 0; i < psv.Length; i++) {
			float r = Random.Range (radius * 0.9f, radius * 1.1f);

			float x = r * Mathf.Cos (angles [i]);
			float y = Random.Range (range [0], range [1]);
			float z = r * Mathf.Sin (angles [i]);

			psv [i] = new Vector3 (x, y, z);
		}

		return psv;
	}

	public Vector3 createBottomLayerVertice(float radius, float[] range) {
		Vector2 circ = Random.insideUnitCircle * radius;

		float x = circ.x;
		float y = Random.Range (range [0], range [1]);
		float z = circ.y;

		Vector3 v = new Vector3 (x, y, z);
		return v;
	}

	public GameObject createSubmesh(Vector3[] pseudoVertices, int faceId) {
		string name = "face" + faceId.ToString ();
		GameObject face = new GameObject (name);
		face.AddComponent<MeshFilter> ();

		MeshFilter mf = face.GetComponent<MeshFilter> ();
		Mesh mesh = mf.mesh;

		int totalTriangles = pseudoVertices.Length - 2;
		int totalVertices = totalTriangles * 3;

		Vector3[] vertices = new Vector3[totalVertices];
		int[] triangles = new int[totalVertices];
		Vector2[] uvs = new Vector2[vertices.Length];
		//Debug.Log (vertices.Length);
		for (int i = 0; i < totalTriangles; i++) {
			int vstartid = i * 3;
			int vendid = (i + 1) * 3;

			//vertices
			if (i == 0) {
				vertices [vstartid] = pseudoVertices [vstartid];
				vertices [vstartid + 1] = pseudoVertices [vstartid + 1];
				vertices [vstartid + 2] = pseudoVertices [vstartid + 2];
			} else {
				vertices [vstartid] = vertices [vstartid - 3]; //first equal to the first
				vertices[vstartid + 1] = vertices[vstartid + 1 - 2]; //second equal to the last
				vertices[vstartid + 2] = pseudoVertices[i + 2];
			}

			//triangles
			for (int j = vstartid; j < vendid; j++) {
				triangles [j] = j;
			}

			uvs [i] = new Vector2 (vertices [i].x, vertices [i].z);
			uvs [i + 1] = new Vector2 (vertices [i + 1].x, vertices [i + 1].z);
			uvs [i + 2] = new Vector2 (vertices [i + 2].x, vertices [i + 2].z);
		}

		//mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();

		return face;
	}

}
