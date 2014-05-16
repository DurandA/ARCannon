using UnityEngine;
using System.Collections;

public class Ambiance : MonoBehaviour {

	public Light light;
	float shortDuration = 0.3f;
	float longDuration = 2f;

	void Update () {
		// argument for cosine
		float phiShort = Time.time / shortDuration * 2f * Mathf.PI;
		float phiLong = Time.time / longDuration * 2f * Mathf.PI;

		light.intensity = GetAmplitude(phiShort)*0.3f + GetAmplitude(phiLong)*0.5f+0.2f;
	}

	float GetAmplitude(float phi){
		// get cosine and transform from -1..1 to 0..1 range
		return Mathf.Cos( phi ) * 0.5f + 0.5f;
	}
}