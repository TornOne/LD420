using UnityEngine;

public class BartenderMovement : MonoBehaviour {
	public CharacterController controller;

	public float speed = 1;
	public float rotationSpeed = 1;
	
	void Update() {
		Vector3 moveDirection = transform.TransformDirection(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		if (moveDirection.magnitude >= 0.5f) {
			controller.SimpleMove(moveDirection.normalized * speed);
		} else {
			controller.SimpleMove(new Vector3());
		}
		transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0, Space.World);
	}
}
