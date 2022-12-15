using UnityEngine;

public class WeaponController : MonoBehaviour {

	public GameObject shot;
	public Transform[] shotSpawn;
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
		for (int i = 0; i < shotSpawn.Length; i++) {
			Instantiate (
				shot, 
				shotSpawn[i].transform.position, 
				shotSpawn[i].transform.rotation
			);
		}
		fireWeaponAudioSource.Play ();
	}
}
