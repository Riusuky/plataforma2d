using UnityEngine;
using System.Collections;

public class PlayerMeter : MonoBehaviour {

	public float air = 10;
	public float maxAir = 10;
	public float airBurnRate = 0f;
	public Texture2D bgTexture;
	public Texture2D airBarTexture;
	public int iconWidth = 32;
	public Vector2 airOffset = new Vector2(10, 10);
	
	private PlayerStateListener player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<PlayerStateListener> ();
	}
	
	void OnGUI(){
		var percent = Mathf.Clamp01 (air / maxAir);
		
		if (!player)
			percent = 0;
		
		DrawMeter (airOffset.x, airOffset.y, airBarTexture, bgTexture, percent);
	}
	
	void DrawMeter(float x, float y, Texture2D texture, Texture2D background, float percent){
		var bgW = background.width;
		var bgH = background.height;
		
		GUI.DrawTexture (new Rect (x, y, bgW, bgH), background);
		
		var nW = ((bgW - iconWidth) * percent) + iconWidth;
		
		GUI.BeginGroup (new Rect (x, y, nW, bgH));
		GUI.DrawTexture (new Rect (0, 0, bgW, bgH), texture);
		GUI.EndGroup ();
		
		
	}
	


	public void updateDamage(){


		if (!player)
			return;



		if (air > 0) {
			air -= Time.deltaTime * airBurnRate * 60;
			if(air	 <= 0){
				player.hitByCrusher();
			}
		} else {
			//Explode script = player.GetComponent<Explode>();
			//script.OnExplode();
			player.hitByCrusher();
			//Destroy(player);


		}
	}

}
