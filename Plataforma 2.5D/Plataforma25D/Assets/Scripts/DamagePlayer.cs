using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour {

	private CharacterHealth characterHealth;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		characterHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterHealth>();
	}
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			characterHealth.health--;
			if(characterHealth.health <= 0)
			{

				//PLAY CHARACTER DEATH animation
				print("Player is dead");
			}
		}
	}
}
