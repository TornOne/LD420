using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartenderMovement : MonoBehaviour {
	public CharacterController controller;

	public float speed = 1;

	void Start() {
		
	}
	
	void Update() {
		Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		if (moveDirection.magnitude >= 0.5f) {
			controller.SimpleMove(moveDirection.normalized * speed);
		}
	}
}
