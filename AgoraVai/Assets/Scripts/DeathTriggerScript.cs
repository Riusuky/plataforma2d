using UnityEngine;
using System.Collections;

public class DeathTriggerScript : MonoBehaviour {

	private GameObject obj = null;
	private bool enemyDirection = true;

	void Start(){
		obj = this.transform.parent.gameObject;
	}


	void OnTriggerEnter2D( Collider2D collidedObject )
	{   
				//collidedObject.SendMessage("hitDeathTrigger", SendMessageOptions.DontRequireReceiver); 

		if (collidedObject.tag == "Player") {

			Vector3 localScale = obj.transform.localScale;
			

			if (localScale.x > 0) {
				enemyDirection = true;
			} else {
				enemyDirection = false;
			}
			
			collidedObject.SendMessage("takeDamage", enemyDirection);
		}


	}    
}
