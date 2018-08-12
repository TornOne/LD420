using UnityEngine;

public class DrinkDesirer : MonoBehaviour {
	public CustomerAI AI;
	BartenderLogic player;
	Tray tray;
	string drinkType = "";
	public string[] drinkTypes;

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
			AI.transform.localPosition = new Vector3(0, 0, 3);
		} else if (AI.state == CustomerAI.State.waiting && (tray.transform.position - transform.position).magnitude <= 2 && tray.RemoveDrink(drinkType)) {
			drinkType = "";
			AI.DrinkCount--;
		}
	}

	public void DesireDrink() {
		drinkType = drinkTypes[Random.Range(0, drinkTypes.Length)];
	}
}
