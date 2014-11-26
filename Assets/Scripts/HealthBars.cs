using UnityEngine;
using System.Collections;

public class HealthBars : MonoBehaviour {
	
		public float protein = 50f;				// The player's protein level.
		public float carb = 50f;					// The player's carb level.	
		public float fat = 50f;					// The player's fat level.
		public AudioClip[] tauntClips;				// Array of clips to play when the player eats food.
		
		private bool alive = true;
		private SpriteRenderer proteinBar;			// Reference to the sprite renderer of the protein bar.
		private SpriteRenderer carbBar;				// Reference to the sprite renderer of the carb bar.
		private SpriteRenderer fatBar;				// Reference to the sprite renderer of the fat bar.
		private Vector3 proteinScale;				// The local scale of the protein bar initially (with half protein).
		private Vector3 carbScale;					// The local scale of the carb bar initially (with half carb).
		private Vector3 fatScale;					// The local scale of the fat bar initially (with half fat).
		private PlayerControl playerControl;		// Reference to the PlayerControl script.
		private Animator anim;						// Reference to the Animator on the player.
		
		
		void Awake ()
		{
			// Setting up references.
			playerControl = GetComponent<PlayerControl>();
			proteinBar = GameObject.Find("ProteinBar").GetComponent<SpriteRenderer>();
			carbBar = GameObject.Find("CarbBar").GetComponent<SpriteRenderer>();
			fatBar = GameObject.Find("FatBar").GetComponent<SpriteRenderer>();
			anim = GetComponent<Animator>();
			
			// Getting the intial scale of the healthbar (whilst the player has full health).
			proteinScale = proteinBar.transform.localScale;
			carbScale = carbBar.transform.localScale;
			fatScale = fatBar.transform.localScale;
		}

		void Update()
		{
			//StartCoroutine (AutoDecrease());
			UpdateBars ();
		}
		
		
		void OnCollisionEnter2D (Collision2D col)
		{
			// If the colliding gameobject is food
			if(col.transform.tag == "Food") 
			{
				// ... and if the player still has valid nutritions level...
				if(	protein < 100f &&
			 		  carb < 100f &&
			   			fat < 100f)
				{
					// ... eat the food
					if(col.transform.name == "banana(Clone)"){
						Eat(30,30,30);
						Application.ExternalCall( "ComerPlatano", "Comiste un Platano" );
					}
					else if(col.transform.name == "candy1(Clone)"){
						Eat(5,5,40); 
						Application.ExternalCall( "ComerDulce", "Comiste un Dulce" );
					}
					else if(col.transform.name == "candy2(Clone)"){
						Eat(0,0,20); 
						Application.ExternalCall( "ComerDulce", "Comiste un Dulce" );
					}
					else if(col.transform.name == "candy4(Clone)"){
						Eat(10,15,20); 
						Application.ExternalCall( "ComerDulce", "Comiste un Dulce" );
					}
					else if(col.transform.name == "candy11(Clone)"){
						Eat(10,15,30); 
						Application.ExternalCall( "ComerDulce", "Comiste un Dulce" );
					}
					else if(col.transform.name == "orange(Clone)" ){
						Eat(20,10,10); 
						Application.ExternalCall( "ComerNaranja", "Comiste una Naranja" );
					}
					else if(col.transform.name == "lime2(Clone)"){
						Eat(10,15,0); 
						Application.ExternalCall( "ComerLima", "Comiste una Lima" );
					}
					else if(col.transform.name == "lemon2(Clone)"){
						Eat(10,15,15); 
						Application.ExternalCall( "ComerLimon", "Comiste un Limon" );
					}
					else if(col.transform.name == "candy3(Clone)"){
						Eat(50,5,15); 
						Application.ExternalCall( "ComerDulce", "Comiste un Dulce" );
					}
					// Consume the food
					Destroy(col.gameObject);
					//Debug.Log("Come fruta");
				}
			}
		}

		public void die(){
			// Find all of the colliders on the gameobject and set them all to be triggers.
			Collider2D[] cols = GetComponents<Collider2D>();
			foreach(Collider2D c in cols)
			{
				c.isTrigger = true;
			}
			// Move all sprite parts of the player to the front
			SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer s in spr)
			{
				s.sortingLayerName = "UI";
			}
		
			// ... disable user Player Control script
			GetComponent<PlayerControl>().enabled = false;
			
			// ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
			GetComponentInChildren<Gun>().enabled = false;
			
			// ... Trigger the 'Die' animation state
			anim.SetTrigger("Die");
		}

		
		void Eat(int proteinAmount, int carbAmount, int fatAmount){
	
			// Increase the player's nutritions levels depending on food.
			 protein += proteinAmount;
			 carb += carbAmount;
			 fat += fatAmount;

			// Update what the bars looks like.
			UpdateBars();
			
			// Play a random clip of the player getting hurt.
			int i = Random.Range (0, tauntClips.Length);
			AudioSource.PlayClipAtPoint(tauntClips[i], transform.position);
		} 
		
		public void UpdateBars ()
		{
				if (protein > 0f && protein <= 100f &&
						carb > 0f && carb <= 100f &&
						fat > 0f && fat <= 100f) {
						// Set the health bar's colour to proportion of the way between blue and purple based on the player's protein level.
						proteinBar.material.color = Color.Lerp (Color.red, Color.yellow, 1 - protein * 0.01f);
						// Set the health bar's colour to proportion of the way between green and red based on the player's carb level.
						carbBar.material.color = Color.Lerp (Color.blue, Color.cyan, 1 - carb * 0.01f);
						// Set the health bar's colour to proportion of the way between green and red based on the player's fat level.
						fatBar.material.color = Color.Lerp (Color.green, Color.magenta, 1 - fat * 0.01f);
				
						// Set the scale of the health bar to be proportional to the player's protein level.
						proteinBar.transform.localScale = new Vector3 (proteinScale.x * protein * 0.01f, 3, 1);
						// Set the scale of the health bar to be proportional to the player's carb level.
						carbBar.transform.localScale = new Vector3 (carbScale.x * carb * 0.01f, 3, 1);
						// Set the scale of the health bar to be proportional to the player's fat level.
						fatBar.transform.localScale = new Vector3 (fatScale.x * fat * 0.01f, 3, 1);
				
						// Decrease nutritition levels over time
						protein -= 0.03f;
						carb -= 0.03f;
						fat -= 0.03f;
				} else if(protein <= 0f &&
		          			carb <= 0f &&
		          			fat <= 0f){
					if(alive){
						Application.ExternalCall( "MuerteHambre", "Te moriste por hambre" );
						die ();
						alive = false;
					}
				}
				else{
					if(alive){
						Application.ExternalCall( "MuerteSobredosis", "Te moriste por sobredosis" );
						die ();
						alive = false;
					}
				}
		}
		
	}
	