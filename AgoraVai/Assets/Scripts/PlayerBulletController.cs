using UnityEngine;
using System.Collections;

public class PlayerBulletController : MonoBehaviour {

	public GameObject playerObject = null; // sera populado automaticamente quando o attack for executado no playerStateListener
	public float bulletSpeed = 15.0f;
	//public Rigidbody2D rigidbody2D;
	
	private float selfDestructTimer = 0.0f;

	void Start(){

	}

	void Update()
	{
		if(selfDestructTimer > 0.0f)
		{
			if(selfDestructTimer < Time.time)
				Destroy(gameObject);
		}
	}
	
	public void launchBullet()
	{
		//Local scale do jogador informa em qual direaçao o jogador esta.
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
		selfDestructTimer = Time.time + 1.0f;
	}
}
