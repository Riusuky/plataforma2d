using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {


	Animator menuAnim;

	// Use this for initialization
	void Awake () { 
		menuAnim = GetComponent<Animator> ();
	}
	
	public void MenuFade(){
		menuAnim.SetBool ("FadeOut",true);
	}
};

