using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		GameObject g = other.gameObject;
		Debug.Log (g.name);
		if (g.CompareTag("Player")) {
			Destroy (gameObject);
		}
	}
}
