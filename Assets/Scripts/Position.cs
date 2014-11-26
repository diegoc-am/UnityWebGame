using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {
	public int height;
	public int xOffset;
	private Vector3 pos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		pos = new Vector3(xOffset, Screen.height - height, 10);
		transform.position = Camera.main.ScreenToWorldPoint(pos);
	}
}
