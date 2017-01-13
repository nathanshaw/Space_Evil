using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mover : MonoBehaviour {

	public bool randomSpeed;
	public float xSpeedMin;
	public float xSpeedMax;

	public float currentXSpeed;
	public float currentYSpeed;

	public float zSpeedMin;
	public float zSpeedMax;

	void Start () {
		Rigidbody rigidbody = GetComponent<Rigidbody> ();

		if (randomSpeed) {
			rigidbody.velocity = new Vector3 (
				Random.Range (xSpeedMin, xSpeedMax),
				0.0f,
				Random.Range (zSpeedMin, zSpeedMax));
			return;
		}
		// if not random speed then set speed to min speed
		rigidbody.velocity = new Vector3 (xSpeedMin, 0.0f, zSpeedMin);
	}
}