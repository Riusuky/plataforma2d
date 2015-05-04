using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour 
{
	public int health = 4;
	public float timer = 0.0f;
	public float waitTime = 2.0f;
	public bool bossDead;

	private BossController bossController;
	private Animator anim;

	// Use this for initialization
	void Awake () 
	{
		bossController = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
		anim = GameObject.Find ("Boss_Rig").GetComponent<Animator>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(health <= 0)
		{
			if(!bossDead)
			{
				BossDying();
			}

			else
			{
				BossDead();
				LevelReset();
			}

		}
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Projectile" && health > 0)
		{
			if(health != 1)
			anim.SetTrigger("isHit");

			health--;
			print ("Boss Health: " + health);

			Destroy (other.gameObject);
			bossController.attacking = true;
		}
	}

	void BossDying()
	{
		bossDead = true;
		anim.SetBool ("isDead", bossDead);
	}

	void BossDead()
	{
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Boss_Death"))
		{
			anim.SetBool ("isDead", false);
		}
	}


	void LevelReset()
	{
		timer += Time.deltaTime;
		
		if(timer >= waitTime)
		{
			Application.LoadLevel(0);
		}
	}
}
