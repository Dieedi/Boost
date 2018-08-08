using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Renderer))]
public class LightSwitch : MonoBehaviour {

	[SerializeField] Texture2D lightmap_dir;
	[SerializeField] Texture2D lightmap_color;
	[SerializeField] string enlightSceneName;
	//[SerializeField] bool enlight = false;

	LightmapData[] lightmaps;
	Texture2D o_lightmap_dir;
	Texture2D o_lightmap_color;
	int mapIndex;


	// Use this for initialization
	void Start () {
		mapIndex = GetComponent<Renderer>().lightmapIndex;
		o_lightmap_color = LightmapSettings.lightmaps[mapIndex].lightmapColor;
		o_lightmap_dir = LightmapSettings.lightmaps[mapIndex].lightmapDir;
		lightmaps = LightmapSettings.lightmaps;
	}
	
	// Update is called once per frame
	void Update () {
		//if (enlight) {
		//	enlight = false;
		//	SetLightMap(1);
		//}
	}

	public void SetLightMap()
	{
		lightmaps[mapIndex].lightmapDir = lightmap_dir;
		lightmaps[mapIndex].lightmapColor = lightmap_color;
		LightmapSettings.lightmaps = lightmaps;
		SceneManager.LoadScene(enlightSceneName, LoadSceneMode.Additive);
	}
}
