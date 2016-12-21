﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour 
{
	public GameObject[] aestroids;
	public GameObject[] enemyDrones;
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
	public GUIText restartText;
	public GUIText gameOverText;

	private bool gameOver;
	private bool restart;

	private int score;

	void Start () {
		//scoreText = GetComponent <GUIText> ();
		restart = false;
		gameOver = false;
		restartText.text = "";
		gameOverText.text = "";
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

	void UpdateScore () {
		scoreText.text = "Score: " + score;
	}

	public void GameOver () {
		gameOverText.text = "GAME OVER";
		gameOver = true;
	}

	void LevelCompleated () {
		gameOverText.text = "YOU WIN!";
		restartText.text = "Press 'R' to restart";
		restart = true;
	}
}
