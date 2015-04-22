using UnityEngine;
using System.Collections;

public class PlayerColliderListener : MonoBehaviour {

	public PlayerStateListener targetStateListener = null;


	void OnTriggerEnter2D(Collider2D collidedObject){
		switch(collidedObject.tag){

			case "Platform":
				
				// When the player lands on a platform, toggle the Landing state.
				targetStateListener.onStateChange(PlayerStateController.playerStates.landing);

				break;

		}
	}


}
