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
	public GameObject topSideBolt;
	public GameObject bottomSideBolt;
	public Transform bottomSideSpawn;
	public Transform topSideSpawn;

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

	// side gun
	public float sideGunSize;

	// to keep track of button presses
	// TODO, add this
	private bool fireButtonPressed;

	void Start() {
		audioSource = GetComponent<AudioSource> ();

	}

	//code that is run for every frame
	void Update() {

		if (Input.anyKey) {
			foreach (char c in Input.inputString) {
				Debug.Log (c);
			}
		}
		for (int i = 0;i < 20; i++) {
			if(Input.GetKeyDown("joystick 1 button "+i)){
				print("joystick 1 button "+i);
			}
		}
		if (Input.GetKeyDown ("joystick button 0")) {
			Debug.Log ("button 1 pressed");
		}
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			fireButtonPressed = true;
			timeBetweenBolts = 1 / shotsPerSecond;
			// bolts
			nextFire = Time.time + timeBetweenBolts;
			for (int i = 0; i < activeBolts; i++) {
				Instantiate (bolts [i], boltSpawns [i].position, boltSpawns [i].rotation);
			}
			audioSource.Play ();
			// side gun
			if (sideGunSize > 0) {
				GameObject topBolt = Instantiate (topSideBolt, topSideSpawn.position, topSideSpawn.rotation) as GameObject;  
				GameObject bottomBolt = Instantiate (bottomSideBolt, bottomSideSpawn.position, bottomSideSpawn.rotation) as GameObject;
				// TODO, make it so the side bolts can have their sizes changed...
				Vector3 scale = new Vector3 (1.0f, 1.0f, sideGunSize);
				topBolt.transform.localScale = scale;
				bottomBolt.transform.localScale = scale;
			}
		} else if (Input.GetButton ("Fire1") == false) {
			fireButtonPressed = false;
		} else if (fireButtonPressed == false && Input.GetButton ("Fire1")) {
			fireButtonPressed = true;
			timeBetweenBolts = 1 / shotsPerSecond;
			// bolts
			nextFire = Time.time + timeBetweenBolts;
			for (int i = 0; i < activeBolts; i++) {
				Instantiate (bolts [i], boltSpawns [i].position, boltSpawns [i].rotation);
			}
			audioSource.Play ();
			// side gun
			if (sideGunSize > 0) {
				GameObject topBolt = Instantiate (topSideBolt, topSideSpawn.position, topSideSpawn.rotation) as GameObject;  
				GameObject bottomBolt = Instantiate (bottomSideBolt, bottomSideSpawn.position, bottomSideSpawn.rotation) as GameObject;
				// TODO, make it so the side bolts can have their sizes changed...
				Vector3 scale = new Vector3 (1.0f, 1.0f, sideGunSize);
				topBolt.transform.localScale = scale;
				bottomBolt.transform.localScale = scale;
			}
		}else if (Input.GetButton ("Fire1")) {
			fireButtonPressed = true;
		}
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
