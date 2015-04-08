using UnityEngine;
using System.Collections;

public class PlayerStateController : MonoBehaviour {

	public enum playerStates
	{
		idle = 0,
		left,
		right,
		jump,
		landing,
		falling,
		kill,
		resurrect,
		firingWeapon,
		attack,
		slide,
		takeDamage,
	   	continueGame,
		_stateCount
	}
	
	public static float[] stateDelayTimer = new float[(int)playerStates._stateCount];
	
	public delegate void playerStateHandler(PlayerStateController.playerStates newState);
	public static event playerStateHandler onStateChange;
	
	void LateUpdate () 
	{
		//if(!GameStates.gameActive)
		//	return;
		
		// Detect the current input of the Horizontal axis, then broadcast a state update for the player as appropriate
		float horizontal = Input.GetAxis("Horizontal");
		if(horizontal != 0.0f)
		{
			if(horizontal < 0.0f)
			{
				if(onStateChange != null)
					onStateChange(PlayerStateController.playerStates.left);
			}
			else
			{
				if(onStateChange != null)
					onStateChange(PlayerStateController.playerStates.right);
			}
		}
		else
		{
			if(onStateChange != null)
				onStateChange(PlayerStateController.playerStates.idle);
		}
		
		float jump = Input.GetAxis("Jump");
		if(jump > 0.0f)
		{
			if(onStateChange != null)
				onStateChange(PlayerStateController.playerStates.jump);
		}
		
		float firing = Input.GetAxis("Fire1");
		if(firing > 0.0f)
		{
			if(onStateChange != null)
				onStateChange(PlayerStateController.playerStates.firingWeapon);
		}

		float firing2 = Input.GetAxis("Fire2");
		if(firing2 > 0.0f)
		{
			if(onStateChange != null){
				onStateChange(PlayerStateController.playerStates.attack);
			}						
		}

		float firing3 = Input.GetAxis("Fire3");
		if(firing3 > 0.0f)
		{
			if(onStateChange != null){
				onStateChange(PlayerStateController.playerStates.slide);

			}						
		}
	}
}
