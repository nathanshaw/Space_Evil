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

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "PlayerOne" || other.tag == "PlayerTwo") {
			Instantiate (explosion, transform.position, transform.rotation);
			gameController.PlayerHit (damage);
			Destroy (gameObject);
		} 
	}
}