using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour {
	//public float ang;

	public float size;
	public Vector3 origPosition;

	//public Material material;

	public void createNormalRock () {
		transform.position = origPosition;

		//Material material = GetComponent<MeshRenderer> ().materials [0];

		MeshGenerator mg = GetComponent<MeshGenerator> ();

		int topDetails = 4;

		float topRadius = 0.6f * size;
		float bottomRadius = 0.2f * size;

		float[] topHeightRange = { 0.5f * size, size };
		float[] middleHeightRange = { -0.5f * size, 0.5f * size };
		float[] bottomHeightRange = { -size, -0.5f * size };

		//pseudo vertices top
		Vector3[] topPseudoVertices = mg.createTopLayerPseudoVertices(topDetails, topRadius, topHeightRange);

		//pseudo vertices second layer
		Vector3[] SecondLayerPseudoVertices = mg.createSecondLayerPseudoVertices(mg.topVerticesAngles, size, middleHeightRange);

		//bottom vertice
		Vector3 bottomLayerVertice = mg.createBottomLayerVertice(bottomRadius, bottomHeightRange);

		//seal the top
		GameObject topface = mg.createSubmesh (topPseudoVertices, 1);
		topface.transform.parent = transform;

		//seal the side
		for (int i = 0; i < topDetails; i++) {
			int nexti = ((i + 1) >= topDetails) ? 0 : (i + 1);
			Vector3[] sidefacevertices = {
				SecondLayerPseudoVertices [i],
				SecondLayerPseudoVertices [nexti],
				topPseudoVertices [nexti],
				topPseudoVertices [i]
			};
			GameObject sideface = mg.createSubmesh (sidefacevertices, i + 2);
			sideface.transform.parent = transform;

			Vector3[] bottomfacevertices = { SecondLayerPseudoVertices [i], bottomLayerVertice, SecondLayerPseudoVertices [nexti] };
			GameObject bottomface = mg.createSubmesh (bottomfacevertices, i + 2);
			bottomface.transform.parent = transform;
		}

		combineMeshes ();
//
	}

	public void createSmallRock() {
		transform.position = origPosition;

		MeshGenerator mg = GetComponent<MeshGenerator> ();
		//Material material = GetComponent<MeshRenderer> ().materials [0];

		int topDetails = 4;

		float bottomRadius = 0.2f * size;

		float[] topHeightRange = { 0.0f, size };
		float[] bottomHeightRange = { -size, -0.5f * size };

		Vector3[] topPseudoVertices = mg.createTopLayerPseudoVertices(topDetails, size, topHeightRange);
		Vector3 bottomLayerVertice = mg.createBottomLayerVertice (bottomRadius, bottomHeightRange);

		GameObject topFace = mg.createSubmesh (topPseudoVertices, 1);
		topFace.transform.parent = transform;

		for (int i = 0; i < topDetails; i++) {
			int nexti = ((i + 1) >= topDetails) ? 0 : (i + 1);
			Vector3[] sidefacevertices = {
				topPseudoVertices [i],
				bottomLayerVertice,
				topPseudoVertices [nexti]
			};
			GameObject sideface = mg.createSubmesh (sidefacevertices, i + 2);
			sideface.transform.parent = transform;
		}

		combineMeshes ();

	}

	void combineMeshes() {
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter> ();

		CombineInstance[] combine = new CombineInstance[meshFilters.Length - 1];

		//MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
		//meshRenderer.material = mat;

		int i = 1;
		while (i < meshFilters.Length) {
			combine[i - 1].mesh = meshFilters[i].sharedMesh;
			combine[i - 1].transform = meshFilters[i].transform.localToWorldMatrix;
			//meshFilters [i].gameObject.SetActive (false);
			Destroy(meshFilters[i].gameObject);
			i++;
		}
		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		transform.gameObject.SetActive (true);
		//Debug.Log (meshFilters.Length);
	}
}
