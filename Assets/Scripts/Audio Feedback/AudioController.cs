using UnityEngine;
using System.Collections;

/**
 * Author : Thomas Rouvinez.
 */
public class AudioController : MonoBehaviour {

	// -----------------------------------------------------------------------------
	// Variables.
	// -----------------------------------------------------------------------------

	public AudioClip[] fire;
	public AudioClip[] orderHit;
	public AudioClip[] orderMiss;
	
	// -----------------------------------------------------------------------------
	// Audio requests.
	// -----------------------------------------------------------------------------

	// Fire !!!
	public void Fire()
	{
		this.GetComponent<AudioSource>().clip = fire[Random.Range(0, fire.Length)];
		this.GetComponent<AudioSource>().Play();
	}

	// Firing for effect.
	public void OrderHit()
	{
		this.GetComponent<AudioSource>().clip = orderHit[Random.Range(0, orderHit.Length)];
		this.GetComponent<AudioSource>().Play();
	}

	
	// Order is not recognized.
	public void OrderMiss()
	{
		this.GetComponent<AudioSource>().clip = orderMiss[Random.Range(0, orderMiss.Length)];
		this.GetComponent<AudioSource>().Play();
	}
}