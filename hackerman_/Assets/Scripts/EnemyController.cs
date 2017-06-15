using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private GameObject Player;
	private Rigidbody2D rb;
	public GameObject patrolPoints;
	//public Transform Player;

	// how fast we want the enemy to chase
	public float ChaseSpeed = 5f;

	// the range at which it detects Player
	public float Range = 5f;
	public float GameOverRange = 1f;
	// what our current speed is (get only)
	float CurrentSpeed;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		patrolPoints = GameObject.FindGameObjectWithTag ("PatrolPoints");
		Debug.Log ("___");
		Debug.Log (patrolPoints.ToString());
		rb = GetComponent<Rigidbody2D>();		
	}
	
	// Update is called once per frame
	void Update () {
			
	}

	void FixedUpdate(){
		float dist = Vector3.Distance(transform.position,Player.transform.position);
		if (dist <= Range && dist > GameOverRange) {
			Vector3 direction = (Player.transform.position - transform.position).normalized;
			rb.MovePosition (transform.position + direction * ChaseSpeed * Time.deltaTime);
			//transform.position = transform.position + direction * ChaseSpeed * Time.deltaTime ;
			//rb.AddForce(test, ForceMode2D.Force);
		} else if (dist < GameOverRange) {
			Player.GetComponent<GameOverController> ().gameOver ();
		}	
	}
}

