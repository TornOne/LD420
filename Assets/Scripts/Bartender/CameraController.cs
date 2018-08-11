using UnityEngine;

public class CameraController : MonoBehaviour {
	public float rotationSpeed = 1;

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	void OnApplicationFocus(bool focus) {
		Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;
	}

	void Update () {
		transform.Rotate(-Input.GetAxis("Mouse Y") * rotationSpeed, 0, 0);
	}
}
