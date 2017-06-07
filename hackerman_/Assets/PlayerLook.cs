using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {
	public float maxAngle = 180;
	public float offset = -90.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float AngleRad = Mathf.Atan2 (Input.mousePosition.y - transform.position.y, Input.mousePosition.x - transform.position.x);
		//convert to degrees
		float AngleDeg = (180 / Mathf.PI) * AngleRad;
		transform.rotation = Quaternion.Euler(0,0,AngleDeg+offset);
	}
}
