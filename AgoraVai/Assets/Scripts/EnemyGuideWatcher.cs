using UnityEngine;
using System.Collections;

public class EnemyGuideWatcher : MonoBehaviour {

	public EnemyControllerScript enemyObject = null;
	
	void OnTriggerExit2D( Collider2D otherObj )
	{
		// If this trigger just left a Platform object, then the enemy 
		// is about to walk off the platform. Tell the enemy that they need to switch directions!
		if(otherObj.tag == "Platform")
			enemyObject.switchDirections();
	}
}
