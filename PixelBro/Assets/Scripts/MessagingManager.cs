using UnityEngine;
using System.Collections.Generic;
using System;

public class MessagingManager : MonoBehaviour {
	
	//public property for manager
	private List<Action> subscribers = new List<Action>();
	//static slington property
	public static MessagingManager Instance {
		get;
		private set;
	}


	void Awake(){
		Debug.Log ("Messaging Manager Started");
		//FIRST, we check if there are any other instances conflictiong
		if (Instance != null && Instance != this) {
			//Destroy other instance sif its not the same
			Destroy(gameObject);
		}

		//save our currrent singleton instance
		Instance = this;

		//make sure that the instance is not destroyed between scenes
		//(this is optional)
		DontDestroyOnLoad (gameObject);
	}


	//the subscribe method for manager
	public void Subscriber(Action subscriber){
		Debug.Log ("Subscriber registred");
		subscribers.Add (subscriber);
	}


	public void UnSubscriber(Action subscriber){
		Debug.Log ("remove");
		subscribers.Remove (subscriber);
	}

	public void ClearAllSubscriber(Action subscriber){
		Debug.Log ("remove");
		subscribers.Clear ();
	}

	public void Broadcast(){

		Debug.Log ("Broadcast requested, no of subscribers = " + subscribers.Count);

		foreach (var subscr in subscribers) {
			subscr();
		}
	}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
