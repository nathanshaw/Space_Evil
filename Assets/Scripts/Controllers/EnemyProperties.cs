using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour {

	public float hitPoints;
	public int scoreValue;
	public float dropChance;
	public AudioSource deathAudioSource;

	public bool Hit (float damage) {
		
		hitPoints -= damage;
		// Debug.Log ("Enemy Hit : " + hitPoints);
		if (hitPoints <= 0) {
			GameController gc = GetComponent (typeof(GameController)) as GameController;
			float luck = Random.RandomRange (0.0f, 1.0f);
			// Debug.Log ("luck : " + luck + " dc: " + dropChance);
			if (luck < dropChance) {
				GameController.Instance.dropRandomPowerUp (gameObject.transform.position);
				Debug.Log ("Dropping PU from enemy kill");
			}
			deathAudioSource.Play ();
			ScoreManager.Instance.AddPoints (scoreValue);
			Destroy (gameObject);
			return true;
		}
		return false;
	}
}
