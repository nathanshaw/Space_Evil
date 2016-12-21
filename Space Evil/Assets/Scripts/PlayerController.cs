using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour 
{
	public Boundary boundary;
	public float speed;
	private float vTilt;
	private float hTilt;
	public GameObject bolt;
	public Transform boltSpawn;
	public float boltFireRate;
	private float nextFire;
	private AudioSource audioSource;

	void Start() {
		audioSource = GetComponent<AudioSource> ();

	}

	//code that is run for every frame
	void Update() {
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + boltFireRate;
			Instantiate (bolt, boltSpawn.position, boltSpawn.rotation);
			audioSource.Play();
		};
			// TODO make it so you can do manual fireing
	}

	// executed once per physics step
	void FixedUpdate()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical); 
		rigidbody.velocity = movement * speed;
		rigidbody.position = new Vector3
		(
			Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
		rigidbody.rotation = Quaternion.Euler(rigidbody.velocity.x * hTilt, 90, rigidbody.velocity.z * vTilt);
	}

}
