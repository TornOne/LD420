﻿using UnityEngine;

public class Tray : MonoBehaviour {
	public bool isCarried = true;
	public int drinkLimit = 3;
	int drinkCount = 0;
	public GameObject[] glasses;
	public GameObject player, beer, sizzurp;
	MeshCollider meshCollider;
	Rigidbody rb;

	public bool IsFull{
		get{
			int emptySlot = 0;
			for (; emptySlot < drinkLimit; emptySlot++) {
				if (glasses[emptySlot] == null) {
					return false;
				}
			}
			return true;
		}
	}

	void Start() {
		glasses = new GameObject[drinkLimit];
		meshCollider = GetComponentInChildren<MeshCollider>();
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
			GameObject drink = Instantiate(drinkType, transform);
			drink.transform.localPosition = new Vector3(Mathf.Cos(drinkAngle) * (0.1f + 0.01f * drinkLimit), 0, Mathf.Sin(drinkAngle) * (0.1f + 0.01f * drinkLimit));
			//drink.transform.localRotation *= Quaternion.Euler(0, -Mathf.Rad2Deg * drinkAngle, 0);
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
		transform.localPosition = new Vector3(0.5f, 0.3f + height, 0.9f);
	}

	bool Throw() {
		if (!isCarried) {
			return false;
		} else {
			isCarried = false;
			meshCollider.enabled = true;
			rb.isKinematic = false;
			transform.parent = null;
			rb.AddForce(Camera.main.transform.forward * 10, ForceMode.VelocityChange);
			drinkCount = 0;

			for (int i = 0; i < glasses.Length; i++) {
				if (glasses[i] != null) {
					glasses[i].GetComponent<BoxCollider>().enabled = true;
					Rigidbody glassRb = glasses[i].GetComponent<Rigidbody>();
					glassRb.isKinematic = false;
					glassRb.AddForce(Camera.main.transform.forward * 10, ForceMode.VelocityChange);
					glasses[i].transform.parent = null;
					glasses[i] = null;
				}
			}

			return true;
		}
	}

	void OnMouseDown(){
		ReturnTray();
		GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ClearCrosshair();
	}

	void OnMouseEnter(){
		if(enabled)
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTrayCrosshair();
	}

	void OnMouseExit(){
		if(enabled)
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ClearCrosshair();
	}

	public void ReturnTray() {
		if (!isCarried) {
			isCarried = true;
			meshCollider.enabled = false;
			rb.isKinematic = true;
			transform.parent = player.transform;
			transform.rotation = Quaternion.identity;
		}
	}
}
