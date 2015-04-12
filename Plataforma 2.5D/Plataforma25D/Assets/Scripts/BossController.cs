using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {
	public bool bossAwake = false;
	public bool inBattle = false;
	public bool attacking = false;

	public float idleTimer = 0.0f;
	public float idleWaitTime = 10.0f;


	private Animator anim;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		anim = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (bossAwake) {

			anim.SetBool("bossAwake",true);
			if(inBattle){
				if(!attacking){
					idleTimer += Time.deltaTime;
				}

				if(idleTimer >= idleWaitTime){
					//attack
					print("Boss is attacking");
					attacking = true;
					idleTimer = 0.0f;
				}
			}else{
				idleTimer = 0.0f;
			}

		}
	
	}
}
