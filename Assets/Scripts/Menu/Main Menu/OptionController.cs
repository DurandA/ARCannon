using UnityEngine;
using System.Collections;

public class OptionController : MonoBehaviour {
	
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
		//Application.LoadLevel(1);
	}
}