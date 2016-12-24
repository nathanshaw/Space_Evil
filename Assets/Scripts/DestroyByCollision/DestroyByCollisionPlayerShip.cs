 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollisionPlayerShip : MonoBehaviour {

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
		// Debug.Log ("Collision player ship : ", other.tag);
		if (other.CompareTag ("Power Up")) {
			Destroy (other.gameObject);
		}
	}
}