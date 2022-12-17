using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex (int index) {
		SceneManager.LoadScene (index);
	}

    // TODO - start a new 1P game
    public void New1PGameYes()
    {
        PlayerPrefs.SetInt("NumPlayers", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("ship_customization");
    }

    // TODO - right now these buttons do the same thing =)
    public void New2PGameYes()
    {
        PlayerPrefs.SetInt("NumPlayers", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene("ship_customization");
    }
}
