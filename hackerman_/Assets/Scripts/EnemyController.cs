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
		anim.SetBool("EnemyMoving",false);
		float patrolDist = Vector3.Distance(transform.position,currentPoint.position);
		if (patrolDist <= 0.1f) {
			changeCurrentPoint ();
			return;
		}
		bool chasing = checkSight (currentPoint.transform.position);
		anim.SetBool("EnemyMoving",true);
		if (!chasing) {
			patrol ();
		}
			
	}
	bool checkSight(Vector3 endPoint){
		Vector3 start = transform.position;
		Vector3 direction = (endPoint - transform.position).normalized;
		RaycastHit2D sightTest = Physics2D.Raycast(start,  direction, checkDistance);
		Debug.DrawRay(start,direction*checkDistance,Color.red, 5.0f);
		/*
		 * dieser Code macht eigentlich dass geguckt wie groß der Radius zwischen spieler und enemy ist
		 * und überpürft ob dieser Radius kleiner als ein gegebenr wert ist -> falls ja ist er im Sichtfeld
		 * nur leider ist der angle immer 90 was keinen Sinn ergibt, das liegt höchstwahrscheinlich daran, dass
		 * der transform.forward sich nicht ändert obwohl die matrix durch rotate verändert wird:(
		float dist = Vector3.Distance(transform.position,Player.transform.position);
		if (dist < Range) {
			Vector3 targetDir = Player.transform.position - transform.position;
			Vector3 forward = transform.forward;
			float angle = Vector3.Angle(targetDir, forward);
			if (angle < 5.0f) {
				//enemy sees player
			}
		}
*/
		if (sightTest.collider != null) 
		{
			if (sightTest.collider.gameObject.name == "Player") 
			{
				//if enemy sees player its gameover
				Player.GetComponent<GameOverController> ().gameOver ();
			}
		}
		return false;
	}

	void patrol(){
		Vector3 direction = (currentPoint.position - transform.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
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
	

		//das würde eigentlich machen, dass er noch ein blickcheck in die Blickrichtung macht
		// das funktioniert leider auch nicht weil transform forward nicht richtig ist :(
		//checkSight (transform.up);
		WaitTimer += Time.deltaTime;
		if (WaitTimer > MaxTimer) {
			currentPoint = patrolPoints[currentCount];
			WaitTimer = 0f;
		}


	}
}

