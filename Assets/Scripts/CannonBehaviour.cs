using UnityEngine;
using System.Collections;

public class CannonBehaviour : MonoBehaviour {

	public Rigidbody projectile;
	private Vector3[] trace=new Vector3[256];
	private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, Color.yellow);
		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		if (GUI.Button (new Rect (10, 10, 50, 50), "Fire")) {
			Rigidbody clone;
			clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
			clone.transform.localScale = projectile.transform.localScale;
			clone.velocity = transform.TransformDirection(Vector3.forward * 10);
			Debug.Log("Clicked the button with an image");
			StartCoroutine(TraceProjectile(clone.transform));
		}
	}

	IEnumerator TraceProjectile (Transform projectile) {
		int i = 0;
		while (projectile!=null&&i<trace.Length) {
			trace [i] = projectile.position;
			i++;
			yield return 0;
		}
		lineRenderer.SetVertexCount(i);
		for (int j=0; j<i; j++) {
			lineRenderer.SetPosition (j, trace [j]);
		}
		lineRenderer.enabled = true;
	}
}
