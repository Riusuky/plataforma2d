using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour {

	private CharacterMovement characterMovement;
	private BossController bossController;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		characterMovement = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ();
		bossController = GameObject.FindGameObjectWithTag ("Boss").GetComponent<BossController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void EnableBossBattle(){
		characterMovement.enabled = true;
		bossController.inBattle = true;
	}
}
