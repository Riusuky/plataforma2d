using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	private CoinCount coinCount;

	void Awake(){
		coinCount = GameObject.Find("CoinCount").GetComponent<CoinCount>();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			gameObject.SetActive(false);

			coinCount.coinCount++;
		}
	}
}
