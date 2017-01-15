using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour {

	public float hitPoints;
	public int scoreValue;

	public float commonDropChance;
	public float uncommonDropChance;
	public float rareDropChance;

	public float healthDropChance; 

	public AudioSource deathAudioSource;
	public GameObject deathExplosion;

	public bool Hit (int sourcePlayerID, float damage) {
		hitPoints -= damage;
		if (hitPoints <= 0) {
			float luck = Random.Range (0.0f, 1.0f);
			if (luck < rareDropChance) {
				GameController.Instance.dropRarePowerUp (gameObject);
			}
			else if (luck < uncommonDropChance) {
				GameController.Instance.dropUncommonPowerUp (gameObject);
			}
			else if (luck < commonDropChance) {
				GameController.Instance.dropCommonPowerUp (gameObject);
			}
			else {
				luck = Random.Range (0.0f, 1.0f); 
				if (luck < healthDropChance) {
					GameController.Instance.dropHealth (gameObject.transform.position);
				} 
			}
			deathAudioSource.Play ();
			ScoreManager.Instance.AddPoints (sourcePlayerID, scoreValue);
			if (deathExplosion != null) {
				Instantiate (deathExplosion, transform.position, transform.rotation);
			}
			Destroy (gameObject);
			return true;
		}
		return false;
	}
}
