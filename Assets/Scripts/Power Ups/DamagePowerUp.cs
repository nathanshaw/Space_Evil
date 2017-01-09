 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerUp : MonoBehaviour {

	public int scoreValue;
	private GameController gameController;
	private GameObject gameControllerObject;
	public float boltDamageChange;

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
			gameController.PlayerBoltDamageChange (boltDamageChange);
			ScoreManager.Instance.AddPoints (scoreValue);
		}
	}
}