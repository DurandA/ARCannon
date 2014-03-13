using UnityEngine;
using System.Collections;

public class CannonBehaviour : MonoBehaviour {

	// -----------------------------------------------------------------------------
	// Variables.
	// -----------------------------------------------------------------------------

	public Transform output;
	public Transform shaft;
	public Transform deck;
	public Rigidbody projectile;

	// Trace.
	private Vector3[] trace=new Vector3[256];
	private LineRenderer lineRenderer;

	#if UNITY_EDITOR
	string xRot = "0";
	string yRot = "0";
	#endif

	// -----------------------------------------------------------------------------
	// Unity life cycle.
	// -----------------------------------------------------------------------------

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

	public IEnumerator MoveTowards (Vector2 direction, float playbackSpeed){
		float t = Time.time;
		float u = Time.time;

		Quaternion initialShaftRot = shaft.localRotation;
		Quaternion initialDeckRot = deck.localRotation;

		while ((u - t) * playbackSpeed<1f) {
			shaft.localRotation = Quaternion.Lerp (initialShaftRot, initialShaftRot * Quaternion.Euler(direction.x,0f,0f), (u - t) * playbackSpeed);
			deck.localRotation = Quaternion.Lerp (initialDeckRot, initialDeckRot * Quaternion.Euler(0f,0f,direction.y), (u - t) * playbackSpeed);
			u = Time.time;
			yield return 0;
		}
	}

	// -----------------------------------------------------------------------------
	// GUI.
	// -----------------------------------------------------------------------------

	void OnGUI () {
		#if UNITY_EDITOR
		xRot = GUI.TextField(new Rect(10, 80, 40, 20), xRot, 3);
		yRot = GUI.TextField(new Rect(60, 80, 40, 20), yRot, 3);

		if (GUI.Button (new Rect (120, 60, 50, 50), "Rotate")) {
			StartCoroutine(MoveTowards(new Vector2(int.Parse(xRot),int.Parse(yRot)),1f));
		}
		#endif
	}

	// -----------------------------------------------------------------------------
	// Firing functions.
	// -----------------------------------------------------------------------------

	public void Fire (float power){
		Rigidbody clone;
		clone = Instantiate(projectile, output.position, output.rotation) as Rigidbody;
		clone.transform.localScale = projectile.transform.localScale;
		clone.velocity = output.TransformDirection(Vector3.forward * power);

		StartCoroutine(TraceProjectile(clone.transform));
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