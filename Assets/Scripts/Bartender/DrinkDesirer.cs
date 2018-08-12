using UnityEngine;

public class DrinkDesirer : MonoBehaviour {
	public CustomerAI AI;
	Tray tray;
	string drinkType = "";
	public string[] drinkTypes;

	void Start() {
		tray = GameObject.FindGameObjectWithTag("Tray").GetComponent<Tray>();
	}

	void OnMouseDown() {
		if (AI.state == CustomerAI.State.waiting && (tray.transform.position - transform.position).magnitude <= 2 && tray.RemoveDrink(drinkType)) {
			drinkType = "";
			AI.DrinkCount--;
		}
	}

	public void DesireDrink() {
		drinkType = drinkTypes[Random.Range(0, drinkTypes.Length)];
	}
}
