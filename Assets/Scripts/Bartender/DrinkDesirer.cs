using UnityEngine;

public class DrinkDesirer : MonoBehaviour {
	Tray tray;
	public string drinkType;

	void Start() {
		tray = GameObject.FindGameObjectWithTag("Tray").GetComponent<Tray>();
	}

	void OnMouseDown() {
		if ((tray.transform.position - transform.position).magnitude <= 2) {
			tray.RemoveDrink(drinkType);
		}
	}
}
