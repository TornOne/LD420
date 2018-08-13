using UnityEngine;
using System.Collections;

public class Repulsor : MonoBehaviour {
	public float punchStrength = 10f;
	public float cooldown = 0.5f;
	public AudioClip punchSfx;
	private bool onCooldown = false;

	void OnTriggerEnter(Collider other) {
		if(!onCooldown){
			Rigidbody rb = other.attachedRigidbody;
			if (rb != null) {
				CollisionPropagator prop = rb.GetComponent<CollisionPropagator>();
				if (prop != null) {
					prop.parent.Hp--;
					StartCoroutine(prop.parent.Ragdollify(3));
				}

				rb.AddForceAtPosition(Vector3.left * punchStrength, other.ClosestPoint(transform.position), ForceMode.VelocityChange);
				AudioSource.PlayClipAtPoint(punchSfx, transform.position);
				StartCoroutine(CooldownCoroutine());
			}
		}
	}

	IEnumerator CooldownCoroutine(){
		onCooldown = true;
		yield return new WaitForSeconds(cooldown);
		onCooldown = false;
	}
}
