using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZigZagMover01 : MonoBehaviour {

	public float startingZ;
	public float betweenTime;
	public float startWait;

	public float xSpeedMin;
	public float xSpeedMax;
	public float zSpeedMin;
	public float zSpeedMax;

	void Start () {
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		rigidbody.velocity = new Vector3(
			Random.Range(xSpeedMin, xSpeedMax),
			0.0f,
			Random.Range(zSpeedMin, zSpeedMax)
		);
		StartCoroutine (Move());
	}

	IEnumerator Move ()
	{   
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		yield return new WaitForSeconds (startWait);
		// flip the velocity every betweenTimes seconds
		while (true) {
			rigidbody.velocity = Vector3.Scale (rigidbody.velocity, 
				new Vector3 (0.0f, 0.0f, -1.0f)); 
			yield return new WaitForSeconds (betweenTime);
		}
	}
}