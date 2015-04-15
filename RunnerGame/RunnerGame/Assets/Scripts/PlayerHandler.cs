using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {

	private bool inAir = false;
	private int _animState = Animator.StringToHash("animState");
	private Animator _animator;
	public bool jumpPress = false;

	Rigidbody2D rigidbody;


	// Use this for initialization
	void Awake () {
		rigidbody = GetComponent<Rigidbody2D> ();
		_animator = this.transform.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!inAir && Mathf.Abs(rigidbody.velocity.y) > 0.05f) {
			_animator.SetInteger (_animState, 1);
			inAir = true;
		} else if(inAir && rigidbody.velocity.y == 0.00f){
			_animator.SetInteger (_animState, 0);
			inAir = false;
			if(jumpPress)
				jump();
		}

	}

	public void jump(){
		jumpPress = true;
		if (inAir)
			return;

		rigidbody.AddForce (Vector2.up * 3000);
		GameObject.Find("Main Camera").GetComponent<playSound>().PlaySound("jump");

	}
}
