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

	public AudioSource audio;
	private AudioController sounds;

	// -----------------------------------------------------------------------------
	// Load Android ASR plugin.
	// -----------------------------------------------------------------------------
	
	void Start () 
	{
		unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		sounds = audio.GetComponent<AudioController>();
	}

	void Update()
	{

	}

	void OnGUI(){
		
		if (GUI.Button(new Rect(Screen.width - 410, Screen.height - 450, 400, 400), "START ASR"))
		{
			startASR();
		}

		else if (GUI.Button (new Rect (10, Screen.height - 450, 400, 400), "Fire")) {
			sounds.Fire();
			currentCannon.Fire(80f);
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

	// Called when the ASR results are received.
	private void onResultsReceived(string msg)
	{
		Regex regex =  new Regex();
		Order[] order = regex.interpret(msg);

		if(order != null)
		{
			currentCannon.StartCoroutine(currentCannon.MoveTowards(orderToVector2D(order),1f));
		}
	}

	private void onErrorReceived(string msg)
	{

	}

	// -----------------------------------------------------------------------------
	// Order to vector 2D.
	// -----------------------------------------------------------------------------

	private Vector2 orderToVector2D(Order[] order)
	{
		Vector2 vector = new Vector2();
		float x = 0.0f;
		float y = 0.0f;

		// Combine orders if many available.
		for(int i = 0 ; i < order.Length ; i++)
		{
			if(order[i].getOrientation() != -6)
			{
				// Order understood.
				if(order[i].getOrientation() == 2 || order[i].getOrientation() == -2)
				{
					// Set direction on y.
					if(order[i].getOrientation() > 0)
					{
						y += order[i].getAngle()* (-1);
					}
					else
					{
						y += order[i].getAngle();
					}
				}
				else if(order[i].getOrientation() == 1 || order[i].getOrientation() == -1)
				{
					// Set direction on x.
					if(order[i].getOrientation() > 0)
					{
						x += order[i].getAngle()* (-1);
					}
					else
					{
						x += order[i].getAngle();
					}
				}

				// Understood = true;
				sounds.OrderHit();
			}
			else
			{
				// Understood = false;
				sounds.OrderMiss();
			}
		}

		// Return the commands for the cannon.
		vector.Set(x, y);
		return vector;
	}
}