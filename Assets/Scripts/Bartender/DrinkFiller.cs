using UnityEngine;

public class DrinkFiller : MonoBehaviour {
	public GameObject UI;
	public Tray tray;
	public string drinkType;
	public float fillTime = 1;

	void OnMouseEnter() {
		UI.SetActive(true);
	}

	void OnMouseExit() {
		UI.SetActive(false);
	}

	private void OnMouseUp() {
		
	}

	void OnMouseDown() {
		if ((tray.transform.position - transform.position).magnitude <= 1.5f && tray.isCarried) {
			tray.AddDrink(drinkType);
		}
	}
}
