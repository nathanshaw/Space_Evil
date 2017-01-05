using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
	public static int score; 
	//public static int highscores[10];
	public static int highscore;
	// Use this for initialization
	void Start () {
		highscore = PlayerPrefs.GetInt ("highscore", highscore);
	}
	
	// Update is called once per frame
	void Update () {
		// intsted only check if thiss is true when player dies
		if (score > highscore) { 


			highscore = score; 
			PlayerPrefs.SetInt ("highscore", highscore);
	}
}
}
