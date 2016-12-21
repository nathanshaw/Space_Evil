 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour {

	public int scoreValue;
	private GameController gameController;
	private GameObject gameControllerObject;
	//private GameObject player;
	//private GameObject playerObject;
	public PlayerController playerController;

	void Start () {
		gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
		/*
		player = GameObject.FindWithTag ("Player");
		if (player != null) {
			playerObject = player.GetComponent<GameObject> ();
		}
		if (playerObject == null) {
			Debug.Log("Cannot find 'player' object");
		}
		*/
	}

	// Use this for initialization
	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag ("Player") ) {
			playerController.speed += 10;
			Debug.Log ("Speed increased to : " + playerController.speed);
			gameController.AddScore (scoreValue);
			Destroy (gameObject);
		}
	}
}