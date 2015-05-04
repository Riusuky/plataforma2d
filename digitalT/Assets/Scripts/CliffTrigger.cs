using UnityEngine;
using System.Collections;

public class CliffTrigger : MonoBehaviour 
{
	public bool playerRight;
	private Animator anim;
	
	// Use this for initialization
	void Start () 
	{
		anim = GameObject.Find("Boss_Rig").GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			anim.SetBool("playerRight", playerRight);
		}
	}
}
