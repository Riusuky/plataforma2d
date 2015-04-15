using UnityEngine;
using System.Collections;

public class enemyScript : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll){


		if (coll.gameObject.tag == "Player") {
			GameObject tmpPlayer = GameObject.Find("Player");
			tmpPlayer.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 2000);
			tmpPlayer.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2000);
			tmpPlayer.GetComponent<Collider2D>().enabled = false;

			GameObject.Find("Main Camera").GetComponent<playSound>().PlaySound("die");
		}
	}
}
