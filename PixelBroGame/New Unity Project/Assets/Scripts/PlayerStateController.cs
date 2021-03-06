﻿using UnityEngine;
using System.Collections;

public class PlayerStateController : MonoBehaviour {

	public enum playerStates
	{
		idle = 0,
		left,
		right,
		down,
		up,
		jump,
		landing,
		falling,
		kill,
		resurrect,
		firingWeapon,
		_stateCount
	}
	
	public static float[] stateDelayTimer = new float[(int)playerStates._stateCount];
	
	public delegate void playerStateHandler(PlayerStateController.playerStates newState);
	public static event playerStateHandler onStateChange;

	void LateUpdate () 
	{

		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		if (vertical != 0.0f) {
			if(vertical < 0.0f){
				if(onStateChange != null){
					onStateChange(PlayerStateController.playerStates.down);
				}
			}else{
				if(onStateChange != null){
					onStateChange(PlayerStateController.playerStates.up);
				}
			}
		}

		if(horizontal != 0.0f)
		{
			if(horizontal < 0.0f)
			{
				if(onStateChange != null){
					onStateChange(PlayerStateController.playerStates.left);
				}					
			}
			else
			{
				if(onStateChange != null){
					onStateChange(PlayerStateController.playerStates.right);
				}					
			}
		}
		else
		{
			if(onStateChange != null){
				onStateChange(PlayerStateController.playerStates.idle);
			}
				
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

		//

	}

}
