using UnityEngine;
using System.Collections;

public class FocusMode : MonoBehaviour {

	public CameraDevice.FocusMode focusMode = CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO;

	// Use this for initialization
	void Start () {
		if (CameraDevice.Instance.SetFocusMode (focusMode))
			Debug.LogWarning (focusMode+" not available!");


	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
