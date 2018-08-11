using UnityEngine;

public class Tray : MonoBehaviour {
	public bool isCarried = true;
	public int drinkLimit = 3;
	int drinkCount = 0;
	GameObject[] glasses;
	public GameObject player, beer, sizzurp;
	MeshCollider meshCollider;
	Rigidbody rb;

	void Start() {
		glasses = new GameObject[drinkLimit];
		meshCollider = GetComponent<MeshCollider>();
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		if (Input.GetButtonDown("RMB")) {
			Throw();
		}
	}

	public bool AddDrink(string drinkName) {
		GameObject drinkType;
		switch (drinkName) {
			case "Beer":
				drinkType = beer;
				break;
			default:
				drinkType = sizzurp;
				break;
		}

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
			GameObject drink = Instantiate(drinkType, transform.position + new Vector3(Mathf.Cos(drinkAngle) * (0.1f + 0.01f * drinkLimit), 0.1f, Mathf.Sin(drinkAngle) * (0.1f + 0.01f * drinkLimit)), Quaternion.identity, transform);
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

	public void SetHeight(float height) {
		transform.Translate(new Vector3(0.5f, 0.3f + height, 0.9f) - transform.localPosition);
	}

	bool Throw() {
		if (!isCarried) {
			return false;
		} else {
			isCarried = false;
			meshCollider.enabled = true;
			rb.isKinematic = false;
			rb.AddForce(Camera.main.transform.forward, ForceMode.VelocityChange);
			transform.parent = null;

			for (int i = 0; i < glasses.Length; i++) {
				if (glasses[i] != null) {
					glasses[i].GetComponent<BoxCollider>().enabled = true;
					Rigidbody glassRb = glasses[i].GetComponent<Rigidbody>();
					glassRb.isKinematic = false;
					glassRb.AddForce(Camera.main.transform.forward, ForceMode.VelocityChange);
					glasses[i].transform.parent = null;
					glasses[i] = null;
				}
			}

			return true;
		}
	}

	void OnMouseDown() {
		if (!isCarried && (player.transform.position - transform.position).magnitude <= 3) {
			meshCollider.enabled = false;
			rb.isKinematic = true;
			isCarried = true;
			transform.parent = player.transform;
			transform.SetPositionAndRotation(new Vector3(0.5f, 0.3f, 0.9f), Quaternion.identity);
		}
	}
}
