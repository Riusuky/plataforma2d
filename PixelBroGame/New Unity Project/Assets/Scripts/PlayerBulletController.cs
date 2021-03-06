﻿using UnityEngine;
using System.Collections;

public class PlayerBulletController : MonoBehaviour {

	public GameObject playerObject = null; // Will be populated automatically when the bullet is created in PlayerStateListener
	public GameObject bulletExplostionPrefab = null; 


	
	public float bulletSpeed = 20.0f;	
	private float selfDestructTimer = 0.0f;



	// Use this for initialization
	void Update () {

		if(selfDestructTimer > 0.0f)
		{
			if(selfDestructTimer < Time.time){
				//print("antes de destruir criar uma nov inst do bulllet explostion");

				InitExplostionEffect();
				Destroy(gameObject);
			}				
		}	
	}

	void InitExplostionEffect(){
		GameObject newBulletExplosion = (GameObject)Instantiate(bulletExplostionPrefab);
		newBulletExplosion.transform.position = transform.position;
		newBulletExplosion.GetComponent<Animator> ().SetInteger("animState",1);
	}
	
	// Update is called once per frame
	public void launchBullet () {
		// The local scale of the player object tells us which direction
		// the player is looking. Rather than programming in extra variables to
		// store where the player is looking, just check what already knows
		// that information... the object scale!
		float mainXScale = playerObject.transform.localScale.x;
		
		Vector2 bulletForce;
		
		if(mainXScale < 0.0f)
		{
			// Fire bullet left
			bulletForce = new Vector2(bulletSpeed * -1.0f,0.0f);
		}
		else
		{
			// Fire bullet right
			bulletForce = new Vector2(bulletSpeed,0.0f);
		}
		
		GetComponent<Rigidbody2D>().velocity = bulletForce;
		
		selfDestructTimer = Time.time + 0.30f;
	}

	public void HitSomething(){
		InitExplostionEffect();
		Destroy (gameObject);
	}





}
