using UnityEngine;
using System.Collections;

public class EscadaController : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D( Collider2D collidedObject ){
		if (collidedObject.tag == "Player") {
			collidedObject.SendMessage("EnterEscada");
		}
	}

	void OnTriggerExit2D( Collider2D collidedObject ){
		if (collidedObject.tag == "Player") {
			collidedObject.SendMessage("ExitEscada");
		}
	}
}
