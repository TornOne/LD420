using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CustomerAI : MonoBehaviour {
	enum State {
		moving,
		waiting,
		fighting,
		struggling,
		dead
	}
	State state;

	public Transform seatsNode;
	Transform seat;
	NavMeshAgent navAgent;

	public DrinkDesirer drinkDesirer;

	public Transform leftLegIK, rightLegIK;
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

	void PickRandomSeat() {
		int seatIndex = Random.Range(0, seatsNode.childCount);

		for (int i = 0; i < seatsNode.childCount; i++) {
			seat = seatsNode.GetChild((seatIndex + i) % seatsNode.childCount);
			if (!seat.GetComponent<Seat>().isOccupied) {
				break;
			}
		}

		seat.GetComponent<Seat>().isOccupied = true;
	}

	void Start() {
		//Enter bar
		navAgent = GetComponent<NavMeshAgent>();
		drinkCount = Random.Range(1, drinkCount + 1);
		PickRandomSeat();
		StartCoroutine(MoveTo(seat.position, "sit"));
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log("Collided with " + collision.gameObject.name);
		string otherTag = collision.gameObject.tag;
		if (otherTag == "Customer" || otherTag == "Player") {
			agressionLevel++;
			if (agressionLevel == agressionCap) {
				Debug.Log("Starting to punch");
				//TODO: Start fighting
			}
		}
	}

	void LeaveBar() {
		StartCoroutine(MoveTo(new Vector3(0, 0, -15), "leave"));
	}

	IEnumerator MoveTo(Vector3 position, string finishAction = "") {
		navAgent.SetDestination(position);
		state = State.moving;

		while (state == State.moving && (navAgent.pathPending || navAgent.remainingDistance > 0.2f)) {
			leftLegIK.transform.localPosition = new Vector3(0, -1, Mathf.Sin(Time.time * footSpeed) * footAmplitude);
			rightLegIK.transform.localPosition = new Vector3(0, -1, -Mathf.Sin(Time.time * footSpeed) * footAmplitude);
			yield return null;
		}

		navAgent.ResetPath();
		leftLegIK.transform.localPosition = new Vector3(0, -1, 0);
		rightLegIK.transform.localPosition = new Vector3(0, -1, 0);

		//Only do finish action if you reached the destination
		if (state != State.moving) {
			yield break;
		}

		switch (finishAction) {
			case "sit":
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
			transform.rotation = Quaternion.RotateTowards(transform.rotation, seat.rotation, Time.deltaTime * navAgent.angularSpeed);
			yield return null;
		}
		state = State.waiting;
		DrinkCount = drinkCount;
	}
}
