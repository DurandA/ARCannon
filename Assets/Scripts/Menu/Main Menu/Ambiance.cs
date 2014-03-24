using UnityEngine;
using System.Collections;

public class Ambiance : MonoBehaviour {

	public Light light;
	private float minTime = .5f;
	private float threshold = .5f;
	private float lastTime = .0f;
	private float[] smoothing = new float[20];

	void Start(){
		// Initialize the array.
		for(int i = 0 ; i < smoothing.Length ; i++){
			smoothing[i] = .0f;
		}
	}

	void Update () {
		float sum = .0f;
		
		// Shift values in the table.
		for(int i = 1 ; i < smoothing.Length ; i++)
		{
			smoothing[i-1] = smoothing[i];
			sum+= smoothing[i-1];
		}
		
		smoothing[smoothing.Length -1] = Random.value;
		sum+= smoothing[smoothing.Length -1];

		light.intensity = sum / smoothing.Length;
	}
}