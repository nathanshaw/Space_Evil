using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZigZag01 : MonoBehaviour {

	public float startingZ;

	void Start () {
		// give a starting Z coordinate

		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		/*
		rigidbody.velocity = new Vector3(
			Random.Range(xSpeedMin, xSpeedMax),
			0.0f,
			Random.Range(zSpeedMin, zSpeedMax));
			*/
	}
}