﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

	public GameObject playerExplosion;
	//public float hitPoints;

	//public int scoreValue;
	private GameController gameController;
	private GameObject gameControllerObject;

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
		if (other.CompareTag ("Boundary")) {
			return;
		}

		if (other.CompareTag("Player")) {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
		}
		//gameController.AddScore (scoreValue);
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}