using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mover : MonoBehaviour {

	public float xSpeedMin;
	public float xSpeedMax;
	public float zSpeedMin;
	public float zSpeedMax;

	void Start () {
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		rigidbody.velocity = new Vector3(
			Random.Range(xSpeedMin, xSpeedMax),
			0.0f,
			Random.Range(zSpeedMin, zSpeedMax));
	}
}