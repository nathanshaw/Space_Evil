using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {
	public static GUIController Instance;
	// upper left
	public GUIText ulText;
	// lower left
	public GUIText llText;
	// upper right
	public GUIText urText;
	// lower right
	public GUIText lrText;
	// center center
	public GUIText ccText; 
	// lower center
	public GUIText lcText;
	// upper center
	public GUIText ucText; 
	// health bar
	public GameObject healthBar; 

	bool lrNotificationBeingDisplayed;
	bool urNotificationBeingDisplayed;

	public float lrNotificationWaitTime;
	public float urNotificationWaitTime;

	void Start () {
		string newText = "";
		ulText.text = newText; 
		llText.text = newText; 
		urText.text = newText; 
		lrText.text = newText; 
		lcText.text = newText; 
		ccText.text = newText; 
		ucText.text = newText; 
	}

	void Update () {
		
	}

	public void Awake() {
		Instance = this;
	}

	public void SetULText (string newText){
		ulText.text = newText; 
	}
	public void SetLLText (string newText){
		llText.text = newText; 
	}
	public void SetURText (string newText){
		urText.text = newText; 
	}
	public void SetLRText (string newText){
		lrText.text = newText; 
	}
	public void SetLCText (string newText){
		lcText.text = newText; 
	}
	public void SetCCText (string newText){
		ccText.text = newText; 
	}
	public void SetUCText (string newText){
		ucText.text = newText; 
	}

	public void UpdateHealthBar(float size) {
		healthBar.transform.localScale = new Vector3(
			size * 10, 
			healthBar.transform.localScale.y, 
			healthBar.transform.localScale.z
		);

		// set the healthbar color
		if (size > 0.5) {
			healthBar.GetComponent<Renderer> ().material.color = Color.green;
		} 
		else if (size > 0.25f) {
			healthBar.GetComponent<Renderer> ().material.color = Color.yellow;
		} 
		else {
			healthBar.GetComponent<Renderer> ().material.color = Color.red;
		}
	}

	public IEnumerator LowerRightNotification (string text) {
		if (lrNotificationBeingDisplayed == false) {
			lrNotificationBeingDisplayed = true;
			lrText.text = text;
			yield return new WaitForSeconds (lrNotificationWaitTime);
			lrText.text = "";
			lrNotificationBeingDisplayed = false;
			// wait 4 seconds then remove text
		} else {
			while (lrNotificationBeingDisplayed == true) {
				yield return new WaitForSeconds (0.3f);
			}
			lrNotificationBeingDisplayed = true;
			lrText.text = text;
			yield return new WaitForSeconds (lrNotificationWaitTime * 0.6f);
			lrText.text = "";
			lrNotificationBeingDisplayed = false;
		}
	}

	public IEnumerator UpperRightNotification (string text) {
		if (urNotificationBeingDisplayed == false) {
			urNotificationBeingDisplayed = true;
			urText.text = text;
			yield return new WaitForSeconds (urNotificationWaitTime);
			urText.text = "";
			urNotificationBeingDisplayed = false;
			// wait 4 seconds then remove text
		} else {
			while (urNotificationBeingDisplayed == true) {
				yield return new WaitForSeconds (0.3f);
			}
			urNotificationBeingDisplayed = true;
			urText.text = text;
			yield return new WaitForSeconds (urNotificationWaitTime * 0.6f);
			urText.text = "";
			urNotificationBeingDisplayed = false;
		}
	}

}
