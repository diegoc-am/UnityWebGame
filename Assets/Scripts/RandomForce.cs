using UnityEngine;
using System.Collections;

public class RandomForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		rigidbody2D.AddForce (new Vector2 (Random.Range(-1000f,1000f),0f)); 
		StartCoroutine (autoDestroy());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator autoDestroy(){
		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}
}
