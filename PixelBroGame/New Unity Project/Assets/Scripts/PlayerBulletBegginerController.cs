using UnityEngine;
using System.Collections;

public class PlayerBulletBegginerController : MonoBehaviour {



	private float selfDestructTimer = 0f;
	public Transform bulletSpawn;


	// Use this for initialization

	void Awake(){
		selfDestructTimer = Time.time + 0.07f;
	} 

	void Update () {

		transform.position = bulletSpawn.position;

		if(selfDestructTimer < Time.time) 
			Destroy(gameObject);
	}

}
