using UnityEngine;
using System.Collections;

public class Delegate3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public class WorkerManager
	{
		void DoWork(){
			DoJob1 ();
			DoJob2 ();
			DoJob3 ();
		}

		private void DoJob1 (){
			//do some filing
		}

		private void DoJob2 (){
			//do some filing
		}

		private void DoJob3 (){
			//do some filing
		}
	}


	public class WorkerManager2{
		//workerManager delegate
		delegate void MyDelegateHook();
		MyDelegateHook ActionTodo;

		public string WorkerType = "Peon";

		//On Startup , assing jobs to the worker; note this is configurable instead of fixed

		void Start(){
			//Peon get lots of wor to do 
			if (WorkerType == "Peon") {
				ActionTodo += DoJob1 ();
				ActionTodo += DoJob2 ();
			} else {
				ActionTodo += DoJob3 ();
			
			}
		}

		void Update(){
			ActionTodo ();
		}
		void DoJob1(){}
		void DoJob2(){}
		void DoJob3(){}


	}
}
