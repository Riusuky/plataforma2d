using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinCount : MonoBehaviour {

	public int coinCount = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = coinCount.ToString ();
	}
}
