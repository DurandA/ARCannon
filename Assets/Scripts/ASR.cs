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
	
	private bool dbLevelOn = false;
	private double dbLevel = 0f;

	private AndroidJavaClass unityPlayer;
	private AndroidJavaObject activity;

	public CannonBehaviour currentCannon;

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

	}

	void OnGUI(){
		
		if (GUI.Button(new Rect(10, 200, 500, 300), "START ASR"))
		{
			startASR();
		}
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

	}
	
	private void onReadyForSpeech(string msg)
	{

	}

	private void onBeginningOfSpeech(string msg)
	{

	}

	private void onEndOfSpeech(string msg)
	{

	}

	private void onSpeechCancelled(string msg)
	{

	}

	private void onResultsReceived(string msg)
	{
		Regex regex =  new Regex();
		Order[] order = regex.interpret(msg);

		if(order != null)
		{
			for(int i = 0 ; i < order.Length ; i++)
			{
				if(order[i].getOrientation() != -6)
				{
					currentCannon.StartCoroutine(currentCannon.MoveTowards(orderToVector2D(order[i]),1f));
				}

			}
		}
	}

	private void onErrorReceived(string msg)
	{

	}

	// -----------------------------------------------------------------------------
	// Order to vector 2D.
	// -----------------------------------------------------------------------------

	private Vector2 orderToVector2D(Order order)
	{
		Vector2 vector = new Vector2();
		float x = 0.0f;
		float y = 0.0f;

		if(order.getOrientation() == 2 || order.getOrientation() == -2)
		{
			if(order.getOrientation() > 0)
			{
				y = order.getAngle()* (-1);
			}
			else
			{
				y = order.getAngle();
			}
		}
		else
		{
			if(order.getOrientation() > 0)
			{
				x = order.getAngle()* (-1);
			}
			else
			{
				x = order.getAngle();
			}
		}

		vector.Set(x, y);
		return vector;
	}
}