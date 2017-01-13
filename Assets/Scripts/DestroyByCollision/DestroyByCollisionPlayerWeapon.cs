 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollisionPlayerWeapon : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public WeaponProperties weaponProperties;


	private GameController gameController;
	private GameObject gameControllerObject;
	private EnemyProperties enemyProperties;
	private int sourcePlayerID;

	void Start () {
		gameControllerObject = GameObject.FindWithTag ("GameController");
		// set damage to starting damage at start
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
		sourcePlayerID = weaponProperties.sourcePlayerID;
	}

	// Use this for initialization
	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag ("Active Enemy") ||
		    other.CompareTag ("Passive Enemy")) {
			Instantiate (explosion, transform.position, transform.rotation);
			// add the value of enemy to the total score
			enemyProperties = other.gameObject.GetComponent<EnemyProperties>();
			Destroy (gameObject);
			enemyProperties.Hit (sourcePlayerID, weaponProperties.damage);
		}
	}
}