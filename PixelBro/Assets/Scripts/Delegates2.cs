using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Delegates2 : MonoBehaviour {


	public class Worker{
		List<string> WorkCompletefor = new List<string>();
		public void doSomething(string ManagetName, Action myDelegate){
			WorkCompletefor.Add (ManagetName);

			//begin work
			myDelegate ();
		}

	}

	public class Manager{
		private Worker myWork = new Worker();

		public void PeiceOfWork1(){
			//a piece pf very long tedious work
		}

		public void PeiceOfWork2(){
			//a piece pf very long tedious work
		}
		public void DoWork(){
			//Send worker to do jobe 1
			myWork.doSomething ("Manager1", PeiceOfWork1);

			myWork.doSomething ("Manager1", PeiceOfWork2);
		}


	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
