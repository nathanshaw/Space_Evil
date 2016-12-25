using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

// make public class for all of the player stats

public class PlayerController : MonoBehaviour 
{
	public Boundary boundary;
	public float speed;
	public float vTilt;
	public float hTilt;
	public GameObject[] bolts;
	public Transform[] boltSpawns;

	// this is number of bolts a second...
	public float shotsPerSecond;
	private float timeBetweenBolts;

	public float maxHitPoints;
	public float currentHitPoints;
	public float boltDamage;
	public int activeBolts;

	private float nextFire;
	private AudioSource audioSource;

	void Start() {
		audioSource = GetComponent<AudioSource> ();
	}

	//code that is run for every frame
	void Update() {
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			timeBetweenBolts = 1 / shotsPerSecond;
			nextFire = Time.time + timeBetweenBolts;
			for (int i = 0; i < activeBolts; i++) {
				Instantiate (bolts[i], boltSpawns[i].position, boltSpawns[i].rotation);
			}
			audioSource.Play ();
		};
			// TODO make it so you can do manual fireing
	}

	// executed once per physics step
	void FixedUpdate()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical); 
		rigidbody.velocity = movement * speed;
		rigidbody.position = new Vector3
		(
			Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
		rigidbody.rotation = Quaternion.Euler(rigidbody.velocity.x * hTilt, 90, rigidbody.velocity.z * vTilt);
	}
}
