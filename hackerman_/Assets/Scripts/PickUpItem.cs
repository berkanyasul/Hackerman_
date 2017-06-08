using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickUpItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameOver = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameOverController> ();	
	}

	GameOverController gameOver;

	// Update is called once per frame
	void Update () {
	
	
	}

	void OnTriggerEnter2D(Collider2D other){

		GameObject g = other.gameObject;
		Debug.Log (g);
		if (g.CompareTag("Player")) {
			gameOver.pickedUpItem ();
			Destroy (gameObject);
		}
	}
}
