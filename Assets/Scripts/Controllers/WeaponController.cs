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

	public AudioSource fireWeaponAudioSource;

	void Start() {
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
		fireWeaponAudioSource.Play ();
	}
}
