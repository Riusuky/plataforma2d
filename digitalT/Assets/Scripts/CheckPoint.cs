using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{
	private BossController bossController;
	private CharacterMovement characterMovement;
	private Animator anim;
	
	// Use this for initialization
	void Awake () 
	{
		bossController = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
		characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
		anim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			//Change the camera behavior
			//Wake up the boss
			bossController.bossAwake = true;
			
			GetComponent<Collider>().isTrigger = false;
			anim.SetFloat ("speed", 0.0f);
			//Disable Character Movement
			characterMovement.enabled = false;

		}
	}
}
