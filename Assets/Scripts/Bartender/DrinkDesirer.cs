﻿using UnityEngine;

public class DrinkDesirer : MonoBehaviour {
	public CustomerAI AI;
	Tray tray;
	string drinkType = "";
	public string[] drinkTypes;

	void Start() {
		tray = GameObject.FindGameObjectWithTag("Tray").GetComponent<Tray>();
	}

	void OnMouseDown() {
		if ((tray.transform.position - transform.position).magnitude <= 1 && tray.RemoveDrink(drinkType)) {
			AI.DrinkCount--;
			drinkType = "";
		}
	}

	public void DesireDrink() {
		drinkType = drinkTypes[Random.Range(0, drinkTypes.Length)];
	}
}
