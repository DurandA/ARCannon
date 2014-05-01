using UnityEngine;
using System.Collections;

public class Ambiance : MonoBehaviour {

	// Your light gameObject here.
	public Light light;
	short delay = 0;

	void Update () {
		delay++;

		if(delay > 8)
		{
			light.intensity = Random.value;
			delay = -0;
		}
	}
}