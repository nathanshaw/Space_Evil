using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour {

	public float hitPoints;
	public int scoreValue;

	public bool Hit (float damage) {
		hitPoints -= damage;
		if (damage <= 0) {
			return true;
		}
		return false;
	}

}
