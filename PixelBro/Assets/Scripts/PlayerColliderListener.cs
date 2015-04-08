using UnityEngine;
using System.Collections;

public class PlayerColliderListener : MonoBehaviour {

	public PlayerStateListener targetStateListener = null;
	
	void OnTriggerEnter2D( Collider2D collidedObject )
	{
		switch(collidedObject.tag)
		{
		case "Platform":
			// When the player lands on a platform, toggle the Landing state.
			targetStateListener.onStateChange(PlayerStateController.playerStates.landing);
			break;
			
		case "DeathTrigger":
			// Player hit the death trigger - kill 'em!
			targetStateListener.onStateChange(PlayerStateController.playerStates.kill);
			break;
		}
	}
	
	void OnTriggerExit2D( Collider2D collidedObject)
	{
		switch(collidedObject.tag)
		{
		case "Platform":
			// When the player leaves a platform, set the state as falling. If the player actually
			//     is not falling, this will get verified by the PlayerStateListener.
			targetStateListener.onStateChange(PlayerStateController.playerStates.falling);
			break;
		}         
	}
}
