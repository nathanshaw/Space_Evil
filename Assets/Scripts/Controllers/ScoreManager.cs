using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a singleton class
// to access its functions use ScoreManager._instance.Funstion()
public class ScoreManager : MonoBehaviour
{
    public PlayerOne player1Properties;
    public PlayerTwo player2Properties;
    //public static int highscores[10];
    public static int highscore;

    public static ScoreManager Instance;
    public GUIController guiController;
    // Use this for initialization 

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", highscore);
        guiController.Awake();
        // player1Properties.Awake();
        // player2Properties.Awake();
        player1Properties.Start();
        player2Properties.Start();
    }

    void UpdateHighScore()
    {
        // intsted only check if thiss is true when player dies
        if (player1Properties.score > highscore)
        {
            highscore = player1Properties.score;
            PlayerPrefs.SetInt("highscore", highscore);
            PlayerPrefs.Save();
        }
        if (player2Properties.score > highscore)
        {
            highscore = player2Properties.score;
            PlayerPrefs.SetInt("highscore", highscore);
            PlayerPrefs.Save();
        }
    }

    public void AddPoints(int playerIndex, int pointsToAdd)
    {
		if (playerIndex == 1) {
        	player1Properties.score += pointsToAdd;
        	guiController.SetULText("" + player1Properties.score);
        }
        else if (playerIndex == 2)
        {
        	player2Properties.score += pointsToAdd;
            guiController.SetLLText("" + player2Properties.score);
        }
	}

    public void NewGame()
    {
		player1Properties.score = 0;
		guiController.SetLLText("0");
		player2Properties.score = 0;
		guiController.SetULText("0");
    }

    public int GetScore(int playerID)
    {
		if (playerID == 1) {
        	return player1Properties.score;
		}
		else if (playerID == 2) {
        	return player2Properties.score;
		}
		return 0;
    }
}

