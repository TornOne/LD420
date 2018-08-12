using UnityEngine;

public class Repulsor : MonoBehaviour {
	public float punchStrength = 10f;

	void OnTriggerEnter(Collider other) {
		Rigidbody rb = other.attachedRigidbody;
		if (rb != null) {
			CollisionPropagator prop = rb.GetComponent<CollisionPropagator>();
			if (prop != null) {
				StartCoroutine(prop.parent.Ragdollify(3));
			}

			rb.AddForceAtPosition(Vector3.left * punchStrength, other.ClosestPoint(transform.position), ForceMode.VelocityChange);
		}
	}
}
