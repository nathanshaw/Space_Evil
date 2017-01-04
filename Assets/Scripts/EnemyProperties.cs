using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour {

	public float hitPoints;
	public int scoreValue;
	private AudioSource deathAudioSource;

	public bool Hit (float damage) {
		hitPoints -= damage;
		if (damage <= 0) {
			return true;
			deathAudioSource.Play ();
		}
		return false;
	}

}
