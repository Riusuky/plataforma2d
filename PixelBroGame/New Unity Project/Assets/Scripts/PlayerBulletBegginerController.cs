using UnityEngine;
using System.Collections;

public class PlayerBulletBegginerController : MonoBehaviour {



	private float selfDestructTimer = 0f;
	// Use this for initialization

	void Awake(){
		selfDestructTimer = Time.time + 0.07f;
	} 

	void Update () {
		if(selfDestructTimer < Time.time) 
			Destroy(gameObject);
	}

}
