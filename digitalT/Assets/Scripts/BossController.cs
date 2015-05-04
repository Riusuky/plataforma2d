using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour 
{
	public bool bossAwake = false;
	public bool inBattle = false;
	public bool attacking = false;

	public float idleTimer = 0.0f;
	public float idleWaitTime = 10.0f;

	public float attackTimer = 0.0f;
	public float attackWaitTime = 2.0f;
	public int attackCount = 0;

	private Animator anim;
	private BossHealth bossHealth;
	private CharacterHealth characterHealth;

	private BoxCollider handTrigger_L;
	private BoxCollider handTrigger_R;
	private SphereCollider eyeTrigger;

	public float blinkTimer = 0.0f;
	public float blinkWaitTime = 2.0f;
	
	// Use this for initialization
	void Awake () 
	{
		anim = GetComponentInChildren<Animator>();
		bossHealth = GetComponentInChildren<BossHealth>();
		characterHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHealth>();

		handTrigger_L = GameObject.Find ("HandTrigger_L").GetComponent<BoxCollider>();
		handTrigger_R = GameObject.Find ("HandTrigger_R").GetComponent<BoxCollider>();
		eyeTrigger = GameObject.Find ("EyeTrigger").GetComponent<SphereCollider>();



		anim.SetInteger("attackCount", attackCount);
	}

	public void UpdateAttackCount()
	{
		attackCount--;
	}
	// Update is called once per frame
	void Update () 
	{
		if(bossAwake)
		{
			print ("Boss is awake!");
			//Play Intro animation
			anim.SetBool("bossAwake", true);
			anim.SetInteger("attackCount", attackCount);

			if(inBattle)
			{
				blinkTimer += Time.deltaTime;

				if(blinkTimer >= blinkWaitTime)
				{
					EyeBlink();
					blinkTimer = 0.0f;
				}

				if(!attacking)
				{
					idleTimer += Time.deltaTime;
				}
				else
				{
					idleTimer = 0.0f;
					attackTimer += Time.deltaTime;
					anim.SetBool ("attackReady", true);

					if(attackTimer >= attackWaitTime)
					{
						attacking = false;
						anim.SetTrigger ("bossAttack");

						attackTimer = 0.0f;
						anim.SetBool("attackReady", false);

						print ("BOSS SMASH!");
						handTrigger_L.GetComponent<Collider>().enabled = true;
						handTrigger_R.GetComponent<Collider>().enabled = true;
					}
				}

				if(idleTimer >= idleWaitTime)
				{
					//Attack
					print ("Boss is attacking!");
					attacking = true;
					idleTimer = 0.0f;
				}
			}
			else
			{
				idleTimer = 0.0f;
			}

			if(bossHealth.health > 0 && characterHealth.health > 0)
			{
				if(attackCount == 0)
				{
					if(bossHealth.health == 4)
					{
						attackCount = 1;
						attackWaitTime = 4.0f;
					}

					if(bossHealth.health == 3)
					{
						attackCount = 2;
						attackWaitTime = 3.0f;
					}

					if(bossHealth.health == 2)
					{
						attackCount = 3;
						attackWaitTime = 2.0f;
					}

					if(bossHealth.health == 1)
					{
						attackCount = 4;
						attackWaitTime = 1.0f;
					}
				}
			}

		}
		
	}

	void EyeBlink()
	{
		if(bossHealth.health > 0)
		{
			anim.SetTrigger("blink");
		}
	}

	public void EnableEye()
	{
		eyeTrigger.GetComponent<Collider>().enabled = true;
	}

	public void DisableEye()
	{
		eyeTrigger.GetComponent<Collider>().enabled = false;
	}

}
