using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ASR))]
public class GameManager : MonoBehaviour {
	
	public CannonBehaviour[] cannons;
	private int currentCannonIdx = 0;
	private bool _isSwitching = false;
	public GUIStyle style;

	public float timer = 90f;
	#if UNITY_EDITOR
	string xRot = "0";
	string yRot = "0";
	#endif
	
	public CannonBehaviour currentCannon
	{
		get {return cannons[currentCannonIdx];}
	}

	public CannonBehaviour nextCannon
	{
		get {return cannons[(currentCannonIdx+1)%cannons.Length];}
	}

	public bool isSwitching
	{
		get {return _isSwitching;}
	}
	
	void Start(){
		currentCannon.controlEnabled=true;
	}

	void Update(){
		if (!isSwitching) {
			timer -= Time.deltaTime;
			if (timer < 0f)	
				StartCoroutine (SwitchCannon ());
		}
		if (!isSwitching && Input.GetButton("Fire1")){
			currentCannon.Fire((Input.GetAxis("Z Axis")+1f)*20f, nextCannon.transform);
			StartCoroutine(SwitchCannon());
		}
	}

	void OnGUI(){
		#if UNITY_EDITOR
		xRot = GUI.TextField(new Rect(10, 80, 40, 20), xRot, 3);
		yRot = GUI.TextField(new Rect(60, 80, 40, 20), yRot, 3);

		if (GUI.Button (new Rect (120, 60, 50, 50), "Rotate")) {
			currentCannon.StartCoroutine(currentCannon.MoveTowards(new Vector2(int.Parse(xRot),int.Parse(yRot)),1f));
		}
		#endif
		if(!isSwitching)
			GUI.Label (new Rect (Screen.width-80,20,40,20), ((int)timer).ToString(), style);
		if (GUI.Button (new Rect(10, 10, Screen.width/8, Screen.height/8),"<--"))
		    Application.LoadLevel(0);
		GUI.Label (new Rect (Screen.width/2-100,20,200,20), currentCannon.name + " " + currentCannon.hits + " : " + nextCannon.hits + " " + nextCannon.name, style);
	}

	public IEnumerator SwitchCannon(){
		currentCannon.controlEnabled = false;
		_isSwitching = true;
		currentCannonIdx = (++currentCannonIdx < cannons.Length) ? currentCannonIdx : 0;
		yield return new WaitForSeconds (3f);
		timer = 30f;
		_isSwitching = false;
		currentCannon.controlEnabled = true;
		//#if not UNITY_EDITOR
		//Changing the world center is not supported at runtime
		//Camera.main.GetComponent<QCARBehaviour> ().SetWorldCenter (currentCannon.transform.parent.GetComponent<ImageTargetBehaviour>());
		//#endif
	}


}
