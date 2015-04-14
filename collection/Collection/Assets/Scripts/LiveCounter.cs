using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LiveCounter : MonoBehaviour {

	public int initialLives = 3;
	public int extraLives = 0;
	public int totalLives;


	// Use this for initialization
	void Start () {
		GetLives ();
	}

	void GetLives(){
		totalLives = initialLives + extraLives;
	}
	
	// Update is called once per frame
	void Update () {
		totalLives = initialLives + extraLives;

		GetComponent<Text>().text = totalLives.ToString ();


	}
}
