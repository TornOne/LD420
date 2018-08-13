using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FixedJoint))]
public class Repairable : MonoBehaviour {

	public AudioClip breakingSound;
	public bool isBroken = false;
	public float repairDistance = 5f;

	private GameObject player;
	private Vector3 startingPosition;
	private Quaternion startingRotation;
	private float jointBreakForce;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		startingPosition = transform.position;
		startingRotation = transform.rotation;
		jointBreakForce = GetComponent<FixedJoint>().breakForce;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnJointBreak(float force){
		AudioSource.PlayClipAtPoint(breakingSound, transform.position);
		isBroken = true;
	}

	void OnMouseDown() {
		if (Vector3.Distance(player.transform.position, transform.position) > 3f || !isBroken) return;

		GetComponent<Rigidbody>().isKinematic = true;
		transform.position = startingPosition;
		transform.rotation = startingRotation;
		GetComponent<Rigidbody>().isKinematic = false;
		FixedJoint newJoint = gameObject.AddComponent<FixedJoint>();
		newJoint.breakForce = jointBreakForce;
		isBroken = false;
	}

	void OnMouseOver(){
		if(Vector3.Distance(player.transform.position, transform.position) > 3f){
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ClearCrosshair();
		}else if(isBroken){
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetRepairCrosshair();
		}
	}

	void OnMouseExit(){
		GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ClearCrosshair();
	}
}
