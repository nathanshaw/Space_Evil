using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour 
{
	public GameObject[] aestroids;
	public GameObject[] enemyDrones;
	public GameObject[] commonPowerUps;

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

	// for keeping track of the score
	public GUIText scoreText;
	private int score;

	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText hitPointsText;

	// keeping track of notifications
	public GUIText notificationText;
	public float notificationWaitTime;

	// Player object
	public GameObject player;
	public GameObject playerExplosion;

	// player states
	public float playerHitPoints;

	// text
	private bool gameOver;
	private bool restart;

	void Start () {
		restart = false;
		gameOver = false;
		restartText.text = "";
		gameOverText.text = "";
		hitPointsText.text = "" + playerHitPoints;
		notificationText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	void Update ()
	{
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadScene ("level_1");
			}
		}
	}

	IEnumerator SpawnWaves() {
		Quaternion spawnRotation = Quaternion.identity;
		yield return new WaitForSeconds (startWait);
		for (int w = 0; w < waveCount; w++) {
			// decrease the time inbetween aestroid spawns
			spawnWait *= waveRespawnMult;

			// spawn a speed powerup inbetween each wave
			GameObject powerup = commonPowerUps [Random.Range (0, commonPowerUps.Length)];
			Vector3 powerUpSpawnPosition = new Vector3 (
				spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z)
			);
			GameObject powerupClone = Instantiate (powerup, powerUpSpawnPosition, spawnRotation) as GameObject;

			// increase the number of hazards by the wave number we are on
			hazardCount += w;
			yield return new WaitForSeconds (waveWait);
			for (int i = 0; i < hazardCount; i++) {
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
				if (waveCount >= firstEnemyWave) {
					enemyDroneCount++;
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
				}

				yield return new WaitForSeconds (spawnWait);
			}
				if (gameOver) {
					restart = true;
					restartText.text = "Press 'R' to restart";
					break;
				}
			}
		if (gameOver == false && restart == false) {
			LevelCompleated ();
		}
	}

	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	public int PlayerHit(float damage) {
		playerHitPoints -= damage;
		Debug.Log ("Player Took Damage, Current Hit Points : " + playerHitPoints);
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
		Quad healthBar = gameObject.GetComponent (typeof(Quad)) as Quad;
		if (Quad == null) {
			Debug.Log ("Was unable to find the healthbar Quad");
		}
		return 0;
	}

	void UpdateScore () {
		scoreText.text = "Score: " + score;
	}

	public void GameOver () {
		gameOverText.text = "GAME OVER";
		gameOver = true;
	}

	public IEnumerator Notification (string text) {
		notificationText.text = text;
		yield return new WaitForSeconds (notificationWaitTime);
		notificationText.text = "";
		// wait 4 seconds then remove text
	}

	void LevelCompleated () {
		gameOverText.text = "YOU WIN!";
		restartText.text = "Press 'R' to restart";
		restart = true;
	}

	public float PlayerSpeedChange (float newSpeed) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.speed += newSpeed;
		StartCoroutine (Notification ("Speed : " + pc.speed));
		return pc.speed;
	}

	public float PlayerFireRateChange (float fireRateChange) {
		PlayerController pc = player.GetComponent (typeof(PlayerController)) as PlayerController;
		if (pc == null) {
			Debug.Log ("cannt find player controller from Game controller");
		}
		pc.shotsPerSecond += fireRateChange;
		StartCoroutine (Notification ("Fire Rate : " + pc.shotsPerSecond));
		return pc.shotsPerSecond;
	}
}
