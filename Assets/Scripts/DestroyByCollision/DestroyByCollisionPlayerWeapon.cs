 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollisionPlayerWeapon : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public float Damage;
	public float StartingDamage;
	public float MaxDamage;

	private GameController gameController;
	private GameObject gameControllerObject;
	private EnemyProperties enemyProperties;

	void Start () {
		gameControllerObject = GameObject.FindWithTag ("GameController");
		// set damage to starting damage at start
		Damage = StartingDamage;
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
			Destroy (gameObject);
			enemyProperties.Hit (Damage);
		}
	}

	public void updateDamage (float change) {
		Debug.Log ("Damage : " + Damage);
		Damage += change;
		Debug.Log ("Changed to : " + Damage);
		if (Damage < 1) {
			Damage = 1;
		} else if (Damage > MaxDamage) {
			Damage = MaxDamage;
		}
	}
}