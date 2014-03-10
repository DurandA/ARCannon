using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {

	public Transform target;
	public Transform blastDecal;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < (target!=null?target.position.y:0f))
			Destroy (gameObject);
	}

	void OnDestroy(){
		Transform blast = Instantiate (blastDecal, transform.position, Quaternion.identity) as Transform;
		blast.localScale = blastDecal.localScale;
	}
}
