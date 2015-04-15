using UnityEngine;
using System.Collections;

public class inputController : MonoBehaviour {

	private bool isMobile = true;
	private PlayerHandler _player;



	// Use this for initialization
	void Start () {
		if(Application.isEditor){
			isMobile = false;
		}

		_player = GameObject.Find ("Player").GetComponent<PlayerHandler> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (isMobile) {
			int tmC = Input.touchCount;
			tmC--;

			if (Input.GetTouch (tmC).phase == TouchPhase.Began) {
				handleInteraction(true);
			}
			if (Input.GetTouch (tmC).phase == TouchPhase.Ended) {
				handleInteraction(false);
			}

		} else {

			if(Input.GetMouseButtonDown(0)){
				handleInteraction(true);
			}

			if(Input.GetMouseButtonUp(0)){
				handleInteraction(false);
			}
		}
	}


	void handleInteraction(bool starting){
		if (starting) {
			_player.jump();
		} else {
			_player.jumpPress = false;
		}

	}
}
