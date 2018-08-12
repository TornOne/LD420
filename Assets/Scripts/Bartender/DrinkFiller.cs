﻿using UnityEngine;
using System.Collections;

public class DrinkFiller : MonoBehaviour {
	public Transform drinkSpawnPosition;
	public GameObject drinkPrefab;
	public GameObject UI;
	public Tray tray;
	public string drinkType;
	public float fillTime = 0.5f, drinkMoveSpeed = 1;
	public bool isFilling = false;

	void OnMouseEnter() {
		UI.SetActive(true);
	}

	void OnMouseExit() {
		UI.SetActive(false);
	}

	private void OnMouseUp() {

	}

	void OnMouseDown() {
		if ((tray.transform.position - transform.position).magnitude <= 1.5f && tray.isCarried && !isFilling) {
			Debug.Log("Filling glass");
			StartCoroutine(GlassFillCoroutine());
		}
	}

	private IEnumerator GlassFillCoroutine(){
		isFilling = true;
		// Spawn drink
		GameObject drink = Instantiate(drinkPrefab, drinkSpawnPosition.position, drinkSpawnPosition.rotation);
		yield return new WaitForSeconds(0.1f);

		// Fill glass
		float filled = 0f;
		while(filled < 1){
			filled += Time.deltaTime / fillTime;
			drink.GetComponent<Whiskey>().filled = Mathf.Min(filled, 1);
			yield return null;
		}

		while(Vector3.Distance(drink.transform.position, tray.transform.position) > 0.1f){
			if(!tray.isCarried) {
				Destroy(drink);
				yield break;
			}

			float moveDistance = Time.deltaTime * drinkMoveSpeed * Vector3.Distance(drink.transform.position, tray.transform.position);
			drink.transform.position = Vector3.MoveTowards(drink.transform.position, tray.transform.position, moveDistance);
			yield return null;
		}

		Destroy(drink);
		tray.AddDrink(drinkType);
		isFilling = false;
	}
}
