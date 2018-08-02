using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Fader : MonoBehaviour {

	//[Tooltip("ShaderMode values : 0 = Opaque, 1 = Cutout, 2 = Fade, 3 = Transparent")]
	//[SerializeField] float shaderMode = 2;
	public float alphaValue = 0;

	[SerializeField] UnityEvent FadeOutEvent;
	[SerializeField] Material ExcludedMaterial;

	private bool fade = true;
	private bool fadeOut = false;
	private SkinnedMeshRenderer[] AllMeshes;
	private List<Material> Materials = new List<Material>();

	// Use this for initialization
	void Start ()
	{
		AllMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer mesh in AllMeshes) {
			foreach (Material mat in mesh.sharedMaterials) {
				if (!mat.Equals(ExcludedMaterial)) {
					Materials.Add(mat);
				}
			}
		}

		ShaderChange(0);
	}

	// Update is called once per frame
	void Update () {
		if (!fadeOut)
			StartCoroutine(StartFade());
	}

	IEnumerator StartFade()
	{
		yield return new WaitForSeconds(1f);
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut ()
	{
		while (alphaValue < 1) {
			yield return new WaitForSeconds(.1f);
			fadeOut = true;
			alphaValue += 0.001f;
			ShaderChange(alphaValue);
			if (alphaValue >= 1) {
				FadeOutEvent.Invoke();
			}
		}
	}

	private void ShaderChange(float newAlphaValue)
	{
		fade = newAlphaValue >= 1 ? false : true;
		float shaderMode;
		int renderQueue;
		shaderMode = fade ? 2f : 0f;
		renderQueue = fade ? 3000 : -1;

		foreach (Material mat in Materials) {
			if (fade) {
				mat.SetFloat("_Mode", shaderMode);
				mat.SetFloat("_GlossMapScale", 0f);
				mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				mat.SetInt("_ZWrite", 0);
			} else {
				mat.SetFloat("_Mode", shaderMode);
				mat.SetFloat("_GlossMapScale", 0.8f);
				mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				mat.SetInt("_ZWrite", 1);
			}
			mat.DisableKeyword("_ALPHATEST_ON");
			mat.DisableKeyword("_ALPHABLEND_ON");
			mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			mat.renderQueue = renderQueue;
			Color CurrentColor = mat.color;
			Color NewColor = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, newAlphaValue);
			mat.SetColor("_Color", NewColor);
			mat.SetFloat("_Metallic", newAlphaValue);
		}
	}
}
