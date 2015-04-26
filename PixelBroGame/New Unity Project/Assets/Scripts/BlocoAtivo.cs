using UnityEngine;
using System.Collections;

public class BlocoAtivo : MonoBehaviour {

	public int totalParts  = 10;
	public BlocoAtivoPartes bodyPart;
	//public BlocoAtivoPartes ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D( Collider2D collidedObject ){

		if (collidedObject.tag == "PlayerBullet") {
			collidedObject.SendMessage("HitSomething");
			OnExplode();
		}
	}


	public void OnExplode(){
		Destroy (gameObject);
		
		var t = transform;
		
		for (int i = 0; i < totalParts; i++) {
			BlocoAtivoPartes clone = Instantiate(bodyPart, t.position, Quaternion.identity) as BlocoAtivoPartes;
			clone.GetComponent<Rigidbody2D>().AddForce(Vector3.right * (Random.Range (-50, 50)));
			clone.GetComponent<Rigidbody2D>().AddForce(Vector3.up * Random.Range(100, 400));
		}

	}

}
