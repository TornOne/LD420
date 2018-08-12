using UnityEngine;

public class CollisionPropagator : MonoBehaviour {
	public CustomerAI parent;

	void OnCollisionEnter(Collision collision) {
		parent.HandleCollision(collision);
	}
}
