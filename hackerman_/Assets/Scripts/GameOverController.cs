using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {
	public int needToPickUp = 1;
	public int pickedUp = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void gameOver(){
		SceneManager.LoadScene(2);	
	}

	public void pickedUpItem(){
		pickedUp++;
		if(pickedUp == needToPickUp){
			gameOver ();
		}
	}
}
