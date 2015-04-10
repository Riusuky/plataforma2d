using UnityEngine;
using System.Collections;

public class EventDelegate : MonoBehaviour {

	//Delegate method definition
	public delegate void ClickAction ();
	//Event hook using delegate method sgnature

	public static event ClickAction OnClicked;

	// Use this for initialization
	void Start () {
		OnClicked += Events_OnClicked;
	}


	void OnDestroy(){
		OnClicked -= Events_OnClicked;
	}

	void Events_OnClicked(){
		Debug.Log ("The button was clicked");
	}
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Space)) {
			//trigger the event delegate if there is a subscriber
			if(OnClicked != null){
				OnClicked();
			}

		}
	}


}
