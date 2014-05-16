using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class PlayController : MonoBehaviour {

	Color textMeshColor;

	void Start()
	{
		textMeshColor = GetComponent<TextMesh> ().color;
	}
	
	void OnMouseOver() 
	{
		GetComponent<TextMesh> ().color = Color.red;
	}
	
	void OnMouseExit()
	{
		GetComponent<TextMesh> ().color = textMeshColor;
	}

	void OnMouseDown()
	{
		Application.LoadLevel(1);
	}
}