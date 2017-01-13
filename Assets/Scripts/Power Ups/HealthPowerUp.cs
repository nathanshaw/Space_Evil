 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour {

	public int scoreValue;
	private GameController gameController;
	private GameObject gameControllerObject;
	public float healthIncreaseMin; 
	public float healthIncreaseMax;  
	private float value;

	void Start () {
		float scale = Random.Range (0.0f, 1.0f);
		Debug.Log ("scale is  : " + scale);
		value = healthIncreaseMin + (scale * (healthIncreaseMax - healthIncreaseMin));   
			
		gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}  
			transform.localScale = new Vector3 (scale, scale, scale);
	}

	// Use this for initialization
	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag ("PlayerOne") ){
			gameController.playerControllers[0].HealthChange  (value);
			ScoreManager.Instance.AddPoints (0, scoreValue);
		} 
		else if (other.CompareTag ("PlayerTwo") ){
			gameController.playerControllers[1].HealthChange (value);
			ScoreManager.Instance.AddPoints (1, scoreValue);
		}
	}
}