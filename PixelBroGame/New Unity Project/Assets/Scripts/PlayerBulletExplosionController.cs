using UnityEngine;
using System.Collections;

public class PlayerBulletExplosionController : MonoBehaviour {

	private float selfDestructTimer = 0.0f;

	// Use this for initialization
	void Start () {
		selfDestructTimer = Time.time + 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(selfDestructTimer < Time.time) 
			Destroy(gameObject);
	}


}
