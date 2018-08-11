using UnityEngine;

public class Tray : MonoBehaviour {
	public int drinkLimit = 3;
	int drinkCount = 0;
	GameObject[] glasses;
	public GameObject beer;

	void Start() {
		glasses = new GameObject[drinkLimit];
	}

	public bool AddDrink(string drinkName) {
		GameObject drinkType = beer;

		if (drinkCount == drinkLimit) {
			return false;
		} else {
			int emptySlot = 0;
			for (; emptySlot < drinkLimit; emptySlot++) {
				if (glasses[emptySlot] == null) {
					break;
				}
			}
			float drinkAngle = emptySlot * 2 * Mathf.PI / drinkLimit;
			GameObject drink = Instantiate(drinkType, transform.position + new Vector3(Mathf.Cos(drinkAngle) * 0.02f * drinkLimit, 0.1f, Mathf.Sin(drinkAngle) * 0.02f * drinkLimit), Quaternion.identity, transform);
			drink.name = drinkName;
			glasses[emptySlot] = drink;
			drinkCount++;
			return true;
		}
	}

	public bool RemoveDrink(string drinkName) {
		GameObject drinkType = beer;

		for (int i = 0; i < drinkLimit; i++) {
			if (glasses[i] != null && glasses[i].name == drinkName) {
				Destroy(glasses[i]);
				glasses[i] = null;
				drinkCount--;
				return true;
			}
		}

		return false;
	}
}
