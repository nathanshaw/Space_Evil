 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidePowerUp : MonoBehaviour {

	public int scoreValue;
	private GameController gameController;
	private GameObject gameControllerObject;
	public float sideSizeIncrease;
	public float sideDamageIncrease;

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
		if (other.CompareTag ("PlayerOne") ){
			gameController.playerControllers[0].SideSizeChange (sideSizeIncrease);
			gameController.playerControllers[0].SideDamageChange (sideDamageIncrease);
			ScoreManager.Instance.AddPoints (0, scoreValue);
		} 
		else if (other.CompareTag ("PlayerTwo") ){
			gameController.playerControllers[1].SideSizeChange (sideSizeIncrease);
			gameController.playerControllers[1].SideDamageChange (sideDamageIncrease);
			ScoreManager.Instance.AddPoints (1, scoreValue);
		}
	}
}