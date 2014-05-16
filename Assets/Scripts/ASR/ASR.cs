using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Creation date : 03.01.2014
 * Last modified : 03.11.2014
 * 
 * Description : class to handle calls to the Google ASR API.
 */
using System.Collections.Generic;

[RequireComponent(typeof(GameManager))]
public class ASR : MonoBehaviour {

	// -----------------------------------------------------------------------------
	// Variables.
	// -----------------------------------------------------------------------------
	
	private bool dbLevelPushed = false;
	private bool dbLevelToggled = false;
	private float dbLevelSmoothed = 0f;	//<-- use this for the level amplitude bar.
	private float dbLevelNormalized{
		get{return Mathf.Clamp01(dbLevelSmoothed/3500);}
	}
	private Queue<float> buff;
	private int Count=0; float Sum=0;
	private float rawLevel = 0f;

	private AndroidJavaClass unityPlayer;
	private AndroidJavaObject activity;

	public AudioSource audio;
	private AxisAudioController axisSounds;
	private AlliesAudioController alliesSounds;

	public Texture2D microphone;
	public Texture2D cannon;

	// -----------------------------------------------------------------------------
	// Load Android ASR plugin.
	// -----------------------------------------------------------------------------
	
	void Start () {
		unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		buff = new Queue<float>(32);

		axisSounds = audio.GetComponent<AxisAudioController>();
		alliesSounds = audio.GetComponent<AlliesAudioController>();
		DebugStreamer.message = "Hello world!";

	}

	void Update (){
		GameManager manager = GetComponent<GameManager> ();
		if (!dbLevelToggled && dbLevelPushed) {
			DebugStreamer.message="startAudioRecorder";
			startAudioRecorder();
		}
		if (dbLevelToggled && !dbLevelPushed) {
			DebugStreamer.message="stopAudioRecorder";
			stopAudioRecorder();
			manager.currentCannon.Fire (dbLevelNormalized*20f, manager.nextCannon.transform);
			StartCoroutine (manager.SwitchCannon ());
		}
		if ((dbLevelToggled && dbLevelPushed) )
			activity.Call("getAmplitude", "");
		if ((dbLevelPushed))
			manager.currentCannon.powerBar.SetPower (dbLevelNormalized);

		dbLevelToggled = dbLevelPushed;
	}

	void OnGUI(){
		GameManager manager = GetComponent<GameManager> ();
		if (!manager.isSwitching) {
			/*if (dbLevelPushed){
				float boxHeight = dbLevelNormalized*Screen.height-20;
				GUI.Box(new Rect(Screen.width/4,Screen.height-10-boxHeight,Screen.width/6,boxHeight),"Level");
			}*/
			if (GUI.Button (new Rect (Screen.width - Screen.width / 6, Screen.height - Screen.height / 6, Screen.width / 8, Screen.height / 8), "Give order"))
				startASR ();
			dbLevelPushed = GUI.Toggle (new Rect (10, Screen.height - Screen.height / 6, Screen.width / 8, Screen.height / 8), dbLevelPushed, dbLevelPushed ? cannon : microphone);
			if (dbLevelPushed) {
					GUI.Label (new Rect (0, 0, 100, 40), dbLevelSmoothed+"");
			}
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
		activity.Call("startDBMeter", "");
	}

	void stopAudioRecorder()
	{
		activity.Call("stopDBMeter", "");
	}

	// -----------------------------------------------------------------------------
	// Asynchronous returns.
	// -----------------------------------------------------------------------------

	private void storeAmplitude(string msg)
	{
		float dbLevel = float.Parse(msg);
		rawLevel = dbLevel;
		buff.Enqueue (dbLevel);
		if (Count <= 28) {
			Sum += dbLevel;
			Count++;
		}
		else
			Sum+=dbLevel-buff.Dequeue();

		dbLevelSmoothed = Sum / Count;
		Debug.Log (dbLevelSmoothed);
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
			CannonBehaviour currentCannon = GetComponent<GameManager>().currentCannon;
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
						x += order[i].getAngle()* (-1);
					}
					else
					{
						x += order[i].getAngle();
					}
				}
				else if(order[i].getOrientation() == 1 || order[i].getOrientation() == -1)
				{
					// Set direction on x.
					if(order[i].getOrientation() > 0)
					{
						y += order[i].getAngle()* (-1);
					}
					else
					{
						y += order[i].getAngle();
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