using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour {
	List<GameObject> glasses = new List<GameObject>(3);
	public GameObject beer;

	public bool AddDrink(string drinkName) {
		GameObject drinkType = beer;

		if (glasses.Count == 3) {
			return false;
		} else {
			float drinkAngle = glasses.Count * Mathf.Deg2Rad * 120;
			GameObject drink = Instantiate(drinkType, transform.position + new Vector3(Mathf.Cos(drinkAngle) * 0.1f, 0.1f, Mathf.Sin(drinkAngle) * 0.1f), Quaternion.identity, transform);
			drink.name = drinkName;
			glasses.Add(drink);
			return true;
		}
	}

	public bool RemoveDrink(string drinkName) {
		GameObject drinkType = beer;

		foreach (GameObject glass in glasses) {
			if (glass.name == drinkName) {
				glasses.Remove(glass);
				Destroy(glass);
				return true;
			}
		}

		return false;
	}
}
