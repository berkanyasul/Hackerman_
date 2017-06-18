using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private GameObject Player;
	private Rigidbody2D rb;
	public GameObject patrolPoint;
	//public Transform Player;

	// how fast we want the enemy to chase
	public float ChaseSpeed = 5f;
	public float PatrolSpeed = 3f;
	// the range at which it detects Player
	public float Range = 5f;
	public float GameOverRange = 1f;
	// what our current speed is (get only)
	float CurrentSpeed;
	float WaitTimer = 0f;
	float MaxTimer = 2f;

	Transform[] patrolPoints;
	Transform currentPoint;
	int currentCount = 0;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		patrolPoint = GameObject.FindGameObjectWithTag ("PatrolPoints");
		patrolPoints = new Transform[patrolPoint.transform.childCount];

		var i = 0;
		foreach(Transform child in patrolPoint.transform)
		{
			patrolPoints[i] = (child);
			i++;
		}
		currentPoint = patrolPoints[currentCount];
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
		} else {
			patrol();
		}	
	}

	void patrol(){
		float patrolDist = Vector3.Distance(transform.position,currentPoint.position);
		if (patrolDist <= 0.1f) {
			changeCurrentPoint ();
		}
		Vector3 direction = (currentPoint.position - transform.position).normalized;
		rb.MovePosition (transform.position + direction * PatrolSpeed * Time.deltaTime);
	}

	void changeCurrentPoint(){
		WaitTimer += Time.deltaTime;
		if (WaitTimer > MaxTimer) {
			currentCount++;
			if (currentCount == patrolPoint.transform.childCount) {
				currentCount = 0;
			}
			currentPoint = patrolPoints[currentCount];
			WaitTimer = 0f;
		}


	}
}

