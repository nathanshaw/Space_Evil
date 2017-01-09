using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour 
{ 
	public static GameController Instance;
	// game hazards
	public GameObject[] aestroids;
	public GameObject[] enemyDrones;

	// power ups
	public GameObject[] commonPowerUps;
	public GameObject[] uncommonPowerUps;
	public GameObject[] rarePowerUps;
	public GameObject healthPowerUp;
	 
	// spawning
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

	// GUI Controller
	// public static GUIController GUIController; 
	// scoreManager 
	//public ScoreManager scoreManager;

	// Player object
	public GameObject player;
	public GameObject playerExplosion;

	// player states
	public float playerHitPoints;
	public float playerMaxHitPoints;
	public float playerBoltStartSpeed;
 	// keeping track of game state (is it paused?)
	private bool gamePaused;

	// state
	private bool gameOver;
	private bool restart; 

	public void Awake () {
		Instance = this;
	} 

	void Start () {
		//GUIController.Awake ();
		restart = false;
		gameOver = false;
		ScoreManager.Instance.NewGame ();
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
			if (Input.GetKeyDown (KeyCode.R) || Input.GetButton("Start")) {
				SceneManager.LoadScene ("level_1");
			}
		}
	}

	public void GameOver () {
		GUIController.Instance.SetLCText ("Game Over");
		gameOver = true;
	}

	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds (startWait);
		for (int w = 0; w < waveCount; w++) {
			// decrease the time inbetween aestroid spawns
			float powerUpPower = Random.Range(0.0f, 1.0f); 
			if (powerUpPower < 0.7) {
				Debug.Log ("spawning random power up");
				spawnRandomPowerUp ();
			} else if (powerUpPower < 0.9) {
				Debug.Log ("spawning uncommon power up");
				spawnUncommonPowerUp ();
			} else {
				Debug.Log ("spawning rare power up");
				spawnRarePowerUp ();
			}
			spawnWait *= waveRespawnMult;
			//ScoreManager.AddPoints (w * 1000);
			// increase the number of hazards by the wave number we are on
			hazardCount += w;
			GUIController.Instance.SetURText("Wave : " + w);
			yield return new WaitForSeconds (waveWait);
			for (int i = 0; i < hazardCount; i++) {
				spawnRandomAestroid ();
				if (waveCount >= firstEnemyWave) {
					enemyDroneCount++;
					spawnEnemyDrone ();
				}
				yield return new WaitForSeconds (spawnWait);
				if (gameOver) {
					restart = true;
					GUIController.Instance.SetCCText ("Press 'R' to restart");
					break;
				}
			}
			if (gameOver) {
					restart = true;
				GUIController.Instance.SetCCText("Press 'R' to restart");
					break;
			}
		}
		if (gameOver == false && restart == false) {
			LevelCompleated ();
		}
	}
	public GameObject spawnRandomPowerUp () {
		float chance = Random.Range (0.0f, 1.0f);
		GameObject returno;

		if (chance < 0.85) {
			returno = spawnCommonPowerUp ();
		}
		else if (chance < 0.98) {
			returno = spawnUncommonPowerUp ();
		}
		else {
			returno = spawnRarePowerUp ();
		}
		return returno;
	}

	public GameObject dropRandomPowerUp (Vector3 loc) {
		float chance = Random.Range (0.0f, 1.0f);
		GameObject returno;

		if (chance < 0.85) {
			returno = dropCommonPowerUp (loc);
		}
		else if (chance < 0.98) {
			returno = dropUncommonPowerUp (loc);
		}
		else {
			returno = dropRarePowerUp (loc);
		}
		return returno;
	}

	GameObject spawnCommonPowerUp () {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = commonPowerUps [Random.Range (0, commonPowerUps.Length)];
		Vector3 powerUpSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	GameObject spawnUncommonPowerUp () {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = uncommonPowerUps [Random.Range (0, uncommonPowerUps.Length)];
		Vector3 powerUpSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	GameObject spawnRarePowerUp () {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = rarePowerUps [Random.Range (0, rarePowerUps.Length)];
		Vector3 powerUpSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}
		
	public GameObject dropHealth (Vector3 powerUpSpawnPosition) {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerupClone = Instantiate (healthPowerUp, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	GameObject dropCommonPowerUp (Vector3 powerUpSpawnPosition) {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = commonPowerUps [Random.Range (0, commonPowerUps.Length)];
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	GameObject dropUncommonPowerUp (Vector3 powerUpSpawnPosition) {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = uncommonPowerUps [Random.Range (0, uncommonPowerUps.Length)];
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	GameObject dropRarePowerUp (Vector3 powerUpSpawnPosition) {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = rarePowerUps [Random.Range (0, rarePowerUps.Length)];
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
			GUIController.Instance.SetLLText("0");
			return 1;
		}
		return 0;
	}

	void updateHealthBar() {
		float normalizedScale = playerHitPoints / playerMaxHitPoints;
		GUIController.Instance.UpdateHealthBar (normalizedScale);
	}
		
	public void RequestPause() { 
		if (gamePaused == true) {
			gamePaused = false; 
			Time.timeScale = 1.0f;
			GUIController.Instance.SetLCText ("");
		}
		else {
			gamePaused = true;
			Time.timeScale = 0.0f;
			GUIController.Instance.SetLCText ("Game Paused");
		}
	}

	void LevelCompleated () { 
		GUIController.Instance.SetLCText ("YOU WIN!");
		GUIController.Instance.SetCCText ("Press 'R' to restart");
		restart = true;
	}

	public float PlayerSpeedChange (float speedChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.speed += speedChange;
		GUIController.Instance.SetLRText("Speed : " + pc.speed);
		return pc.speed;
	}

	public float PlayerSideSizeChange (float sideSizeChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.sideGunSize += sideSizeChange;
		GUIController.Instance.SetLRText("Side Size : " + pc.sideGunSize);
		return pc.speed;
	}

	public float PlayerSideDamageChange (float sideDamageChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.sideGunDamage += sideDamageChange;
		GUIController.Instance.SetLRText("Side Damage : " + pc.sideGunDamage);
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
		GUIController.Instance.SetLRText("Bolt Speed change : " + boltSpeedChange);
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
		GUIController.Instance.SetLRText("MultiShot : " + pc.activeBolts);
		return pc.activeBolts;
	}

	public float PlayerHealthChange (float healthChange) {
		// TODO move all player info to player controller
		playerHitPoints += healthChange;
		if (playerHitPoints > playerMaxHitPoints) {
			playerHitPoints = playerMaxHitPoints;
		}
		updateHealthBar ();
		GUIController.Instance.SetLRText("HP : " + playerHitPoints);
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
		GUIController.Instance.SetLRText("Max HP : " + playerMaxHitPoints);
		return playerHitPoints;
	}

	public float PlayerFireRateChange (float fireRateChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.shotsPerSecond += fireRateChange;
		GUIController.Instance.SetLRText("Fire Rate : " + pc.shotsPerSecond);
		return pc.shotsPerSecond;
	}

	public void PlayerBoltDamageChange (float boltDamageChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		float boltDamage = pc.ChangeBoltDamage(boltDamageChange);
		GUIController.Instance.SetLRText("Bolt Damage : " + boltDamage);
	}
}