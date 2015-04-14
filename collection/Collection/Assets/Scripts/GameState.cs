using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	private GameObject[] coins;
	public int totalCoins;
	private CoinCount coinCounter;
	private LiveCounter liveCounter;

	public bool gameRunning = false;
	
	// Use this for initialization
	void Start () {
		coinCounter = GameObject.Find ("CoinCount").GetComponent<CoinCount> ();
		liveCounter = GameObject.Find ("LivesCount").GetComponent<LiveCounter> ();

		coins = GameObject.FindGameObjectsWithTag ("Coin");
		totalCoins = coins.Length;
	}
	
	// Update is called once per frame
	void Update () {
		int collectedCoins;
		collectedCoins = coinCounter.coinCount;

		liveCounter.extraLives = collectedCoins / totalCoins;
		if (liveCounter.totalLives < 0) {
			print("GAME OVER");
		}
	}

	public void StartGame(){
		gameRunning = true;
	}

	public void GameOver(){
		gameRunning = false;
		print ("Game Over");
	}


}
