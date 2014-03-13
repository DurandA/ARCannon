using UnityEngine;
using System.Collections;

public class PlayController : MonoBehaviour {

	void OnStart()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	// Hover system.
	void OnMouseOver() 
	{
		renderer.material.color = Color.yellow;
	}
	
	void OnMouseExit()
	{
		renderer.material.color = Color.white;
	}
	
	// Click system.
	void OnMouseUp()
	{
		Application.LoadLevel(1);
	}
}