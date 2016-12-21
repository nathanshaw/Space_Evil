using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float initialDelay;  
	public float fireRateRandomRange;
	private float halfRange;

	private AudioSource audioSource;

	void Start() {
		audioSource = GetComponent<AudioSource> ();
		halfRange = fireRateRandomRange * 0.5f;
		InvokeRepeating (
			"Fire", 
			initialDelay, 
			Random.Range(
				fireRate - halfRange,
				fireRate + halfRange)
		);

	}

	void Fire () {
		Instantiate (
			shot, 
			shotSpawn.transform.position, 
			shotSpawn.transform.rotation
		);
		audioSource.Play ();
	}
}
