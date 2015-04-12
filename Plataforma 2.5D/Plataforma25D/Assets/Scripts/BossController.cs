using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {
	public bool bossAwake = false;
	public bool inBattle = false;
	public bool attacking = false;

	public float idleTimer = 0.0f;
	public float idleWaitTime = 10.0f;


	public float attackTimer = 0.0f;
	public float attackWaitTime = 2.0f;
	public int attackCount = 1;

	private Animator anim;
	private BossHealth bossHealth;

	private BoxCollider handTrigger_left;
	private BoxCollider handTrigger_right;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		anim = GetComponentInChildren<Animator> ();
		bossHealth = GetComponentInChildren<BossHealth> ();
		handTrigger_left = GameObject.Find ("HandTrigger_L").GetComponent<BoxCollider> ();
		handTrigger_right = GameObject.Find ("HandTrigger_R").GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {

		if (bossAwake) {

			anim.SetBool("bossAwake",true);
			if(inBattle){
				if(!attacking){
					idleTimer += Time.deltaTime;
				}else{
					idleTimer = 0.0f;
					attackTimer += Time.deltaTime;
					if(attackTimer >= attackWaitTime){
						attacking = false;
						attackTimer = 0.0f;
						print("Boss SMASH!");
						handTrigger_right.enabled = true;
						handTrigger_left.enabled = true;
					}
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
