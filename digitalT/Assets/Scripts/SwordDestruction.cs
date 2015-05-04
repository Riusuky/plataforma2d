using UnityEngine;
using System.Collections;

public class SwordDestruction : MonoBehaviour 
{
	public float lifeSpan = 2.0f;

	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, lifeSpan);
	}
}
