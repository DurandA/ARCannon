using UnityEngine;
using System.Collections;

public class BlastDecay : MonoBehaviour {

	public float initialDelay;
	private float t;

	// Use this for initialization
	void Start () {
		t = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time>t+initialDelay){
			if (Time.time<t+initialDelay+5f)
				renderer.material.color=new Color(renderer.material.color.r,renderer.material.color.g,renderer.material.color.b,1-(Time.time-t-initialDelay)/5);
			else
				Destroy(gameObject);
		}
	}
}
