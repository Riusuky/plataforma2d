using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour 
{
	public float xMargin = 1.0f;
	public float yMargin = 1.0f;

	public float xSmooth = 10.0f;
	public float ySmooth = 10.0f;

	public Vector2 maxXAndY;
	public Vector2 minXAndY;

	public Transform cameraTarget;

	// Use this for initialization
	void Awake () 
	{
		cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;
	}

	bool CheckXMargin()
	{
		return Mathf.Abs (transform.position.x - cameraTarget.position.x) > xMargin;
	}

	bool CheckYMargin()
	{
		return Mathf.Abs (transform.position.y - cameraTarget.position.y) > yMargin;
	}

	
	// Update is called once per frame
	void FixedUpdate () 
	{
		TrackPlayer();
	}

	void TrackPlayer()
	{
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		if(CheckXMargin())
		{
			targetX = Mathf.Lerp (transform.position.x, cameraTarget.position.x, xSmooth * Time.deltaTime);
		}

		if(CheckYMargin())
		{
			targetY = Mathf.Lerp (transform.position.y, cameraTarget.position.y, ySmooth * Time.deltaTime);
		}

		targetX = Mathf.Clamp (targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp (targetY, minXAndY.y, maxXAndY.y);

		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
