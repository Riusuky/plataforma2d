using UnityEngine;
using System.Collections;

public class playSound : MonoBehaviour {

	private AudioSource[] _audioSource;


	// Use this for initialization
	void Start () {
		_audioSource = this.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySound(string type){
		switch (type) {
		case "jump":
			_audioSource[1].Play();
			break;

		case "power":
			_audioSource[0].Play();
			break;

		case "die":
			_audioSource[2].Play();
			break;

		}
	}

}
