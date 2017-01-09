 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollisionEnemyBolt : MonoBehaviour {

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

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player") {
			gameController.PlayerHit (damage);
			// if the hit does enough damage...
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}