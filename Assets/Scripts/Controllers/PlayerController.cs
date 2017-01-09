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
	public GameController gc;
	public Transform bottomSideSpawn;
	public Transform topSideSpawn;

	public Transform[] boltSpawns;

	// Player Attributes
	public float shotsPerSecond;
	private float timeBetweenBolts;
	private float manualTimeBetweenBolts;
	public float maxHitPoints;
	public float currentHitPoints;
	// bolt gun
	public float boltDamage;
	public float startingBoltDamage;
	public float maxBoltDamage;
	public int activeBolts;
	// side gun
	public float sideGunSize;
	public float sideGunDamage;
	public float startingSideDamage;
	public float maxSideDamage;
	// to keep track of fireing
	private float nextAutoFire;
	private float nextManualFire;
	private bool fireButtonPressed;
	private bool startButtonPressed;

	private AudioSource audioSource;


	void Start() {
		audioSource = GetComponent<AudioSource> ();
		resetBoltDamage ();
	}

	//code that is run for every frame
	void Update() {
		// see if the game has been paused
		if (Input.GetButton ("Start")) {
			if (startButtonPressed == false) {
				startButtonPressed = true;
				gc.RequestPause ();
			}
		} else if (startButtonPressed){
			startButtonPressed = false;
		}
		// see if the player is attempting to shoot a weapon
		if (Input.GetButton ("Fire1") && Time.time > nextAutoFire) {
			fireButtonPressed = true;
			timeBetweenBolts = 1 / shotsPerSecond;
			// bolts
			nextAutoFire = Time.time + timeBetweenBolts;
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
		} else if (fireButtonPressed == false && 
				   Input.GetButton ("Fire1") &&
				   Time.time > nextManualFire) 
			{
			fireButtonPressed = true;
			timeBetweenBolts = 1 / shotsPerSecond;
			manualTimeBetweenBolts = 1 / (shotsPerSecond * 2);
			nextAutoFire = Time.time + timeBetweenBolts;
			nextManualFire = Time.time + manualTimeBetweenBolts;
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

	public float ChangeBoltDamage (float damChange) {
		// get all the bolts and update their damage;
		// returns the damage of the first bolt
		float updatedDamage = 0.0f;
		for (int i = 0; i < bolts.Length; i++) {
			DestroyByCollisionPlayerWeapon dbcPW = bolts[i].GetComponent<DestroyByCollisionPlayerWeapon>() as DestroyByCollisionPlayerWeapon;
			dbcPW.updateDamage (damChange);
			if (i == 0) {
				updatedDamage = dbcPW.Damage;
			}
		}
		return updatedDamage;
	}

	public float ChangeSideDamage (float damChange) {
		// get all the bolts and update their damage;
		// returns the damage of the first bolt
		float updatedDamage = 0.0f;
		DestroyByCollisionPlayerWeapon dbcPWb = bottomSideBolt.GetComponent<DestroyByCollisionPlayerWeapon>() as DestroyByCollisionPlayerWeapon;
		dbcPWb.updateDamage (damChange);
		DestroyByCollisionPlayerWeapon dbcPWt = topSideBolt.GetComponent<DestroyByCollisionPlayerWeapon>() as DestroyByCollisionPlayerWeapon;
		dbcPWt.updateDamage (damChange);
		updatedDamage = dbcPWt.Damage;
		return updatedDamage;
	}

	private void resetBoltDamage () {
		for (int i = 0; i < bolts.Length; i++) {
			DestroyByCollisionPlayerWeapon dbcPW = bolts[i].GetComponent<DestroyByCollisionPlayerWeapon>() as DestroyByCollisionPlayerWeapon;
			dbcPW.Damage = startingBoltDamage;
			dbcPW.MaxDamage = maxBoltDamage;
			dbcPW.StartingDamage = startingBoltDamage;
		}
	}

	private void resetSideDamage () {
		// get dbcpsw for top and bottom and reset their values
	DestroyByCollisionPlayerWeapon dbcPWb = bottomSideBolt.GetComponent<DestroyByCollisionPlayerWeapon>() as DestroyByCollisionPlayerWeapon;
			dbcPWb.Damage = startingSideDamage;
			dbcPWb.MaxDamage = maxSideDamage;
			dbcPWb.StartingDamage = startingSideDamage;
	DestroyByCollisionPlayerWeapon dbcPWt = topSideBolt.GetComponent<DestroyByCollisionPlayerWeapon>() as DestroyByCollisionPlayerWeapon;
	dbcPWt.Damage = startingSideDamage;
	dbcPWt.MaxDamage = maxSideDamage;
	dbcPWt.StartingDamage = startingSideDamage;
	}
}
