using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour {

	public int health = 4;
	public float timer = 0.0f;
	public float waitTime = 2.0f;

	private BossController bossController;



	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
		bossController = GameObject.FindGameObjectWithTag ("Boss").GetComponent<BossController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (health == 0) {
			//play boss death animation
			LevelReset();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Projectile" && health > 0) {
			health--;
			print("Boss Health: " + health);

			Destroy(other.gameObject);
			bossController.attacking = true;
		}
	}

	void LevelReset(){
		timer += Time.deltaTime;
		if (timer >= waitTime) {			
			Application.LoadLevel(0);		
		}		
	}
}
