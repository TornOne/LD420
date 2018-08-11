using UnityEngine;

public class CameraController : MonoBehaviour {
	public float rotationSpeed = 1;
	public Tray tray;

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	void OnApplicationFocus(bool focus) {
		Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;
	}

	void Update () {
		float currentRotation = transform.localEulerAngles.x;
		//Translate current rotation to a "normal" system
		if (currentRotation <= 90) {
			currentRotation = 90 - currentRotation;
		} else {
			currentRotation = 450 - currentRotation;
		}

		//0 to 180, bottom to top
		float deltaRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
		float targetRotation = currentRotation + deltaRotation;
		//Constrain the delta so the target is between 30 and 150
		if (targetRotation < 30) {
			deltaRotation = 30 - currentRotation;
		} else if (targetRotation > 150) {
			deltaRotation = 150 - currentRotation;
		}

		transform.Rotate(-deltaRotation, 0, 0);
		if (tray.isCarried) {
			tray.SetHeight((targetRotation - 90) / (targetRotation < 90 ? 100 : 50));
		}
	}
}
