using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
	private GameObject guiControllerO;
	public static int score; 
	//public static int highscores[10];
	public static int highscore;
	public static ScoreManager Instance;
	// Use this for initialization 

	public void Awake () {
		Instance = this;
	} 

	void Start () {
		highscore = PlayerPrefs.GetInt ("highscore", highscore);
		GUIController.Instance.Awake();
	}
	
	// Update is called once per frame
	void Update () {
		// intsted only check if thiss is true when player dies
		if (score > highscore) { 
			highscore = score; 
			PlayerPrefs.SetInt ("highscore", highscore);
		}
	}

	public void AddPoints(int pointsToAdd) {
		score += pointsToAdd; 
		GUIController.Instance.SetULText ("" + score);
	}

	public void NewGame() {
			score = 0;
	} 

	public int GetScore () {
		return score;
	} 
}

