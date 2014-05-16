using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
public class PowerBarBehaviour : MonoBehaviour {

	public int segmentCount;
	public float radius = 4f;
	//public Color startColor;
	//public Color endColor;

	private LineRenderer lineRenderer;

	public bool display {
		get { return lineRenderer.enabled; }
		set { lineRenderer.enabled = value; }
	}
	
	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		//lineRenderer.SetVertexCount(segments + 1);
		//CreatePoints (xradius, yradius, segments);
	}

	public void SetPower(float power){
		//startColor.a = power;
		//endColor.a = power-0.5f;
		//lineRenderer.SetColors (startColor, endColor);
		int segments = (int)(power * segmentCount);
		lineRenderer.SetVertexCount (segments+1);
		CreatePoints (radius, segments, segmentCount);
	}

	public void Enable(){
		lineRenderer.enabled = true;
	}

	public void Disable(){
		lineRenderer.enabled = false;
	}

	void CreatePoints (float radius, int segments, int segmentCount)	
	{
		float x;	
		float y;
		float z = 0f;
		
		float angle = 20f;
		
		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;	
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;
			
			lineRenderer.SetPosition (i,new Vector3(x,y,z) );
			
			angle += (360f / segmentCount);
		}
	}
}
