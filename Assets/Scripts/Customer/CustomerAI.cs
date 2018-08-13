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
		dead,
		piano,
		fleeing
	}
	State status = State.moving;
	public State state {
		get {
			return status;
		} set {
			if(value == State.piano){
				anim.SetBool("Sitting", true);
				anim.SetBool("Piano", true);
			}

			if ((value != State.moving || value != State.waiting || value != State.piano) && seat != null && seat.isOccupied) {
				seat.isOccupied = false;
			}

			if (value == State.fighting) {
				anim.SetBool("Fighting", true);
				//EnableFists(true);
				GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = aggressiveCustomerMesh;
			} else {
				anim.SetBool("Fighting", false);
				//EnableFists(false);
			}

			if(value == State.struggling){
				GetComponent<MuscleController>().stickToRoot = true;
				navAgent.enabled = false;
			}

			if(value == State.dead){
				GetComponent<MuscleController>().stickToRoot = true;
				GetComponent<MuscleController>().consciousness = 0;
			}

			if(value == State.fleeing){
				anim.SetBool("Sitting", false);
				anim.SetBool("Piano", false);
			}

			status = value;
		}
	}

	Transform seatsNode;
	Transform seatLoc;
	Seat seat;
	NavMeshAgent navAgent;
	Animator anim;

	public Mesh aggressiveCustomerMesh;
	public DrinkDesirer drinkDesirer;
	public List<GameObject> colliders;
	public Rigidbody rootNode, bodyNode;
	public Collider fist1, fist2;
	public State startState = State.moving;

	public float footSpeed = 3, footAmplitude = 0.5f;
	public float attackStoppingDistance = 1.0f;

	public float aggressionImmunityDuration = 1f;
	private bool isAggressionImmune = false;
	int agressionLevel = 0;
	public int AggressionLevel{
		get{
			return agressionLevel;
		}
		set{
			if(!isAggressionImmune){
				agressionLevel = value;
				StartCoroutine(AggressionImmunityCoroutine());
			}
		}
	}
	public int agressionCap = 3;

	public int drinkCount = 3;
	public int DrinkCount {
		get {
			return drinkCount;
		} set {
			drinkCount = value;
			if (value <= 0) {
				LeaveBar();
			} else {
				drinkDesirer.DesireDrink();
			}
		}
	}

	public float hp = 5;
	public float Hp {
		get {
			return hp;
		} set {
			if (value <= 0) {
				state = State.dead;
			}
			hp = value;
		}
	}

	void Start() {
		seatsNode = GameObject.FindGameObjectWithTag("SeatNode").transform;
		navAgent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		//Enter bar
		if(startState == State.moving){
			drinkCount = Random.Range(1, drinkCount + 1);
			PickRandomSeat();
			StartCoroutine(MoveTo(seatLoc.position, "sit"));
		}else{
			state = startState;
		}
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

	public IEnumerator Ragdollify(float time) {
		rootNode.isKinematic = false;
		yield return new WaitForSeconds(time);
		if (state != State.dead && state != State.struggling) {
			rootNode.isKinematic = true;
			StartCoroutine(GetUpCoroutine());
		}
	}

	public void HandleCollision(Collision collision) {
		//Ignore collisions with self
		if (colliders.Contains(collision.gameObject)) {
			return;
		}

		string otherTag = collision.gameObject.tag;
		if (otherTag == "Customer" || otherTag == "Player") {
			AggressionLevel++;
			if (AggressionLevel >= agressionCap && state != State.struggling) {
				if(state == State.piano){
					state = State.fleeing;
				}else{
					Debug.Log("Starting to punch");
					state = State.fighting;
					StartCoroutine(ChooseAggro());
				}
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

	IEnumerator ChooseAggro() {
		navAgent.stoppingDistance = attackStoppingDistance;
		while (state != State.dead && state != State.struggling) {
			navAgent.enabled = true;
			CustomerAI[] customers = FindObjectsOfType<CustomerAI>();

			if (customers.Length == 1) {
				navAgent.SetDestination(transform.position);
			} else {
				CustomerAI closestCustomer = customers[0];
				float closestDistance = (closestCustomer.transform.position - transform.position).sqrMagnitude;
				foreach (CustomerAI customer in customers) {
					if (customer == this) {
						continue;
					}

					float customerDistance = (customer.transform.position - transform.position).sqrMagnitude;
					if (customerDistance < closestDistance && customer.state != State.dead) {
						closestDistance = customerDistance;
						closestCustomer = customer;
					}
				}

				navAgent.SetDestination(closestCustomer.transform.position);
			}


			yield return new WaitForSeconds(5);
		}

		navAgent.enabled = false;
	}

	IEnumerator GetUpCoroutine(){
		float distance, angle;
		do{
			distance = Vector3.Distance(rootNode.transform.localPosition, new Vector3(0, 0, 1));
			angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(-90, -90, 0));

			rootNode.transform.localPosition = Vector3.MoveTowards(rootNode.transform.localPosition, new Vector3(0, 0, 1), Time.deltaTime * distance);
			rootNode.transform.localRotation = Quaternion.RotateTowards(rootNode.transform.localRotation, Quaternion.Euler(-90, -90, 0), Time.deltaTime * angle);
			yield return null;
		}while(distance > 0.05f && angle > 0.5f);
	}

	IEnumerator AggressionImmunityCoroutine(){
		isAggressionImmune = true;
		yield return new WaitForSeconds(aggressionImmunityDuration);
		isAggressionImmune = false;
	}
}
