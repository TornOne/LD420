using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class CustomerAI : MonoBehaviour {

	public Transform seatsNode;
	public Transform leftLegIK, rightLegIK;
	public float footSpeed = 3, footAmplitude = 0.5f, rotationSpeed = 10f;
	public int DrinkCount = 0;

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
		if(navAgent.remainingDistance > 0.2f){
			leftLegIK.transform.localPosition = new Vector3(0, -1, Mathf.Sin(Time.time * footSpeed) * footAmplitude);
			rightLegIK.transform.localPosition = new Vector3(0, -1, -Mathf.Sin(Time.time * footSpeed) * footAmplitude);
		}else{
			leftLegIK.transform.localPosition = new Vector3(0, -1, 0);
			rightLegIK.transform.localPosition = new Vector3(0, -1, 0);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, seat.rotation, Time.deltaTime * rotationSpeed);
		}
	}
}
