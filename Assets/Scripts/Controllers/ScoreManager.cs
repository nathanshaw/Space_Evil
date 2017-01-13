using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a singleton class
// to access its functions use ScoreManager._instance.Funstion()
public class ScoreManager : MonoBehaviour {
	public static int[] scores = new int[] {0,0,0,0}; 
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

	void UpdateHighScore () {
		// intsted only check if thiss is true when player dies
		foreach (int score in scores) {
			if (score > highscore) { 
				highscore = score; 
				PlayerPrefs.SetInt ("highscore", highscore);
			}
		}
	}

	public void AddPoints(int playerIndex, int pointsToAdd) {
		scores[playerIndex] += pointsToAdd; 
		GUIController.Instance.SetULText ("" + playerIndex + ": " + scores[playerIndex]);
	}

	public void NewGame() {
		for (int i = 0; i < scores.Length; i++) {
			scores [i] = 0;
		}
	} 

	public int GetScore (int playerID) {
		return scores[playerID];
	} 
}

