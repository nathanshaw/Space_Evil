using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour 
{
	public GameObject[] aestroids;
	public GameObject[] enemyDrones;
	public GameObject[] commonPowerUps;
	public GameObject healthBar;

	public Vector3 spawnValues;
	public int hazardCount;
	public int waveCount;
	public float spawnWait;
	public float waveWait;
	public float startWait;
	public float waveRespawnMult;
	public float aestroidMinSize;
	public float aestroidMaxSize;
	public int firstEnemyWave;
	public int enemyDroneCount;

	/// ------------
	/// GUI RELATED
	/// ------------
	// TODO turn this into upper left
	public GUIText scoreText;

	public GUIText smallCenterText;
	public GUIText largeCenterText;
	// turn this into lower left notification
	public GUIText hitPointsText;

	// keeping track of notifications
	// upper right 
	public GUIText urNotificationText;
	public float urNotificationWaitTime;
	private bool urNotificationBeingDisplayed;
	// lower right
	public GUIText lrNotificationText;
	public float lrNotificationWaitTime;
	private bool lrNotificationBeingDisplayed;

	// Player object
	public GameObject player;
	public GameObject playerExplosion;

	// player states
	public float playerHitPoints;
	public float playerMaxHitPoints;
	public float playerBoltStartSpeed;
	private int score;
	// keeping track of game state (is it paused?)
	private bool gamePaused;

	// text
	private bool gameOver;
	private bool restart;


	void Start () {
		restart = false;
		gameOver = false;
		smallCenterText.text = "";
		largeCenterText.text = "";
		hitPointsText.text = "" + playerHitPoints;
		lrNotificationText.text = "";
		urNotificationText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
		updateHealthBar ();

		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		for (int i = 0; i < pc.bolts.Length; i++) {
			Mover boltMover = pc.bolts [i].GetComponent<Mover> ();
			boltMover.xSpeedMax = playerBoltStartSpeed;
			boltMover.xSpeedMin = playerBoltStartSpeed;
		}
	}

	void Update ()
	{
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R) || Input.GetKey("Start")) {
				SceneManager.LoadScene ("level_1");
			}
		}
	}

	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds (startWait);
		for (int w = 0; w < waveCount; w++) {
			// decrease the time inbetween aestroid spawns
			spawnPowerUp ();
			spawnWait *= waveRespawnMult;
			AddScore (w * 1000);
			// increase the number of hazards by the wave number we are on
			hazardCount += w;
			StartCoroutine (UpperRightNotification ("Wave : " + w));
			yield return new WaitForSeconds (waveWait);
			for (int i = 0; i < hazardCount; i++) {
				spawnRandomAestroid ();
				if (waveCount >= firstEnemyWave) {
					enemyDroneCount++;
					spawnEnemyDrone ();
				}
				yield return new WaitForSeconds (spawnWait);
			}
			if (gameOver) {
					restart = true;
					smallCenterText.text = "Press 'R' to restart";
					break;
			}
		}
		if (gameOver == false && restart == false) {
			LevelCompleated ();
		}
	}

	GameObject spawnPowerUp () {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = commonPowerUps [Random.Range (0, commonPowerUps.Length)];
		Vector3 powerUpSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	GameObject spawnEnemyDrone () { 
		Quaternion spawnRotation = Quaternion.identity;
		GameObject enemyDrone = enemyDrones [Random.Range (0, enemyDrones.Length)];
		Quaternion droneSpawnRotation = Quaternion.AngleAxis(270, Vector3.up);
		Vector3 droneSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, 
			Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject droneClone = Instantiate (enemyDrone, 
			droneSpawnPosition, droneSpawnRotation) as GameObject;
		// hack to get them facing the right way
		droneClone.transform.RotateAround(droneClone.transform.position, droneClone.transform.up, Time.deltaTime * 180.0f);
		return droneClone;
	}

	GameObject spawnRandomAestroid () {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject aestroid = aestroids [Random.Range (0, aestroids.Length)];
		Vector3 spawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject aestroidClone = Instantiate (aestroid, spawnPosition, spawnRotation) as GameObject;
		// change scale to be random
		Vector3 scale = new Vector3 (
			Random.Range (aestroidMinSize, aestroidMaxSize),
			Random.Range (aestroidMinSize, aestroidMaxSize),
			Random.Range (aestroidMinSize, aestroidMaxSize)
		);
		aestroidClone.transform.localScale = scale;
		return aestroidClone;
	}

	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	public int PlayerHit(float damage) {
		playerHitPoints -= damage;
		updateHealthBar ();
		// Debug.Log ("Player Took Damage, Current Hit Points : " + playerHitPoints);
		if (playerHitPoints < 0 || playerHitPoints == 0) {
			GameOver ();
			Destroy (player);
			Instantiate (playerExplosion, 
						 player.transform.position, 
						 player.transform.rotation);
			hitPointsText.text = "0";
			return 1;
		}
		hitPointsText.text = "" + damage;
		return 0;
	}

	void updateHealthBar() {
		float normalizedScale = playerHitPoints / playerMaxHitPoints;
		healthBar.transform.localScale = new Vector3(
			normalizedScale * 10, 
			healthBar.transform.localScale.y, 
			healthBar.transform.localScale.z
		);

		// set the healthbar color
		if (normalizedScale > 0.5) {
			healthBar.GetComponent<Renderer> ().material.color = Color.green;
		} 
		else if (normalizedScale > 0.25f) {
			healthBar.GetComponent<Renderer> ().material.color = Color.yellow;
		} 
		else {
			healthBar.GetComponent<Renderer> ().material.color = Color.red;
		}
	}

	void UpdateScore () {
		scoreText.text = "Score: " + score;
	}

	public void GameOver () {
		largeCenterText.text = "GAME OVER";
		gameOver = true;
	}

	public void RequestPause() { 
		if (gamePaused == true) {
			gamePaused = false; 
			Time.timeScale = 1.0f;
			largeCenterText.text = "";
		}
		else {
			gamePaused = true;
			Time.timeScale = 0.0f;
			largeCenterText.text = "Game Paused";
		}
	}

	public IEnumerator LowerRightNotification (string text) {
		if (lrNotificationBeingDisplayed == false) {
			lrNotificationBeingDisplayed = true;
			lrNotificationText.text = text;
			yield return new WaitForSeconds (lrNotificationWaitTime);
			lrNotificationText.text = "";
			lrNotificationBeingDisplayed = false;
			// wait 4 seconds then remove text
		} else {
			while (lrNotificationBeingDisplayed == true) {
				yield return new WaitForSeconds (0.3f);
			}
			lrNotificationBeingDisplayed = true;
			lrNotificationText.text = text;
			yield return new WaitForSeconds (lrNotificationWaitTime * 0.6f);
			lrNotificationText.text = "";
			lrNotificationBeingDisplayed = false;
		}
	}

	public IEnumerator UpperRightNotification (string text) {
		if (urNotificationBeingDisplayed == false) {
			urNotificationBeingDisplayed = true;
			urNotificationText.text = text;
			yield return new WaitForSeconds (urNotificationWaitTime);
			urNotificationText.text = "";
			urNotificationBeingDisplayed = false;
			// wait 4 seconds then remove text
		} else {
			while (urNotificationBeingDisplayed == true) {
				yield return new WaitForSeconds (0.3f);
			}
			urNotificationBeingDisplayed = true;
			urNotificationText.text = text;
			yield return new WaitForSeconds (urNotificationWaitTime * 0.6f);
			urNotificationText.text = "";
			urNotificationBeingDisplayed = false;
		}
	}

	void LevelCompleated () {
		largeCenterText.text = "YOU WIN!";
		smallCenterText.text = "Press 'R' to restart";
		restart = true;
	}

	public float PlayerSpeedChange (float speedChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.speed += speedChange;
		StartCoroutine (LowerRightNotification ("Speed : " + pc.speed));
		return pc.speed;
	}

	public float PlayerSideSizeChange (float sideSizeChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.sideGunSize += sideSizeChange;
		StartCoroutine (LowerRightNotification ("Side Size : " + pc.sideGunSize));
		return pc.speed;
	}

	public float PlayerBoltSpeedChange (float boltSpeedChange) {
		// get all the bolt objects from player controller 
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		for (int i = 0; i < pc.bolts.Length; i++) {
			Mover boltMover = pc.bolts [i].GetComponent<Mover> ();
			boltMover.xSpeedMax += boltSpeedChange;
			boltMover.xSpeedMin += boltSpeedChange;
			Debug.Log("boltMover xspeed changed to : " + boltMover.xSpeedMax);
		}
		StartCoroutine (LowerRightNotification ("Bolt Speed change : " + boltSpeedChange));
		return boltSpeedChange;
	}

	public float PlayerMultiShotChange (int multiShotChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.activeBolts += multiShotChange;
		if (pc.activeBolts < 0) {
			pc.activeBolts = 0;
		} else if (pc.activeBolts > pc.bolts.Length) {
			pc.activeBolts = pc.bolts.Length;
		}
		StartCoroutine (LowerRightNotification ("MultiShot : " + pc.activeBolts));
		return pc.activeBolts;
	}

	public float PlayerHealthChange (float healthChange) {
		// TODO move all player info to player controller
		playerHitPoints += healthChange;
		if (playerHitPoints > playerMaxHitPoints) {
			playerHitPoints = playerMaxHitPoints;
		}
		updateHealthBar ();
		StartCoroutine (LowerRightNotification ("HP : " + playerHitPoints));
		return playerHitPoints;
	}

	public float PlayerMaxHealthChange (float maxHealthChange) {
		// TODO move all player info to player controller
		playerMaxHitPoints += maxHealthChange;
		playerHitPoints += maxHealthChange;
		if (playerMaxHitPoints < 50) {
			playerMaxHitPoints = 50;
		}
		updateHealthBar ();
		StartCoroutine (LowerRightNotification ("Max HP : " + playerMaxHitPoints));
		return playerHitPoints;
	}

	public float PlayerFireRateChange (float fireRateChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.shotsPerSecond += fireRateChange;
		StartCoroutine (LowerRightNotification ("Fire Rate : " + pc.shotsPerSecond));
		return pc.shotsPerSecond;
	}

	public void PlayerBoltDamageChange (float boltDamageChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		float boltDamage = pc.ChangeBoltDamage(boltDamageChange);
		StartCoroutine (LowerRightNotification ("Bolt Damage : " + boltDamage));
	}
}