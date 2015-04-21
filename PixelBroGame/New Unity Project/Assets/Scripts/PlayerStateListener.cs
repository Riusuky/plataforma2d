using UnityEngine;
using System.Collections;

public class PlayerStateListener : MonoBehaviour {

	public float playerWalkSpeed = 3f;
	private bool playerHasLanded = true;

	private Animator playerAnimator = null;

	private PlayerStateController.playerStates previousState = PlayerStateController.playerStates.idle;
	private PlayerStateController.playerStates currentState = PlayerStateController.playerStates.idle;

	void OnEnable()
	{
		PlayerStateController.onStateChange += onStateChange;
	}
	
	void OnDisable()
	{
		PlayerStateController.onStateChange -= onStateChange;
	}

	// Use this for initialization
	void Awake () {
		playerAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate()
	{
		onStateCycle ();
	}

	void onStateCycle()
	{

		Vector3 localScale = transform.localScale;		
		transform.localEulerAngles = Vector3.zero;

		switch (currentState) {

			case PlayerStateController.playerStates.idle:
				break;
				
			case PlayerStateController.playerStates.left:
					transform.Translate(new Vector3((playerWalkSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
					if(localScale.x > 0.0f)
					{
						localScale.x *= -1.0f;
						transform.localScale  = localScale;
					}
				break;
				
			case PlayerStateController.playerStates.right:
					transform.Translate(new Vector3(playerWalkSpeed * Time.deltaTime, 0.0f, 0.0f));				
					if(localScale.x < 0.0f)
					{
						localScale.x *= -1.0f;
						transform.localScale = localScale;              
					}
				break;
		}
	}


	public void onStateChange(PlayerStateController.playerStates newState)
	{

		if(newState == currentState)
			return;

		if(checkIfAbortOnStateCondition(newState))
			return;

		if(!checkForValidStatePair(newState))
			return;

		
		switch (newState) {

			case PlayerStateController.playerStates.idle:
				Debug.Log("idle");
					playerAnimator.SetInteger("StatAnim", 0);
				break;
				
			case PlayerStateController.playerStates.left:
				Debug.Log("left");
					playerAnimator.SetInteger("StatAnim", 1);
				break;
				
			case PlayerStateController.playerStates.right:
				Debug.Log("right");
					playerAnimator.SetInteger("StatAnim", 1);
				break;

		case PlayerStateController.playerStates.jump:                   
			if(playerHasLanded)
			{
				// Use the jumpDirection variable to specify if the player should be jumping left, right or vertical
				Debug.Log("jump");
			}
			break;


		}

		// Store the current state as the previous state
		previousState = currentState;
		
		// And finally, assign the new state to the player object
		currentState = newState;
	}

	bool checkForValidStatePair(PlayerStateController.playerStates newState)
	{
		bool returnVal = false;
		
		// Compare the current against the new desired state.
		switch(currentState)
		{
		case PlayerStateController.playerStates.idle:
			// Any state can take over from idle.
			returnVal = true;
			break;
			
		case PlayerStateController.playerStates.left:
			// Any state can take over from the player moving left.
			returnVal = true;
			break;
			
		case PlayerStateController.playerStates.right:         
			// Any state can take over from the player moving right.
			returnVal = true;              
			break;


		case PlayerStateController.playerStates.jump:
			// The only state that can take over from Jump is landing or kill.		
			returnVal = true;
		
			break;

		}          
		return returnVal;
	}

	bool checkIfAbortOnStateCondition(PlayerStateController.playerStates newState)
	{
		bool returnVal = false;
		
		switch(newState)
		{
			case PlayerStateController.playerStates.idle:
				break;
				
			case PlayerStateController.playerStates.left:
				break;
				
			case PlayerStateController.playerStates.right:
				break;
		case PlayerStateController.playerStates.jump:
			float nextAllowedJumpTime = PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump ];
			
			if(nextAllowedJumpTime == 0.0f || nextAllowedJumpTime > Time.time)
				returnVal = true;
			break;
			

		}
		
		// Value of true means 'Abort'. Value of false means 'Continue'.
		return returnVal;
	}




}
