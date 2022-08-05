using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagitMaterialChanger : MonoBehaviour {

	public float normal_strength = 0.5f;
	public float main_brightness = 0.5f;
	public float light_scale = 0.5f;
	public float reflection_shine = 0.22f;
	public Color32 reflection_color = Color.white;

	public Color32 AtmosNearColor = new Color(0.168f,0.737f,1f);
	public Color32 AtmosFarColor = new Color(0.455f,0.518f,0.9850f);
	public float AtmosFallOff = 3f;

	public bool SetMaterial = false;

	void OnDrawGizmos() {
		if (SetMaterial) {
			SetMaterial = false;
			Debug.Log ("Setting Material");
			changeMaterials ();
		}
	}

	private void changeMaterials() {
		Material[] thismaterials;
		thismaterials = this.GetComponent<Renderer> ().sharedMaterials;
		string col_row;
		//Debug.Log ("Render mats" + thismaterials.Length);
		for (int i = 0; i < thismaterials.Length; i++) {

			//Debug.Log ("Material:" + thismaterials [i].name.Replace(" (Instance)",""));
			thismaterials [i].name = thismaterials [i].name.Replace (" (Instance)", "");
			col_row = thismaterials [i].name.Replace ("earth","");
			//Debug.Log ("Material:" + col_row + " Col:" + col_row.Substring(0,1) + " Row:" + col_row.Substring(2,1));
			int row = int.Parse(col_row.Substring(0,1));
			int col = int.Parse(col_row.Substring(2,1));
			int coloffset = col - 5;
			int rowoffset = row - 4;
			if (col == 1) {
				coloffset = -2;
			}
			if (col == 3) {
				coloffset = -4;
			}
			if (col == 4) {
				coloffset = -5;
			}
			if (col == 5) {
				coloffset = -6;
			}
			if (col == 6) {
				coloffset = -7;
			}
			if (col == 7) {
				coloffset = 0;
			}
			if (col == 8) {
				coloffset = -1;
			}

			if (row == 1) {
				rowoffset = -3;
			}
			if (row == 2) {
				rowoffset = -2;
			}
			if (row == 3) {
				rowoffset = -1;
			}
			if (row == 4) {
				rowoffset = 0;
			}

			thismaterials[i].SetFloat("_LightScale",light_scale);
			thismaterials[i].SetFloat("_Brightness",main_brightness);
			thismaterials[i].SetFloat("_Shininess",reflection_shine);
			thismaterials[i].SetFloat("_NormalStrength",normal_strength);
			thismaterials[i].SetColor("_ReflectionColor",reflection_color);
			thismaterials [i].SetTextureScale("_SpecGlossMap", new Vector2 (8, 4));
			thismaterials [i].SetTextureOffset ("_SpecGlossMap", new Vector2 (coloffset, rowoffset));
			thismaterials [i].SetTextureScale("_Normals", new Vector2 (8, 4));
			thismaterials [i].SetTextureOffset ("_Normals", new Vector2 (coloffset, rowoffset));
			thismaterials [i].SetTextureScale("_Lights", new Vector2 (8, 4));
			thismaterials [i].SetTextureOffset ("_Lights", new Vector2 (coloffset, rowoffset));
			thismaterials[i].SetColor("_AtmosNear",AtmosNearColor);
			thismaterials[i].SetColor("_AtmosFar",AtmosFarColor);
			thismaterials[i].SetFloat("_AtmosFalloff",AtmosFallOff);
			//UnityEditor.AssetDatabase.CreateAsset(thismaterials [i], "Assets/Stagit3DEarth/Objects/Earth/Materials/" + thismaterials [i].name + ".mat");
		}
	}
}
