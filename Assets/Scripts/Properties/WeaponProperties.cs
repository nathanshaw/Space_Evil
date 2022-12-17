using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProperties : MonoBehaviour {
	 

	public float xSpeed;
	public float ySpeed;

	public GameObject Explosion;

	public int sourcePlayerID;

	public float minDamage;
	public float maxDamage;
	public float damage;

	public float UpdateDamage ( float damDelta ) {
		damage += damDelta;
		if (damage < minDamage) {
			damage = minDamage;
		} else if (damage > maxDamage) {
			damage = maxDamage;
		}
		return damage;
	}

}
