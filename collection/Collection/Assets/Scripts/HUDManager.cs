using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void FadeInHUD(){
		anim.SetBool ("Fade",true);
	}
}
