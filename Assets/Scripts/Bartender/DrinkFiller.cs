using UnityEngine;

public class DrinkFiller : MonoBehaviour {
	public Tray tray;
	public string drinkType;

	void OnMouseDown() {
		if ((tray.transform.position - transform.position).magnitude <= 1 && tray.isCarried) {
			tray.AddDrink(drinkType);
		}
	}
}
