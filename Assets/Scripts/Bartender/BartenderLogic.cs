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
			//rb.isKinematic = false;


			MuscleController mc = carry.GetComponent<MuscleController>();
			if(mc != null){
				mc.stickToRoot = false;
				mc.consciousness = 0;
				mc.GetComponent<CustomerAI>().bodyNode.AddForce(Camera.main.transform.forward * 50, ForceMode.VelocityChange);
			}else{
				rb.AddForce(Camera.main.transform.forward * 25, ForceMode.VelocityChange);
			}
			return true;
		}
	}
}
