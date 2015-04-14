using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {


	public float currentTimer;
	public float startTimer = 10f;
	public float timerPercent;
	private Image image;


	private GameState gameState;

	// Use this for initialization
	void Awake () {
		currentTimer = startTimer;
		image = GetComponent<Image> ();
		gameState = GameObject.Find ("GameState").GetComponent<GameState>();
	}
	
	// Update is called once per frame
	void Update () {

		if (gameState.gameRunning) {
			currentTimer -= Time.deltaTime;
			timerPercent = currentTimer / startTimer;
			image.fillAmount = timerPercent;
		}

	
	}
}
