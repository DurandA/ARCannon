using UnityEngine;
using System.Collections;

public class PlayController : MonoBehaviour {

	Color textMeshColor;

	void Start()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		textMeshColor = GetComponent<TextMesh> ().color;
	}

	// Hover system.
	void OnMouseOver() 
	{
		GetComponent<TextMesh> ().color = Color.yellow;
	}
	
	void OnMouseExit()
	{
		GetComponent<TextMesh> ().color = textMeshColor;
	}
	
	// Click system.
	void OnMouseDown()
	{
		Application.LoadLevel(1);
	}
}