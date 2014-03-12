using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Creation date : 03.01.2014
 * Last modified : 03.11.2014
 * 
 * Description : class to handle calls to the Google ASR API.
 */
public class ASR : MonoBehaviour {

	// -----------------------------------------------------------------------------
	// Variables.
	// -----------------------------------------------------------------------------

	//public GUIText stext;
	//public GUIText dbLevelStr;
	private bool dbLevelOn = false;
	private double dbLevel = 0f;

	private string msg = "Hello world";

	private AndroidJavaClass unityPlayer;
	private AndroidJavaObject activity;

	// -----------------------------------------------------------------------------
	// Load Android ASR plugin.
	// -----------------------------------------------------------------------------
	
	void Start () 
	{
		unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
	}

	void Update()
	{
		if (dbLevelOn == true)
		{
			activity.Call("getAmplitude", "");
		}
	}

	void OnGUI(){

		GUI.Button(new Rect(10, Screen.height - 100, (int) (dbLevel / 10), 300), "");

		if (GUI.Button(new Rect(10, 200, 500, 300), "START ASR"))
		{
			startASR();
		}

		if (GUI.Button(new Rect(10, 570, 500, 300), "GET DB LEVEL"))
		{
			if(dbLevelOn == false)
			{
				startAudioRecorder();
			}
			else
			{
				stopAudioRecorder();
			}
		}
		GUI.Label (new Rect(200, 10, 100, 20), msg);
	}

	// -----------------------------------------------------------------------------
	// Function.
	// -----------------------------------------------------------------------------

	void startASR()
	{
		activity.Call("startASR", "");
	}

	void startAudioRecorder()
	{
		activity.Call("startDBMeter", "");
		dbLevelOn = true;
	}

	void stopAudioRecorder()
	{
		activity.Call("stopDBMeter", "");
		dbLevelOn = false;
	}

	// -----------------------------------------------------------------------------
	// Asynchronous returns.
	// -----------------------------------------------------------------------------

	private void storeAmplitude(string msg)
	{
		this.dbLevel = float.Parse(msg);
	}

	private void onDebugFromPlugin(string msg)
	{
		//stext.text = "DEBUG : " + msg;
	}
	
	private void onReadyForSpeech(string msg)
	{
		//stext.text = msg;
	}

	private void onBeginningOfSpeech(string msg)
	{
		//stext.text = msg;
	}

	private void onEndOfSpeech(string msg)
	{
		//stext.text = msg;
	}

	private void onSpeechCancelled(string msg)
	{
		//stext.text = msg;
	}

	private void onResultsReceived(string msg)
	{
		this.msg = msg;
		//Regex regex =  new Regex();
		//stext.text = "Results : " + msg + " || Type : " + regex.interpret(msg);
	}

	private void onErrorReceived(string msg)
	{
		//stext.text = "Error : " + msg;
	}
}