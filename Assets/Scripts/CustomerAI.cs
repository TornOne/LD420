using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class CustomerAI : MonoBehaviour {

	public Transform seatsNode;

	private Transform seat;
	private UnityEngine.AI.NavMeshAgent navAgent;

	void PickRandomSeat(){
		int seatIndex = (int) Random.Range(0, seatsNode.childCount);
		for(int i = 0; i < seatsNode.childCount; i++){
			seat = seatsNode.GetChild(seatIndex + i % seatsNode.childCount);
			if(!seat.GetComponent<Seat>().isOccupied) break;
		}
		seat.GetComponent<Seat>().isOccupied = true;
	}

	// Use this for initialization
	void Start () {
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		PickRandomSeat();
		navAgent.SetDestination(seat.position);
	}

	// Update is called once per frame
	void Update () {

	}
}
