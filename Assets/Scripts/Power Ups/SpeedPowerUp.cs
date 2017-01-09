 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour {

	public int scoreValue;
	private GameController gameController;
	private GameObject gameControllerObject;
	public float speedIncrease;

	void Start () {
		gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// Use this for initialization
	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag ("Player") ) {
			gameController.PlayerSpeedChange (speedIncrease);
			ScoreManager.Instance.AddPoints (scoreValue);
		}
	}
}