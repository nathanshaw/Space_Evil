using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_Mover_01 : MonoBehaviour {

	public float xSpeedMin;
	public float xSpeedMax;
	public float zSpeed;
	public float xThresh;
	public float zThresh;
	private Rigidbody myBody;

	void Start () {
		myBody = GetComponent<Rigidbody> ();
		myBody.velocity = new Vector3(
			Random.Range(xSpeedMin, xSpeedMax),
			0.0f,
			0.0f);
	}

	void Update () {

		if (transform.position.z < -zThresh) {
			myBody.velocity = new Vector3(
				0.0f,
				0.0f,
				zSpeed);
		}
		else if (transform.position.z > zThresh) {
			myBody.velocity = new Vector3(
				0.0f,
				0.0f,
				-zSpeed);
		}
		else if (transform.position.x < xThresh ) {
			Debug.Log ("Stopping boss advance");
			myBody.velocity = new Vector3(
				0.0f,
				0.0f,
				zSpeed);
			myBody.position = new Vector3(xThresh, myBody.position.y, myBody.position.z);
		}
	}
}
