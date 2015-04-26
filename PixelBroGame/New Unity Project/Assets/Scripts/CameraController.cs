using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public PlayerStateController.playerStates currentPlayerState = PlayerStateController.playerStates.idle;
	public GameObject playerObject = null;
	public float cameraTrackingSpeed = 0.2f;
	private Vector3 lastTargetPosition = Vector3.zero;
	private Vector3 currTargetPosition = Vector3.zero;
	private float currLerpDistance = 0.0f;
	
	void Start()
	{
		// Set the initial camera positioning to prevent any weird jerking around
		Vector3 playerPos = playerObject.transform.position;
		Vector3 cameraPos = transform.position;
		Vector3 startTargPos = playerPos;
		
		// Set the Z to the same as the camera so it does not move
		startTargPos.z = cameraPos.z;
		lastTargetPosition = startTargPos;
		currTargetPosition = startTargPos;
		currLerpDistance = 1.0f;
	}
	
	void OnEnable()
	{
		PlayerStateController.onStateChange += onPlayerStateChange;
	}
	
	void OnDisable()
	{
		PlayerStateController.onStateChange -= onPlayerStateChange;
	}
	
	void onPlayerStateChange(PlayerStateController.playerStates newState)
	{
		currentPlayerState = newState;
	}
	
	void LateUpdate()
	{
		// Update based on our current state
		onStateCycle();
		
		// Continue moving to the current target position
		currLerpDistance += cameraTrackingSpeed;
		transform.position = Vector3.Lerp(lastTargetPosition, currTargetPosition, currLerpDistance);
	}
	
	// Every cycle of the engine, process the current state
	void onStateCycle()
	{
		// We use the player state to determine the current action that the camera should take.
		//Notice that in most cases we are tracking the player - however in the case of
		//killing or resurrecting, we don't want to track the player.
		switch(currentPlayerState)
		{
		case PlayerStateController.playerStates.idle:
			trackPlayer();
			break;
			
		case PlayerStateController.playerStates.left:
			trackPlayer();
			break;
			
		case PlayerStateController.playerStates.right:
			trackPlayer();
			break;
			
		case PlayerStateController.playerStates.jump:
			trackPlayer();
			break;
			
		case PlayerStateController.playerStates.firingWeapon:
			trackPlayer();
			break;
		}
	}
	
	void trackPlayer()
	{
		// Get and store the current camera position, and the current player position, in world coordinates.
		Vector3 currCamPos = transform.position;
		Vector3 currPlayerPos = playerObject.transform.position;
		
		if(currCamPos.x == currPlayerPos.x && currCamPos.y == currPlayerPos.y)
		{
			// Positions are the same - tell the camera not to move, then abort.
			currLerpDistance = 1.0f;
			lastTargetPosition = currCamPos;
			currTargetPosition = currCamPos;
			return;
		}
		
		// Reset the travel distance for the lerp
		currLerpDistance = 0.0f;
		
		// Store the current target position so we can lerp from it
		lastTargetPosition = currCamPos;
		
		// Store the new target position
		currTargetPosition = currPlayerPos;
		
		// Change the Z position of the target to the same as the current. We don' want that to change.
		currTargetPosition.z = currCamPos.z;
	}
	
	void stopTrackingPlayer()
	{
		// Set the target positioning to the camera's current position to stop its movement in its tracks
		Vector3 currCamPos = transform.position;
		currTargetPosition = currCamPos;
		lastTargetPosition = currCamPos;
		
		// Also set the lerp progress distance to 1.0f, which will tell the lerping that it is finished.
		// Since we set the target positionins to the camera's current position, the camera will just
		// lerp to its current spot and stop there.
		currLerpDistance = 1.0f;
	}
}
