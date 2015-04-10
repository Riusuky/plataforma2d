using UnityEngine;
using System.Collections;

public class Delegates : MonoBehaviour {

	delegate void RobotAction();
	RobotAction myRobotAction;

	// Use this for initialization
	void Start () {
		myRobotAction = RobotWalk;
	}
	
	// Update is called once per frame
	void Update () {
		myRobotAction();
	}

	public void DoRobotWalk(){
		myRobotAction = RobotWalk;
	}

	public void DoRobotRun(){
		myRobotAction = RobotRun;
	}


	void RobotRun(){
		Debug.Log ("Robot Run");
	}
	void RobotWalk(){
		Debug.Log ("Robot Walking");

	}
}
