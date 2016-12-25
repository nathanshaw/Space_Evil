 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotPowerUp : MonoBehaviour {

	public int scoreValue;
	private GameController gameController;
	private GameObject gameControllerObject;
	public int multiShotChange;

	void Start () {
		gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// Use this for initialization
	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag ("Player") ) {
			gameController.PlayerMultiShotChange (multiShotChange);
		}
	}
}