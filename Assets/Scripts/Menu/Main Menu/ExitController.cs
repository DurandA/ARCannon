using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class ExitController : MonoBehaviour {
	
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
		Application.Quit();
	}
}