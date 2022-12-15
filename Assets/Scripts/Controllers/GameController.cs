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
	public GameObject[] bosses; 

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
	public int bossFrequency;

	// Player object
	public PlayerController[] playerControllers;
	// 0 = player is dead 
	// 1 = player is alive and well
	// 2 = plater is invulnerable TODO
	private int[] playerStates = new int[] {0,0};
	public GameObject playerExplosion;

 	// keeping track of game state (is it paused?)
	private bool gamePaused;
	// has the player lost?
	private bool gameOver;
	private bool restart; 

	public void Awake () {
		Instance = this;
	} 

	void Start () {
		startWait = 5;
		restart = false;
		gameOver = false;
		// TODO - change this depending on if the user selects
		playerStates[0] = 1;
		playerStates[1] = 1;
		// keep track of the player's score
		ScoreManager.Instance.NewGame ();
		// begin the level by spawning waves...
		StartCoroutine (SpawnWaves());

		foreach (PlayerController player in playerControllers) {
			
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
			pc.ResetWeapons ();
			pc.UpdateHealthBar ();
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

	public void PlayerKilled (int whatPlayer) {
		playerStates [whatPlayer] = 0; 
		foreach (int state in playerStates) {
			// if there is still a player alive exit method
			if (state > 0) {
				return;
			}
		} 
		GameOver ();
	}

	public void GameOver () {
		GUIController.Instance.SetCCText ("Game Over");
		gameOver = true;
	}

	IEnumerator SpawnWaves() {
		GUIController.Instance.SetCCText("Get Ready!");
		yield return new WaitForSeconds (startWait);
		for (int w = 1; w < waveCount; w++) {
            GUIController.Instance.SetLCText("Wave #" + (char)w);
			// this needs to change to w instead of wavecount TODO
			if (w % bossFrequency == 0 && w != 0 && bossFrequency > 0) {
				spawnBoss ((w - bossFrequency) / bossFrequency);
			} else {
				Debug.Log ("Not spawning boss, mod was : " + (w % bossFrequency));
			}
			// decrease the time inbetween aestroid spawns
			float powerUpPower = Random.Range(0.0f, 1.0f); 
			if (powerUpPower < 0.7) {
				spawnRandomPowerUp ();
			} else if (powerUpPower < 0.9) {
				spawnUncommonPowerUp ();
			} else {
				spawnRarePowerUp ();
			}
			spawnWait *= waveRespawnMult;

			// increase the number of hazards by the wave number we are on
			hazardCount += (w *3);
			GUIController.Instance.SetCCText ("Wave : " + (char)w);
			yield return new WaitForSeconds (waveWait);
			for (int i = 0; i < hazardCount; i++) {
				spawnRandomAestroid ();
				if (w >= firstEnemyWave) {
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
	// The spawn functions are for powerups that are not dropped
	public GameObject spawnCommonPowerUp () {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = commonPowerUps [Random.Range (0, commonPowerUps.Length)];
		Vector3 powerUpSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	public GameObject spawnUncommonPowerUp () {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = uncommonPowerUps [Random.Range (0, uncommonPowerUps.Length)];
		Vector3 powerUpSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	public GameObject spawnRarePowerUp () {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = rarePowerUps [Random.Range (0, rarePowerUps.Length)];
		Vector3 powerUpSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}
		
	public GameObject dropCommonPowerUp (GameObject dropper) {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = commonPowerUps [Random.Range (0, commonPowerUps.Length)];
		Vector3 powerUpSpawnPosition = dropper.transform.position;
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	public GameObject dropUncommonPowerUp (GameObject dropper) {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = uncommonPowerUps [Random.Range (0, uncommonPowerUps.Length)];
		Vector3 powerUpSpawnPosition = dropper.transform.position;
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	public GameObject dropRarePowerUp (GameObject dropper) {
		Quaternion spawnRotation = Quaternion.identity;
		GameObject powerup = rarePowerUps [Random.Range (0, rarePowerUps.Length)];
		Vector3 powerUpSpawnPosition = dropper.transform.position;
		GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;
		return powerupClone;
	}

	public GameObject dropHealth (Vector3 powerUpSpawnPosition) {
		foreach (PlayerController pc in playerControllers) {
			if (pc.GetHealthPercent () < 100.0) {
				Quaternion spawnRotation = Quaternion.identity;
				GameObject powerupClone = Instantiate (healthPowerUp, powerUpSpawnPosition, spawnRotation) as GameObject;
				return powerupClone;
			}
		}
			return null;
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

	GameObject spawnBoss (int bossNum) { 
		Quaternion spawnRotation = Quaternion.identity;
		GameObject boss;
		if (bossNum < bosses.Length) {
			boss = bosses [bossNum];
			Debug.Log ("Spawning boss " + bossNum);
		} else {
			boss = bosses [bosses.Length - 1];
			Debug.Log ("Spawning boss " + (bosses.Length - 1));
		}
		//Quaternion bossSpawnRotation = Quaternion.AngleAxis(270, Vector3.up);
		Quaternion bossSpawnRotation = Quaternion.identity;
		Vector3 bossSpawnPosition = new Vector3 (
			spawnValues.x, spawnValues.y, 
			Random.Range (-spawnValues.z, spawnValues.z)
		);
		GameObject bossClone = Instantiate (boss, 
			bossSpawnPosition, bossSpawnRotation) as GameObject;
		//Vector3 targetAngle = bossClone.transform.eulerAngles + Vector3.up;
		bossClone.transform.eulerAngles = new Vector3(
			0.0f, 90.0f, 0.0f);
		return bossClone;
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
		//Vector3 targetAngle = droneClone.transform.eulerAngles + Vector3.up;
		droneClone.transform.eulerAngles = new Vector3(
			0.0f, 270.0f, 0.0f);
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
		
	public void RequestPause() { 
		if (gamePaused == true) {
			gamePaused = false; 
			Time.timeScale = 1.0f;
			GUIController.Instance.SetLCText ("");
		}else {
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

}