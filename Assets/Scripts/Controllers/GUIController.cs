using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIController : MonoBehaviour {
	public static GUIController Instance;
	// upper left
	public TMP_Text ulText;
	// lower left
	public TMP_Text llText;
	// upper right
	public TMP_Text urText;
	// lower right
	public TMP_Text lrText;
	// center center
	public TMP_Text ccText; 
	// lower center
	// public TMP_Text lcText;
	// upper center
	// public TMP_Text ucText; 
	// health bar
	public GameObject upperHealthBar;
	public GameObject lowerHealthBar;

	// is there TMP_Text currently being displayed?
	bool lrNotificationBeingDisplayed;
	bool urNotificationBeingDisplayed;
	bool ccNotificationBeingDisplayed;

	// how long should notifications last for?
	public float notificationPeriodShort;
	public float notificationPeriodMedium;
	public float notificationPeriodLong;

	void Start () {
		// set the text in each of my possible locations to nothing
		string newText = "";
		ulText.text = newText;
        llText.text = newText;
        urText.text = newText;
        lrText.text = newText; 
		ccText.text = newText; 
		notificationPeriodShort = 2.0f;
		notificationPeriodMedium = 5.0f;
		notificationPeriodLong = 10.0f;
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
		StartCoroutine(UpperRightNotification(newText));
	}
	public void SetLRText (string newText){
		StartCoroutine(LowerRightNotification(newText)); 
	}
	public void SetCCText (string newText){
		StartCoroutine(CenterCenterNotification (newText));
	}

	public void UpdateUpperHealthBar(float size) {
		upperHealthBar.transform.localScale = new Vector3(
			size * 10, 
			upperHealthBar.transform.localScale.y, 
			upperHealthBar.transform.localScale.z
		);

		// set the healthbar color
		if (size > 0.5) {
			upperHealthBar.GetComponent<Renderer> ().material.color = Color.green;
		} 
		else if (size > 0.25f) {
			upperHealthBar.GetComponent<Renderer> ().material.color = Color.yellow;
		} 
		else {
			upperHealthBar.GetComponent<Renderer> ().material.color = Color.red;
		}
	}

	public void UpdateLowerHealthBar(float size) {
		lowerHealthBar.transform.localScale = new Vector3(
			size * 10, 
			lowerHealthBar.transform.localScale.y, 
			lowerHealthBar.transform.localScale.z
		);

		// set the healthbar color
		if (size > 0.5) {
			lowerHealthBar.GetComponent<Renderer> ().material.color = Color.green;
		} 
		else if (size > 0.25f) {
			lowerHealthBar.GetComponent<Renderer> ().material.color = Color.yellow;
		} 
		else {
			lowerHealthBar.GetComponent<Renderer> ().material.color = Color.red;
		}
	}

	private IEnumerator LowerRightNotification (string text) {
		if (lrNotificationBeingDisplayed == false) {
			lrNotificationBeingDisplayed = true;
			lrText.text = text;
			yield return new WaitForSeconds (notificationPeriodShort);
			lrText.text = "";
			lrNotificationBeingDisplayed = false;
			// wait 4 seconds then remove text
		} else {
			while (lrNotificationBeingDisplayed == true) {
				yield return new WaitForSeconds (0.3f);
			}
			lrNotificationBeingDisplayed = true;
			lrText.text = text;
			yield return new WaitForSeconds (notificationPeriodShort * 0.6f);
			lrText.text = "";
			lrNotificationBeingDisplayed = false;
		}
	}

	private IEnumerator UpperRightNotification (string text) {
		if (urNotificationBeingDisplayed == false) {
			urNotificationBeingDisplayed = true;
			urText.text = text;
			yield return new WaitForSeconds (notificationPeriodShort);
			urText.text = "";
			urNotificationBeingDisplayed = false;
			// wait 4 seconds then remove text
		} else {
			while (urNotificationBeingDisplayed == true) {
				yield return new WaitForSeconds (0.3f);
			}
			urNotificationBeingDisplayed = true;
			urText.text = text;
			yield return new WaitForSeconds (notificationPeriodShort * 0.6f);
			urText.text = "";
			urNotificationBeingDisplayed = false;
		}
	}

	private IEnumerator CenterCenterNotification (string text) {
		if (ccNotificationBeingDisplayed == false) {
			ccNotificationBeingDisplayed = true;
			ccText.text = text;
			Debug.Log ("started waiting for " + text);
			yield return new WaitForSeconds (notificationPeriodMedium);
			Debug.Log ("done waiting for " + text);
			ccText.text = "";
			ccNotificationBeingDisplayed = false;
			// wait 4 seconds then remove text
		} else {
			while (ccNotificationBeingDisplayed == true) {
				Debug.Log ("waiting for .3 seconds");
				yield return new WaitForSeconds (0.3f);
			}
			ccNotificationBeingDisplayed = true;
			ccText.text = text;
			Debug.Log ("started waiting for " + text);
			yield return new WaitForSeconds (notificationPeriodMedium);
			Debug.Log ("done waiting for " + text);
			ccText.text = "";
			ccNotificationBeingDisplayed = false;
		}
	}
}