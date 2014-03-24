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
	private double dbLevelSmoothed = 0f;	//<-- use this for the level amplitude bar.
	private double[] levelSmoothing;

	private AndroidJavaClass unityPlayer;
	private AndroidJavaObject activity;

	public CannonBehaviour currentCannon;

	public AudioSource audio;
	private AxisAudioController axisSounds;
	private AlliesAudioController alliesSounds;

	// -----------------------------------------------------------------------------
	// Load Android ASR plugin.
	// -----------------------------------------------------------------------------
	
	void Start () 
	{
		unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		levelSmoothing = new double[5];

		for(int i = 0 ; i < levelSmoothing.Length ; i++)
		{
			levelSmoothing[i] = .0d;
		}

		axisSounds = audio.GetComponent<AxisAudioController>();
		alliesSounds = audio.GetComponent<AlliesAudioController>();
	}

	void OnGUI(){

		dbLevelOn = false;
		
		if (GUI.Button(new Rect(Screen.width - 410, Screen.height - 450, 400, 400), "START ASR"))
		{
			startASR();
		}

		else if (GUI.Button (new Rect (10, Screen.height - 450, 400, 400), "Fire")) {
			axisSounds.Fire();
			currentCannon.Fire(80f);
		}
	}

	// -----------------------------------------------------------------------------
	// Audio functions.
	// -----------------------------------------------------------------------------

	void startASR()
	{
		activity.Call("startASR", "");
	}

	void startAudioRecorder()
	{
		while(dbLevelOn == true)
		{
			activity.Call("startDBMeter", "");
		}
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

		double sum = .0d;
		
		// Shift values in the table.
		for(int i = 1 ; i < levelSmoothing.Length ; i++)
		{
			levelSmoothing[i-1] = levelSmoothing[i];
			sum+= levelSmoothing[i-1];
		}
		
		levelSmoothing[levelSmoothing.Length -1] = dbLevel;
		sum += dbLevel;
		
		// Compute the average.
		dbLevelSmoothed = sum / levelSmoothing.Length;
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
				axisSounds.OrderHit();
			}
			else
			{
				// Understood = false;
				axisSounds.OrderMiss();
			}
		}

		// Return the commands for the cannon.
		vector.Set(x, y);
		return vector;
	}
}