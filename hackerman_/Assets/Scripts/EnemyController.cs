using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private Animator anim;
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
	public float checkDistance = 10f;
	float CurrentSpeed = 0f;
	float WaitTimer = 0f;
	float MaxTimer = 2f;

	Transform[] patrolPoints;
	Transform currentPoint;
	int currentCount = 0;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
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
		//Debug.Log ("bool",anim.GetBool ("EnemyMoving"));
		anim.SetBool("EnemyMoving",false);
		float patrolDist = Vector3.Distance(transform.position,currentPoint.position);
		if (patrolDist <= 0.1f) {
			changeCurrentPoint ();
			return;
		}
		checkSight (currentPoint);
		anim.SetBool("EnemyMoving",true);
		float dist = Vector3.Distance(transform.position,Player.transform.position);
		if (dist <= Range && dist > GameOverRange) {
			//Vector3 direction = (Player.transform.position - transform.position).normalized;
			//float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
			//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			//rb.MovePosition (transform.position + direction * (ChaseSpeed + CurrentSpeed) * Time.deltaTime);
			//CurrentSpeed += 0.01f;
			patrol();
		} else if (dist < GameOverRange) {
			Player.GetComponent<GameOverController> ().gameOver ();
		} else {
			if (CurrentSpeed > 0f) {
				CurrentSpeed -= 0.02f;
			}

			patrol();
		}	
	}
	void checkSight(Transform endPoint){
		Vector3 start = transform.position;
		//Vector3 direction = (Player.transform.position - transform.position).normalized;
		Vector3 direction = (endPoint.position - transform.position).normalized;


		//do the ray test
		RaycastHit2D sightTest = Physics2D.Raycast(start,  direction, checkDistance);
		Debug.DrawRay(start,direction*checkDistance,Color.red, 5.0f);
		if (sightTest.collider != null) 
		{
			Debug.Log (sightTest.collider.gameObject.name);
			if (sightTest.collider.gameObject.name == "Player") 
			{
				Player.GetComponent<GameOverController> ().gameOver ();
				Debug.Log ("Rigidbody collider is: " + sightTest.collider);
			}
		}
	}

	void patrol(){
		//float patrolDist = Vector3.Distance(transform.position,currentPoint.position);
		//if (patrolDist <= 0.1f) {
		//	changeCurrentPoint ();
		//}
		Vector3 direction = (currentPoint.position - transform.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		rb.MovePosition (transform.position + direction * PatrolSpeed * Time.deltaTime);
	}

	void changeCurrentPoint(){
		anim.SetBool("EnemyMoving",false);

		currentCount++;
		if (currentCount == patrolPoint.transform.childCount) {
			currentCount = 0;
		}

		Vector3 direction = (currentPoint.position - transform.position).normalized;
		float angle = 0f;
		if (WaitTimer > 0.5f && WaitTimer < 0.9f) {
			angle = Mathf.Atan2 (-direction.y, -direction.x) * Mathf.Rad2Deg - 45;
		} else if (WaitTimer > 0.9f && WaitTimer < 1.4f) {
			angle = Mathf.Atan2 (-direction.y, -direction.x) * Mathf.Rad2Deg - 120;
		}else if (WaitTimer > 1.4f && WaitTimer < 1.7f) {
			angle = Mathf.Atan2 (-direction.y, -direction.x) * Mathf.Rad2Deg - 175;
		}else if (WaitTimer > 1.7f && WaitTimer < 1.9f) {
			angle = Mathf.Atan2 (-direction.y, -direction.x) * Mathf.Rad2Deg - 210;
		}
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

		WaitTimer += Time.deltaTime;
		if (WaitTimer > MaxTimer) {
			

			currentPoint = patrolPoints[currentCount];
			WaitTimer = 0f;
		}


	}
}

