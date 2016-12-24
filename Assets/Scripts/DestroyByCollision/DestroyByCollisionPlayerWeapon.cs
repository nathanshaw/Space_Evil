 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollisionPlayerWeapon : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public float damage;

	private GameController gameController;
	private GameObject gameControllerObject;
	private EnemyProperties enemyProperties;

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
			Instantiate (explosion, transform.position, transform.rotation);
			// add the value of enemy to the total score
			enemyProperties = other.gameObject.GetComponent<EnemyProperties>();
			Debug.Log ("enemy properties : " + enemyProperties.scoreValue);
			gameController.AddScore(enemyProperties.scoreValue);
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}