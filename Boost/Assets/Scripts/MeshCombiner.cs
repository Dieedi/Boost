using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour {

	private void Awake()
	{
		CombineMeshes();
	}

	public void CombineMeshes()
	{
		MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

		//Debug.Log(name + " combining meshes " + filters.Length);

		Mesh finalMesh = new Mesh();

		CombineInstance[] combiners = new CombineInstance[filters.Length];

		for (int a = 0; a < filters.Length; a++) {
			combiners[a].subMeshIndex = 0;
			combiners[a].mesh = filters[a].sharedMesh;
			combiners[a].transform = filters[a].transform.localToWorldMatrix;
		}

		finalMesh.CombineMeshes(combiners);

		GetComponent<MeshFilter>().sharedMesh = finalMesh;

		for (int a = 0; a < transform.childCount; a++) {
			transform.GetChild(a).gameObject.SetActive(false);
		}
	}
}
