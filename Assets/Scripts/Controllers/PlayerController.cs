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
	public float movementSpeed;
	public float maxMovementSpeed;
	public float vTilt;
	public float hTilt;
	public GameObject[] bolts;
	public GameObject topSideBolt;
	public GameObject bottomSideBolt;
	public GameObject playerDeathExplosion;
	public GameController gc;
	public Transform bottomSideSpawn;
	public Transform topSideSpawn;

	public Transform[] boltSpawns;

	// Player Attributes 
	public int playerID;// index 0
	public float shotsPerSecond;
	private float timeBetweenBolts;
	private float manualTimeBetweenBolts;
	public float maxHitPoints;
	public float currentHitPoints;

	private float healthPercent;

	// bolt gun
	public float boltDamage;
	public float startingBoltDamage;
	public float maxBoltDamage;
	public int activeBolts;
	public float boltStartSpeed;
	public float boltMinSpeed;
	public float boltMaxSpeed;

	// side gun
	public float sideGunSize;
	public float sideGunDamage;
	public float startingSideDamage;
	public float maxSideDamage;
	public float sideMaxSpeed;
	public float sideMinSpeed;
	public float sideSpeed;

	// to keep track of fireing
	private float nextAutoFire;
	private float nextManualFire;
	private bool fireButtonPressed;
	private bool startButtonPressed;

	public AudioSource weaponFireAudioSource;
	private string playerPrefix;
	// private string startString;
	private string fire1String;
	private string verticalString;
	private string horizontalString;

	void Start ()
	{
		ResetBoltDamage ();
		playerPrefix = "P" + (playerID + 1) + " ";
		Debug.Log ("player prefix : " + playerPrefix);
		verticalString = playerPrefix + "Vertical";
		horizontalString = playerPrefix + "Horizontal";
		fire1String = playerPrefix + "Fire1";
	}

	//code that is run for every frame
	void Update ()
	{
		// see if the game has been paused
		if (Input.GetButton ("Start")) {
			if (startButtonPressed == false) {
				startButtonPressed = true;
				gc.RequestPause ();
			}
		} else if (startButtonPressed) {
			startButtonPressed = false;
		}
		// see if the player is attempting to shoot a weapon
		if (Input.GetButton (fire1String) && Time.time > nextAutoFire) {
			fireButtonPressed = true;
			timeBetweenBolts = 1 / shotsPerSecond;
			// bolts
			nextAutoFire = Time.time + timeBetweenBolts;
			for (int i = 0; i < activeBolts; i++) {
				Instantiate (bolts [i], boltSpawns [i].position, boltSpawns [i].rotation);
			}
			weaponFireAudioSource.Play ();
			// side gun
			if (sideGunSize > 0) {
				GameObject topBolt = Instantiate (topSideBolt, topSideSpawn.position, topSideSpawn.rotation) as GameObject;  
				GameObject bottomBolt = Instantiate (bottomSideBolt, bottomSideSpawn.position, bottomSideSpawn.rotation) as GameObject;
				// TODO, make it so the side bolts can have their sizes changed...
				Vector3 scale = new Vector3 (1.0f, 1.0f, sideGunSize);
				topBolt.transform.localScale = scale;
				bottomBolt.transform.localScale = scale;
			}
		} else if (Input.GetButton (fire1String) == false) {
			fireButtonPressed = false;
		} else if (fireButtonPressed == false &&
			Input.GetButton (fire1String) &&
		           Time.time > nextManualFire) {
			fireButtonPressed = true;
			timeBetweenBolts = 1 / shotsPerSecond;
			manualTimeBetweenBolts = 1 / (shotsPerSecond * 2);
			nextAutoFire = Time.time + timeBetweenBolts;
			nextManualFire = Time.time + manualTimeBetweenBolts;
			for (int i = 0; i < activeBolts; i++) {
				Instantiate (bolts [i], boltSpawns [i].position, boltSpawns [i].rotation);
			}
			weaponFireAudioSource.Play ();
			// side gun
			if (sideGunSize > 0) {
				GameObject topBolt = Instantiate (topSideBolt, topSideSpawn.position, topSideSpawn.rotation) as GameObject;  
				GameObject bottomBolt = Instantiate (bottomSideBolt, bottomSideSpawn.position, bottomSideSpawn.rotation) as GameObject;
				// TODO, make it so the side bolts can have their sizes changed...
				Vector3 scale = new Vector3 (1.0f, 1.0f, sideGunSize);
				topBolt.transform.localScale = scale;
				bottomBolt.transform.localScale = scale;
			}
		} else if (Input.GetButton (fire1String)) {
			fireButtonPressed = true;
		}
	}

	// executed once per physics step
	void FixedUpdate ()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		float moveHorizontal = Input.GetAxis (horizontalString);
		float moveVertical = Input.GetAxis (verticalString);
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical); 
		rigidbody.velocity = movement * movementSpeed;
		rigidbody.position = new Vector3 (
			Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
		rigidbody.rotation = Quaternion.Euler (rigidbody.velocity.x * hTilt, 90, rigidbody.velocity.z * vTilt);
	}

	public float ChangeBoltDamage (float damChange)
	{
		// get all the bolts and update their damage;
		// returns the damage of the first bolt
		float updatedDamage = 0.0f;
		for (int i = 0; i < bolts.Length; i++) {
			WeaponProperties wp = bolts [i].GetComponent<WeaponProperties> () as WeaponProperties;
			wp.UpdateDamage (damChange);
			if (i == 0) {
				updatedDamage = wp.damage;
			}
		}
		return updatedDamage;
	}

	public float ChangeSideDamage (float damChange)
	{
		// get all the bolts and update their damage;
		// returns the damage of the first bolt
		WeaponProperties bwp = bottomSideBolt.GetComponent<WeaponProperties> () as WeaponProperties;
		bwp.UpdateDamage (damChange);
		WeaponProperties twp = topSideBolt.GetComponent<WeaponProperties> () as WeaponProperties;
		twp.UpdateDamage (damChange);
		return twp.damage;
	}

	private void ResetBoltDamage ()
	{
		for (int i = 0; i < bolts.Length; i++) {
			WeaponProperties wp = bolts [i].GetComponent<WeaponProperties> () as WeaponProperties;
			wp.minDamage = startingBoltDamage;
			wp.maxDamage = maxBoltDamage;
			wp.damage = startingBoltDamage;
		}
	}

	private void ResetBoltSpeed ()
	{
		for (int i = 0; i < bolts.Length; i++) {
			WeaponProperties wp = bolts [i].GetComponent<WeaponProperties> () as WeaponProperties;
			wp.xSpeed = startingBoltDamage;
			wp.ySpeed = startingBoltDamage;
		}
	}

	private void ResetSideDamage ()
	{
		// get dbcpsw for top and bottom and reset their values
		WeaponProperties twp = topSideBolt.GetComponent<WeaponProperties> () as WeaponProperties;
		twp.damage = startingSideDamage;
		twp.maxDamage = maxSideDamage;
		twp.minDamage = startingSideDamage;
		WeaponProperties bwp = bottomSideBolt.GetComponent<WeaponProperties> () as WeaponProperties;
		bwp.damage = startingSideDamage;
		bwp.maxDamage = maxSideDamage;
		bwp.minDamage = startingSideDamage;	}

	public void ResetWeapons ()
	{
		ResetBoltDamage ();
		ResetSideDamage ();
		ResetBoltSpeed ();
	}

	private void SendGUIMessage (string message) {
		if (playerID == 0) {
			GUIController.Instance.SetURText(message);
		}
		else if (playerID == 1){
			GUIController.Instance.SetLRText(message);
			}
	}

	public int PlayerHit(float damage) {
		currentHitPoints -= damage;
		UpdateHealthBar ();
		// Debug.Log ("Player Took Damage, Current Hit Points : " + playerHitPoints);
		if (currentHitPoints <= 0) {
			currentHitPoints = 0;
			gc.PlayerKilled (playerID);
			Destroy (gameObject);
			Instantiate (playerDeathExplosion, 
				transform.position, 
				transform.rotation);
			UpdateHealthBar ();
			return 1;
		}
		return 0;
	}
	public float SpeedChange (float speedChange) {
		Debug.Log ("speed : " + movementSpeed); 
		movementSpeed += speedChange;
		if (movementSpeed > maxMovementSpeed) {
			movementSpeed = maxMovementSpeed;
		}
		Debug.Log ("speed : " + movementSpeed);
		SendGUIMessage("Speed : " + movementSpeed);
		return movementSpeed;
	}

	public float SideSizeChange (float sideSizeChange) {

		sideGunSize += sideSizeChange;
		SendGUIMessage("Side Size : " + sideGunSize);
		return sideGunSize;
	}

	public float SideDamageChange (float sideDamageChange) {

		sideGunDamage += sideDamageChange;
		SendGUIMessage("Side Damage : " + sideGunDamage);
		return sideGunDamage;
	}

	public float BoltSpeedChange (float boltSpeedChange) {
		// get all the bolt objects from player controller 
		for (int i = 0; i < bolts.Length; i++) {
			Mover boltMover = bolts [i].GetComponent<Mover> ();
			boltMover.xSpeedMax += boltSpeedChange;
			boltMover.xSpeedMin += boltSpeedChange;
			Debug.Log("boltMover xspeed changed to : " + boltMover.xSpeedMax);
		}
		SendGUIMessage("Bolt Speed change : " + boltSpeedChange);
		return boltSpeedChange;
	}

	public float MultiShotChange (int multiShotChange) {
		activeBolts += multiShotChange;
		if (activeBolts < 0) {
			activeBolts = 0;
		} else if (activeBolts > bolts.Length) {
			activeBolts = bolts.Length;
		}
		SendGUIMessage("MultiShot : " + activeBolts);
		return activeBolts;
	}

	public float HealthChange (float healthChange) {
		currentHitPoints += healthChange;
		if (currentHitPoints > maxHitPoints) {
			currentHitPoints = maxHitPoints;
		}
		UpdateHealthBar ();
		SendGUIMessage("HP : " + currentHitPoints);
		return currentHitPoints;
	}

	public float MaxHealthChange (float maxHealthChange) {
		maxHitPoints += maxHealthChange;
		currentHitPoints += maxHealthChange;
		if (maxHitPoints < 50) {
			maxHitPoints = 50;
		}
		UpdateHealthBar ();
		SendGUIMessage("Max HP : " + maxHitPoints);
		return currentHitPoints;
	}

	public float FireRateChange (float fireRateChange) {
		shotsPerSecond += fireRateChange;
		SendGUIMessage("Fire Rate : " + shotsPerSecond);
		return shotsPerSecond;
	}

	public float BoltDamageChange (float boltDamageChange) {
		float boltDamage = ChangeBoltDamage(boltDamageChange);
		SendGUIMessage("Bolt Damage : " + boltDamage);
		return boltDamage;
	}

	public void UpdateHealthBar() {
		float normalizedScale = currentHitPoints / maxHitPoints;
		if (playerID == 0) {
			GUIController.Instance.UpdateUpperHealthBar (normalizedScale);
			return;
		}
		healthPercent = normalizedScale * 100;
		GUIController.Instance.UpdateLowerHealthBar (normalizedScale);
		}

	public float GetHealthPercent () {
		return healthPercent;
	}
}