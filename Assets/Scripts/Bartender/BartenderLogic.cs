using UnityEngine;

public class BartenderLogic : MonoBehaviour {
	public bool isCarrying = false;
	public GameObject carry;

	void Update() {
		if (Input.GetButtonDown("RMB")) {
			Throw();
		}
	}

	bool Throw() {
		if (!isCarrying) {
			return false;
		} else {
			isCarrying = false;
			carry.transform.parent = null;
			Rigidbody rb = carry.GetComponentInChildren<Rigidbody>();
			rb.isKinematic = false;
			rb.AddForce(Camera.main.transform.forward * 10, ForceMode.VelocityChange);

			return true;
		}
	}
}
