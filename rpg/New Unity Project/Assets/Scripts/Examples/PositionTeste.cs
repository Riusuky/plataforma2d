﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class PositionTeste : MonoBehaviour {
	//Define a menu option in the editor to create the new asset
	[MenuItem("Assets/Create/PositionManager")]
	public static void CreateAsset()
	{

		 //Create a new instance of our scriptable object
		ScriptingObjects positionManager = ScriptableObject.CreateInstance<ScriptingObjects>();
		
		//Create a .asset file for our new object and save it
		AssetDatabase.CreateAsset(positionManager, "Assets/newPositionManager.asset");
		AssetDatabase.SaveAssets();
		
		//Now switch the inspector to our new object
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = positionManager;
	}
	// Use this for initialization


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
