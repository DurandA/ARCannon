using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ASR))]
public class GameManager : MonoBehaviour {
	
	public CannonBehaviour[] cannons;
	private int currentCannonIdx = 0;
	private bool _isSwitching = false;

	private float timer = 30f;
	#if UNITY_EDITOR
	string xRot = "0";
	string yRot = "0";
	#endif
	
	public CannonBehaviour currentCannon
	{
		get {return cannons[currentCannonIdx];}
	}

	public bool isSwitching
	{
		get {return _isSwitching;}
	}
	
	void Start(){
		currentCannon.enabled=true;
	}

	void Update(){
		if (!isSwitching) {
			timer -= Time.deltaTime;
			if (timer < 0f)	
				StartCoroutine (SwitchCannon ());
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
			GUI.Label (new Rect (Screen.width-80,80,40,20), timer.ToString());
		if (!isSwitching && GUI.Button (new Rect (10, Screen.height - Screen.height/6, Screen.width/8, Screen.height/8),"FIRE")){
			currentCannon.Fire(Input.GetAxis("Z Axis"));
			StartCoroutine(SwitchCannon());
		}

	}

	IEnumerator SwitchCannon(){
		_isSwitching = true;
		currentCannonIdx = (++currentCannonIdx < cannons.Length) ? currentCannonIdx : 0;
		yield return new WaitForSeconds (3f);
		timer = 30f;
		_isSwitching = false;
	}


}
