using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	Collider collider;
	private BossController bossController;


	private CharacterMovement characterMovement;


	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake(){
		bossController = GameObject.FindGameObjectWithTag ("Boss").GetComponent<BossController> ();
		characterMovement = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement>();
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {

			bossController.bossAwake = true;

			collider.isTrigger = false;

			characterMovement.enabled = false;
		}
	}
}
