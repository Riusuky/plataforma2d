using UnityEngine;
using System.Collections;

public class EnemyDefence : MonoBehaviour {

	public EnemyControllerScript enemyController; 

	void Start(){
		//enemyController = GetComponent<>
	}

	void OnTriggerEnter2D( Collider2D collidedObject ){
		
		if (collidedObject.tag == "PlayerBullet") {
			collidedObject.SendMessage("HitSomething");
			enemyController.HitByBullet();
		}
	}


}
