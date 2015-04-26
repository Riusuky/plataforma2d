using UnityEngine;
using System.Collections;

public class EnemyControllerScript : MonoBehaviour {

	private bool walkingLeft = true;
	public float walkingSpeed = 0.45f;
	// Use this for initialization
	void Start () {
		updateVisualWalkOrientation();
	}
	
	// Update is called once per frame
	void Update () {

		if(walkingLeft)
		{
			transform.Translate(new Vector3(walkingSpeed * Time.deltaTime, 0.0f, 0.0f));
		}
		else
		{
			transform.Translate(new Vector3((walkingSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
		}

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

	public void switchDirections()
	{
		// Swap the direction to be the opposite of whatever it 
		// currently is
		walkingLeft = !walkingLeft;
		
		// Update the orientation of the Enemy's material to match the
		// new walking direction
		updateVisualWalkOrientation();
	}

	public void HitByBullet(){
		Destroy (gameObject);
	}

}
