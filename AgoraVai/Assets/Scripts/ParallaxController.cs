using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

	public GameObject[] clouds;
	public GameObject[] nearHills;
	public GameObject[] farHills;
	public GameObject[] lava;
	
	public float cloudLayerSpeedModifier;
	public float nearHillLayerSpeedModifier;
	public float farHillLayerSpeedModifier;
	public float lavalLayerSpeedModifier;
	
	public Camera myCamera;
	
	private Vector3 lastCamPos;
	
	void Start()
	{
		lastCamPos = myCamera.transform.position;
	}
	
	void Update()
	{
		Vector3 currCamPos = myCamera.transform.position;
		float xPosDiff = lastCamPos.x - currCamPos.x;
		
		adjustParallaxPositionsForArray(clouds, cloudLayerSpeedModifier, xPosDiff);
		adjustParallaxPositionsForArray(nearHills, nearHillLayerSpeedModifier, xPosDiff);
		adjustParallaxPositionsForArray(farHills, farHillLayerSpeedModifier, xPosDiff);
		adjustParallaxPositionsForArray(lava, lavalLayerSpeedModifier, xPosDiff);
		
		lastCamPos = myCamera.transform.position;
	}
	
	void adjustParallaxPositionsForArray(GameObject[] layerArray, float layerSpeedModifier, float xPosDiff)
	{
		for(int i = 0; i < layerArray.Length; i++)
		{
			Vector3 objPos = layerArray[i].transform.position;
			objPos.x += xPosDiff * layerSpeedModifier;
			layerArray[i].transform.position = objPos;
		}
	}
}
