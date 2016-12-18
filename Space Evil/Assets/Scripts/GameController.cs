using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public GameObject[] aestroids;
	public Vector3 spawnValues;
	public int hazardCount;
	public int waveCount;
	public float spawnWait;
	public float waveWait;
	public float startWait;
	public float waveRespawnMult;
	public float aestroidMinSize;
	public float aestroidMaxSize;

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
				SceneManager.LoadScene (Application.loadedLevel);
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
				yield return new WaitForSeconds (spawnWait);

				if (gameOver) {
					restart = true;
					restartText.text = "Press 'R' to restart";
					break;
				}
			}
			if (gameOver) {
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
