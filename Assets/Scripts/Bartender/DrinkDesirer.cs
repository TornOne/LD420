using UnityEngine;
using System.Collections;

public class DrinkDesirer : MonoBehaviour {
	public CustomerAI AI;
	BartenderLogic player;
	Tray tray;
	string drinkType = "";
	public string[] drinkTypes;
	private bool isDrinking = false;
	public GameObject drinkPrefab;
	public Transform hand;
	public float drinkingTime = 10f;
	public float drinkAlcoholContent = 0.2f;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<BartenderLogic>();
		tray = GameObject.FindGameObjectWithTag("Tray").GetComponent<Tray>();
	}

	void OnMouseDown() {
		if (!tray.isCarried && !player.isCarrying) {
			player.isCarrying = true;
			player.carry = AI.gameObject;
			AI.state = CustomerAI.State.struggling;
			AI.transform.parent = Camera.main.transform;
			AI.rootNode.transform.localPosition = new Vector3(0, 0, 2);
		} else if (AI.state == CustomerAI.State.waiting && (tray.transform.position - transform.position).magnitude <= 2 && tray.RemoveDrink(drinkType) && !isDrinking) {
			drinkType = "";
			StartCoroutine(DrinkingCoroutine());
		}
	}

	public void DesireDrink() {
		drinkType = drinkTypes[Random.Range(0, drinkTypes.Length)];
	}

	IEnumerator DrinkingCoroutine(){
		isDrinking = true;
		GameObject drink = Instantiate(drinkPrefab, hand.position, hand.rotation);
		drink.GetComponent<Collider>().enabled = false;
		drink.GetComponent<Rigidbody>().isKinematic = true;
		drink.transform.parent = hand;

		float drinkLeft = 1f;
		while(drinkLeft > 0){
			yield return null;
			drinkLeft -= Time.deltaTime / drinkingTime;
			drink.GetComponent<Whiskey>().filled = drinkLeft;
		}

		drink.transform.parent = null;
		drink.GetComponent<Rigidbody>().isKinematic = false;
		drink.GetComponent<Collider>().enabled = true;

		AI.DrinkCount--;
		AI.GetComponent<MuscleController>().consciousness -= drinkAlcoholContent;
		isDrinking = false;
	}
}
