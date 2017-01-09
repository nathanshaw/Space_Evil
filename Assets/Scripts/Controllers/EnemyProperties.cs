﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour {

	public float hitPoints;
	public int scoreValue;
	public float dropChance;
	public float healthDropChance; 

	public AudioSource deathAudioSource;
	public GameObject deathExplosion;

	public bool Hit (float damage) {
		hitPoints -= damage;
		// Debug.Log ("Enemy Hit : " + hitPoints);
		if (hitPoints <= 0) {
			float luck = Random.Range (0.0f, 1.0f);
			if (luck < dropChance) {
				GameController.Instance.dropRandomPowerUp (gameObject.transform.position);
			} else {
				luck = Random.Range (0.0f, 1.0f); 
				if (luck < healthDropChance) {
					GameController.Instance.dropHealth (gameObject.transform.position);
				} 
			}
			Debug.Log ("enemy properties calling death audio clip");
			deathAudioSource.Play ();
			ScoreManager.Instance.AddPoints (scoreValue);
			if (deathExplosion != null) {
				Instantiate (deathExplosion, transform.position, transform.rotation);
			}
			Destroy (gameObject);
			return true;
		}
		return false;
	}
}
