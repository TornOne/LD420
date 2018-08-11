using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class CustomerAI : MonoBehaviour {

	public Transform seatsNode;

	private Transform seat;
	private UnityEngine.AI.NavMeshAgent navAgent;

	void PickRandomSeat(){
		int seatIndex = Random.Range(0, seatsNode.childCount);
		for (int i = 0; i < seatsNode.childCount; i++) {
			seat = seatsNode.GetChild(seatIndex + i % seatsNode.childCount);
			if (!seat.GetComponent<Seat>().isOccupied) break;
		}
		seat.GetComponent<Seat>().isOccupied = true;
	}

	void Start() {
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		PickRandomSeat();
		navAgent.SetDestination(seat.position);
	}

	void Update () {

	}
}
