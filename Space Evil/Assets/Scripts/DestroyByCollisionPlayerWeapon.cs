 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollisionPlayerWeapon : MonoBehaviour {

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
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// Use this for initialization
	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag ("Active Enemy") ||
		    other.CompareTag ("Passive Enemy")) {
			// if the hit does enough damage...
			Instantiate (explosion, transform.position, transform.rotation);
			gameController.AddScore (scoreValue);
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}