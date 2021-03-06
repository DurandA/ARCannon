﻿using UnityEngine;
using System.Collections;

public class CannonBehaviour : MonoBehaviour {

	public Transform output;
	public Transform shaft;
	public Transform deck;
	public Rigidbody projectile;
	public Transform fire;

	public bool controlEnabled=false;

	public Vector2 joyDirection;
	public Vector2 asyncDirection;

	public PowerBarBehaviour powerBar;
	
	private Vector3[] trace=new Vector3[256];
	private LineRenderer lineRenderer;

	public int hits=0;

	// Use this for initialization
	void Start () {
		/*lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, Color.yellow);
		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.useWorldSpace = false;
		lineRenderer.enabled = false;*/;
	}
	
	// Update is called once per frame
	void Update () {
		if (controlEnabled) {
			joyDirection = new Vector2 (Input.GetAxisRaw ("X Axis") * 180f, Input.GetAxisRaw ("Y Axis") * 180f);
			deck.localRotation = Quaternion.Euler (0f, 0f, joyDirection.x) * Quaternion.Euler (0f, 0f, asyncDirection.x);						shaft.localRotation = Quaternion.Euler (joyDirection.y, 0f, 0f) * Quaternion.Euler (asyncDirection.y, 0f, 0f);
			powerBar.display = true;
			powerBar.SetPower ((Input.GetAxisRaw ("Z Axis") + 1) / 2);
		}
		else
			powerBar.display = false;
	}

	public IEnumerator MoveTowards (Vector2 direction, float playbackSpeed){
		float t = Time.time;
		float u = Time.time;

		Vector2 initialDirection = new Vector2 (asyncDirection.x, asyncDirection.y);
		while ((u - t) * playbackSpeed<1f) {
			asyncDirection = Vector2.Lerp (initialDirection, initialDirection + direction, (u - t) * playbackSpeed);

			u = Time.time;
			yield return 0;
		}
	}

	public void Fire (float power, Transform target){
		Rigidbody clone;
		clone = Instantiate(projectile, output.position, output.rotation) as Rigidbody;
		clone.transform.localScale = projectile.transform.localScale;
		clone.velocity = output.TransformDirection(Vector3.forward * power);
		clone.GetComponent<ProjectileBehaviour> ().target = target;
		//StartCoroutine(TraceProjectile(clone.transform));
	}

	public void OnTriggerEnter (Collider other) {
		if (other.GetComponent<ProjectileBehaviour> ()) {
			hits++;
			Transform fire = Instantiate(this.fire,Vector3.Lerp(other.transform.position,transform.position,0.6f),Quaternion.identity) as Transform;
			fire.parent=transform;
			Destroy(other.gameObject);
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