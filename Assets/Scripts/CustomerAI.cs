using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CustomerAI : MonoBehaviour {
	public enum State {
		moving,
		waiting,
		fighting,
		struggling,
		dead
	}
	State status = State.moving;
	public State state {
		get {
			return status;
		}

		set {
			if (seat.isOccupied && (value != State.moving || value != State.waiting)) {
				seat.isOccupied = false;
			}

			if (value == State.fighting) {
				anim.SetBool("Fighting", true);
				GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = aggressiveCustomerMesh;
			}

			status = value;
		}
	}

	public Transform seatsNode;
	Transform seatLoc;
	Seat seat;
	NavMeshAgent navAgent;
	Animator anim;

	public Mesh aggressiveCustomerMesh;
	public DrinkDesirer drinkDesirer;
	public List<GameObject> colliders;

	public float footSpeed = 3, footAmplitude = 0.5f;

	int agressionLevel = 0;
	public int agressionCap = 1;

	public int drinkCount = 3;
	public int DrinkCount {
		get {
			return drinkCount;
		}

		set {
			drinkCount = value;
			if (value <= 0) {
				LeaveBar();
			} else {
				drinkDesirer.DesireDrink();
			}
		}
	}

	void Start() {
		navAgent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		//Enter bar
		drinkCount = Random.Range(1, drinkCount + 1);
		PickRandomSeat();
		StartCoroutine(MoveTo(seatLoc.position, "sit"));
	}

	void PickRandomSeat() {
		int seatIndex = Random.Range(0, seatsNode.childCount);

		for (int i = 0; i < seatsNode.childCount; i++) {
			seatLoc = seatsNode.GetChild((seatIndex + i) % seatsNode.childCount);
			seat = seatLoc.GetComponent<Seat>();
			if (!seat.isOccupied) {
				break;
			}
		}

		seat.isOccupied = true;
	}

	public void HandleCollision(Collision collision) {
		//Ignore collisions with self
		if (colliders.Contains(collision.gameObject)) {
			return;
		}

		Debug.Log(name + " collided with " + collision.gameObject.name);
		string otherTag = collision.gameObject.tag;
		if (otherTag == "Customer" || otherTag == "Player") {
			agressionLevel++;
			if (agressionLevel == agressionCap) {
				Debug.Log("Starting to punch");
				state = State.fighting;
			}
		}
	}

	void LeaveBar() {
		seat.isOccupied = false;
		StartCoroutine(MoveTo(new Vector3(0, 0, -15), "leave"));
	}

	IEnumerator MoveTo(Vector3 position, string finishAction = "") {
		anim.SetFloat("WalkSpeed", GetComponent<MuscleController>().consciousness);
		navAgent.enabled = true;
		navAgent.SetDestination(position);
		state = State.moving;

		while (state == State.moving && (navAgent.pathPending || navAgent.remainingDistance > 0.2f)) {
			yield return null;
		}

		anim.SetFloat("WalkSpeed", 0);
		navAgent.ResetPath();
		navAgent.enabled = false;

		//Only do finish action if you reached the destination
		if (state != State.moving) {
			yield break;
		}

		switch (finishAction) {
			case "sit":
				state = State.waiting;
				StartCoroutine(FaceTable());
				break;
			case "leave":
				Destroy(gameObject);
				break;

		}
	}

	IEnumerator FaceTable() {
		float startTime = Time.time;
		while (Time.time - startTime < 1) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, seatLoc.rotation, Time.deltaTime * navAgent.angularSpeed);
			yield return null;
		}
		DrinkCount = drinkCount;
	}
}
