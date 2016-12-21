﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollisionEnemyDrone : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public float hitPoints;

	public int scoreValue;
	private GameController gameController;
	private GameObject gameControllerObject;

	void Start () {
		gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
			Debug.Log ("Found gameControllerObject");
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// Use this for initialization
	void OnTriggerEnter(Collider other) 
	{
		Debug.Log (other.tag);
		if (other.tag == "Boundary") {
			return;
		}
		if (other.tag == "Passive Enemy") {
			return;
		}
		if (other.tag == "Active Enemy") {
			return;
		}
		// if the hit does enough damage...
		Instantiate (explosion, transform.position, transform.rotation);
		if (other.tag == "Player") {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
		}
		gameController.AddScore (scoreValue);
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}