 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollisionEnemyAestroid : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public float damage;

	public int scoreValue;
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
		if (other.CompareTag ("Player")) {
			// if the hit does enough damage...
			Instantiate (explosion, transform.position, transform.rotation);
			gameController.PlayerHit (damage);
			Destroy (gameObject);
		} 
		// for some reason the bolt destroy by collision does not work
		/*
		else if (other.CompareTag ("Player Weapon")) {
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		*/
	}
}