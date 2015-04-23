using UnityEngine;
using System.Collections;

public class PlayerStateListener : MonoBehaviour {

	public float playerWalkSpeed = 3f;
	private bool playerHasLanded = true;


	//bullet
	public GameObject bulletPrefab = null; 
	public GameObject bulletInitPrefab = null; 
	public Transform bulletSpawnTransform;

	//JUMP
	public float playerJumpForceVertical = 500f;
	public float playerJumpForceHorizontal = 250f;
	Vector3 localScale;

	private Animator playerAnimator = null;
	private Rigidbody2D rigidbody2D;

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
		rigidbody2D = GetComponent<Rigidbody2D> ();
		 localScale = transform.localScale;		
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

			case PlayerStateController.playerStates.firingWeapon:

				break;

		}
	}


	public void onStateChange(PlayerStateController.playerStates newState)
	{

		//CONDICAO PARA LOOP
		if(!playerHasLanded){
			if(!inJumpAction(newState))
				return;
		}
			

		//if(newState == currentState)
		//			return;

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
					playerAnimator.SetInteger("StatAnim", 2);
					// Use the jumpDirection variable to specify if the player should be jumping left, right or vertical
					//float jumpDirection = 0.0f;
					//if(currentState == PlayerStateController.playerStates.left){
					//	jumpDirection = -1.0f;
					    //rigidbody2D.velocity = new Vector2 (jumpDirection* playerWalkSpeed, rigidbody2D.velocity.y);
					//}else if(currentState == PlayerStateController.playerStates.right){
					//	jumpDirection = 1.0f;
					    
					//}else{
					//	jumpDirection = 0.0f;
					//}
					
					// Apply the actual jump force
					//rigidbody2D.AddForce(new Vector2(jumpDirection * playerJumpForceHorizontal, playerJumpForceVertical));
					rigidbody2D.AddForce(new Vector2(0, playerJumpForceVertical));								
					playerHasLanded = false;
					PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump] = 0f;
				}
				break;

			case PlayerStateController.playerStates.landing:
				
				playerHasLanded = true;
				PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.jump]= Time.time + 0.1f;
			break;


			case PlayerStateController.playerStates.firingWeapon:
					// Make the bullet object
					GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
					GameObject newBulletInit = (GameObject)Instantiate(bulletInitPrefab);
					


					// Setup the bullet’s starting position
					newBullet.transform.position = bulletSpawnTransform.position;
					newBulletInit.transform.position = bulletSpawnTransform.position;
					

					PlayerBulletController bullCon = newBullet.GetComponent<PlayerBulletController>();
					// Set the player object
					bullCon.playerObject = gameObject;	
					// Launch the bullet!
					bullCon.launchBullet();
					// With the bullet made, set the state of the player back to the current state
					onStateChange(currentState);
					PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.firingWeapon] = Time.time + 0.25f;

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
			if(
				newState == PlayerStateController.playerStates.landing
				|| newState == PlayerStateController.playerStates.kill
				|| newState == PlayerStateController.playerStates.firingWeapon			
				)
				returnVal = true;
			else
				returnVal = false;
		
			break;

		case PlayerStateController.playerStates.landing:
			// The only state that can take over from landing is idle, left or right movement.
			if(
				newState == PlayerStateController.playerStates.left
				|| newState == PlayerStateController.playerStates.right
				|| newState == PlayerStateController.playerStates.idle
				|| newState == PlayerStateController.playerStates.firingWeapon
				)
				returnVal = true;
			else
				returnVal = false;

				break; 

			case PlayerStateController.playerStates.firingWeapon:
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
				if(nextAllowedJumpTime == 0.0f || nextAllowedJumpTime > Time.time){
					returnVal = true;
				}								
				break;

			case PlayerStateController.playerStates.landing:				
				break;

			case PlayerStateController.playerStates.firingWeapon:		

					if(PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.firingWeapon] > Time.time){
						returnVal = true;
					}
					
				break;


		}
		
		// Value of true means 'Abort'. Value of false means 'Continue'.
		return returnVal;
	}

	bool inJumpAction(PlayerStateController.playerStates newState){

		Vector3 localScale = transform.localScale;		
		transform.localEulerAngles = Vector3.zero;

		switch (newState) {
			case PlayerStateController.playerStates.right:
						rigidbody2D.velocity = new Vector2 (1 * 3.5f, rigidbody2D.velocity.y);						
						if(localScale.x < 0.0f)
						{
							localScale.x *= -1.0f;
							transform.localScale = localScale;              
						}
				break;
				
			case PlayerStateController.playerStates.left:		
						rigidbody2D.velocity = new Vector2 (-1 * 3.5f, rigidbody2D.velocity.y);
						if(localScale.x > 0.0f)
						{
							localScale.x *= -1.0f;
							transform.localScale  = localScale;
						}						
				break;
		}


		bool returnVal = true;
		return returnVal;
	}




}
