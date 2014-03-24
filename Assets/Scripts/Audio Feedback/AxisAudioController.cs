using UnityEngine;
using System.Collections;

/**
 * Author : Thomas Rouvinez.
 */
public class AxisAudioController : MonoBehaviour {

	// -----------------------------------------------------------------------------
	// Variables.
	// -----------------------------------------------------------------------------
	
	public AudioClip[] fire;
	public AudioClip[] orderHit;
	public AudioClip[] orderMiss;
	public AudioClip[] hit;
	public AudioClip[] miss;
	public AudioClip[] attacked;
	
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
	
	// Great shot !!!
	public void Hit()
	{
		this.GetComponent<AudioSource>().clip = hit[Random.Range(0, hit.Length)];
		this.GetComponent<AudioSource>().Play();
	}
	
	// Shot miss, you noob !!!
	public void Miss()
	{
		this.GetComponent<AudioSource>().clip = miss[Random.Range(0, miss.Length)];
		this.GetComponent<AudioSource>().Play();
	}
	
	// Under attack !!!
	public void Attacked()
	{
		this.GetComponent<AudioSource>().clip = attacked[Random.Range(0, attacked.Length)];
		this.GetComponent<AudioSource>().Play();
	}
}