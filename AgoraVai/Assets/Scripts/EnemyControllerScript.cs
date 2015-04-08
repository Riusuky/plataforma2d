using UnityEngine;
using System.Collections;

public class EnemyControllerScript : MonoBehaviour {

	// States to allow objects to know when an enemy dies
	public delegate void enemyEventHandler(int scoreMod);
	public static event enemyEventHandler enemyDied;
	
	//public TakeDamageFromPlayerBullet bulletColliderListener = null;
	public float walkingSpeed = 0.45f;
	public GameObject deathFxParticlePrefab = null;
	
	private bool walkingLeft = true;
	
	void OnEnable()
	{
		// Subscribe to events from the bullet collider 
		//bulletColliderListener.hitByBullet += hitByPlayerBullet; 
	}
	
	void OnDisable()
	{
		// Unsubscribe from events
		//bulletColliderListener.hitByBullet -= hitByPlayerBullet;
	}
	
	void Start()
	{
		// Randomly default the enemy’s direction
		walkingLeft = (Random.Range(0,2) == 1);
		updateVisualWalkOrientation();
		
		// Find the round watcher object
		//GameObject roundWatcherObject = GameObject.FindGameObjectWithTag("RoundWatcher");
		
		//if (roundWatcherObject != null)
		//{
			//RoundWatcher roundWatcherComponent = roundWatcherObject.GetComponent<RoundWatcher>();
			
			// Increase the enemy speed for each round.
			//walkingSpeed = walkingSpeed * roundWatcherComponent.currRound;
		//}
	}
	
	void Update()
	{
		// Translate the enemy's position based on the direction that
		// they are currently moving.
		if(walkingLeft)
		{
			transform.Translate(new Vector3(walkingSpeed * Time.deltaTime, 0.0f, 0.0f));
		}
		else
		{
			transform.Translate(new Vector3((walkingSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
		}
	}
	
	public void switchDirections()
	{
		// Swap the direction to be the opposite of whatever it 
		// currently is
		walkingLeft = !walkingLeft;
		
		// Update the orientation of the Enemy's material to match the
		// new walking direction
		updateVisualWalkOrientation();
	}
	
	void updateVisualWalkOrientation()
	{
		Vector3 localScale = transform.localScale;
		if(walkingLeft)
		{
			if(localScale.x > 0.0f)
			{
				localScale.x = localScale.x * -1.0f;
				transform.localScale  = localScale;
			}
		}
		else
		{
			if(localScale.x < 0.0f)
			{
				localScale.x = localScale.x * -1.0f;
				transform.localScale  = localScale;              
			}
		} 
	}
	
	public void hitByPlayerBullet()
	{
		// Call the EnemyDied event and give it a score of 25.
		if(enemyDied != null)
			enemyDied(25);
		
		handleEnemyDeath();
	}
	
	public void hitByCrusher()
	{
		// Enemy was crushed, but don't give any points to the player.
		if(enemyDied != null)
			enemyDied(0);
		
		handleEnemyDeath();
	}
	
	void handleEnemyDeath()
	{
		// Create the particle emitter object.
		GameObject deathFxParticle = (GameObject)Instantiate(deathFxParticlePrefab); 
		
		// Get the enemy position
		Vector3 enemyPos = transform.position;
		
		// Create a new vector that is in front of the enemy
		Vector3 particlePosition = new Vector3(enemyPos.x,enemyPos.y,enemyPos.z + 1.0f);
		
		// Reposition the particle emitter at this new position
		deathFxParticle.transform.position = particlePosition;
		
		Destroy(gameObject,0.1f);
	}
}
