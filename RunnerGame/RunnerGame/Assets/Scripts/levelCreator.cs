using UnityEngine;
using System.Collections;

public class levelCreator : MonoBehaviour {

	//use this for initialization
	public GameObject tilePos;
	private float startUpPosY;
	private const float tileWidth = .70f;
	private int heightLevel = 0;
	private GameObject tmptile;

	public float gameSpeed = 3.0f;
	private float outofbounceX;
	private int blankCounter = 0;
	private int middleCounter = 0;
	private string lastTile = "right";

	private float startTime;

	private bool enemyAdded = false;

	private GameObject collectedTiles;
	private GameObject gameLayer;
	private GameObject bgLayer;


	private float outOfBounceY;
	private GameObject _player;

	void Awake(){
		Application.targetFrameRate = 60;
	}
	// Use this for initialization
	void Start () {
		gameLayer = GameObject.Find ("gameLayer");	
		bgLayer = GameObject.Find ("backgroundLayer");
		collectedTiles = GameObject.Find ("tiles");


		for (int i = 0; i<21; i++) {
			GameObject tmpg1 = Instantiate(Resources.Load("grass_left", typeof(GameObject))) as GameObject;
			tmpg1.transform.parent = collectedTiles.transform.FindChild("gleft").transform;
			tmpg1.transform.position = Vector2.zero;

			GameObject tmpG2 = Instantiate(Resources.Load("grass_middle", typeof(GameObject))) as GameObject;
			tmpG2.transform.parent = collectedTiles.transform.FindChild("gMiddle").transform;
			tmpG2.transform.position = Vector2.zero;

			GameObject tmpG3 = Instantiate(Resources.Load("grass_right", typeof(GameObject))) as GameObject;
			tmpG3.transform.parent = collectedTiles.transform.FindChild("gRight").transform;
			tmpG3.transform.position = Vector2.zero;

			GameObject tmpG4 = Instantiate(Resources.Load("blank", typeof(GameObject))) as GameObject;
			tmpG4.transform.parent = collectedTiles.transform.FindChild("gBlank").transform;
			tmpG4.transform.position = Vector2.zero;


		}

		for (int i = 0; i < 10; i++) {
			GameObject tmpG5 = Instantiate(Resources.Load("enemy", typeof(GameObject))) as GameObject;
			tmpG5.transform.parent = collectedTiles.transform.FindChild("Killers").transform;
			tmpG5.transform.position = Vector2.zero;

		}


		collectedTiles.transform.position = new Vector2(-60.0f,-20.0f);
		tilePos = GameObject.Find("startTilePosistion");
		startUpPosY = tilePos.transform.position.y;		
		outofbounceX = tilePos.transform.position.x - 5.0f;

		outOfBounceY = startUpPosY - 3.0f;
		_player = GameObject.Find ("Player");

		fillScene ();
		startTime = Time.time;

	}
	
	// Update is called once per frame
	void FixedUpdate () {


		if (startTime - Time.time % 5 == 0) {
			gameSpeed += 0.5f;
		}

		gameLayer.transform.position = new Vector2 (gameLayer.transform.position.x - gameSpeed * Time.deltaTime, 0);
		bgLayer.transform.position = new Vector2 (bgLayer.transform.position.x - gameSpeed/4 * Time.deltaTime, 0);

		foreach (Transform child in gameLayer.transform)
		{
			if(child.position.x < outofbounceX){
				
				switch (child.gameObject.name) {
					
				case "grass_left(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gleft").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gleft").transform;
					break;
					
				case "grass_right(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gRight").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gRight").transform;
					break;
					
				case "grass_middle(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gMiddle").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gMiddle").transform;
					
					break;
					
				case "blank(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("gBlank").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("gBlank").transform;
					
					break;

				case "enemy(Clone)":
					child.gameObject.transform.position = collectedTiles.transform.FindChild("Killers").transform.position;
					child.gameObject.transform.parent = collectedTiles.transform.FindChild("Killers").transform;

									
					break;	


				case "Reward":
						
					GameObject.Find("Reward").GetComponent<crateScript>().inPlay = false;


					break;


				default:
					Destroy(child.gameObject);
					
					break;
					
				}
			}
		}

		if (gameLayer.transform.childCount < 25) {
			spawnTile();
		}

		if (_player.transform.position.y < outOfBounceY) {
			killPlayer();
		}

	}

	private void killPlayer(){
		this.GetComponent<scoreHandler> ().sendToHighScore ();
		Application.LoadLevel (0);
	}

	private void spawnTile(){
			
		print (lastTile);
		//print ();

		if (blankCounter > 0) {
			setTile("blank");
			blankCounter--;
			return;
		}

		if (middleCounter > 0) {
			randomizeEnemy();
			setTile("middle");
			middleCounter--;
			return;
		}
		enemyAdded = false;

		if (lastTile == "blank") {
			changeHeight ();
			setTile ("left");
			middleCounter = (int)Random.Range (1, 8);

		} else if (lastTile == "right") {

			this.GetComponent<scoreHandler>().Point++;

			blankCounter = (int)Random.Range (1, 3);
		} else if (lastTile == "middle") {
			setTile("right");
		}

	}

	private void changeHeight(){
		int newHeightLevel = (int)Random.Range (0,4);
		if (newHeightLevel < heightLevel) {		
			heightLevel--;
		}else{
			heightLevel++;
		}

	}

	private void fillScene(){
		for (int i = 0; i <15; i++) {
			setTile("middle");
		}

		setTile ("right");
	}

	public void setTile(string type){

		switch (type) {

			case "left":
			tmptile = collectedTiles.transform.FindChild("gleft").transform.GetChild(0).gameObject;
				break;

			case "right":
			tmptile = collectedTiles.transform.FindChild("gRight").transform.GetChild(0).gameObject;
				break;

			case "middle":

			tmptile = collectedTiles.transform.FindChild("gMiddle").transform.GetChild(0).gameObject;
				break;

			case "blank":
			tmptile = collectedTiles.transform.FindChild("gBlank").transform.GetChild(0).gameObject;
				break;

		}

		tmptile.transform.parent = gameLayer.transform;
		tmptile.transform.position = new Vector2(tilePos.transform.position.x + (tileWidth), startUpPosY + (heightLevel * tileWidth));
		
		tilePos = tmptile;
		lastTile = type;


	}

	private void randomizeEnemy(){
		if (enemyAdded) {
			return;
		}

		if ((int)Random.Range (0, 4) == 1) {
			GameObject newEnemy = collectedTiles.transform.FindChild("Killers").transform.GetChild(0).gameObject;
			newEnemy.transform.parent = gameLayer.transform;

			newEnemy.transform.position = new Vector2(tilePos.transform.position.x + tileWidth, startUpPosY + (heightLevel * tileWidth + (tileWidth*1)));
			enemyAdded = true;
		}

	}

}
