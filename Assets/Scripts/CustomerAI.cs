using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CustomerAI : MonoBehaviour {
	enum State {
		moving,
		waiting,
		fighting
	}

	public Transform seatsNode;
	Transform seat;
	NavMeshAgent navAgent;

	public Transform leftLegIK, rightLegIK, leftArmIK, rightArmIK;
	public float footSpeed = 3, footAmplitude = 0.5f, rotationSpeed = 10f;

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
		PickRandomSeat();
		navAgent.SetDestination(seat.position);
		drinkCount = Random.Range(1, drinkCount + 1);
	}

	void Update () {
		if(navAgent.remainingDistance > 0.2f){
			leftLegIK.transform.localPosition = new Vector3(0, -1, Mathf.Sin(Time.time * footSpeed) * footAmplitude);
			rightLegIK.transform.localPosition = new Vector3(0, -1, -Mathf.Sin(Time.time * footSpeed) * footAmplitude);
		}else{
			leftLegIK.transform.localPosition = new Vector3(0, -1, 0);
			rightLegIK.transform.localPosition = new Vector3(0, -1, 0);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, seat.rotation, Time.deltaTime * rotationSpeed);
		}

		// TODO dependent on state and distance to target
		//if(true){

		//}
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
		//TODO
	}
}
