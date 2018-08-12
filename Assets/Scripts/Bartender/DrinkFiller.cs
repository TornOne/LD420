using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DrinkFiller : MonoBehaviour {
	public Transform drinkSpawnPosition;
	public GameObject drinkPrefab;
	private Tray tray;
	public string drinkType;
	public float fillTime = 0.2f, drinkMoveTime = 0.3f;
	public bool isFilling = false;

	void Start(){
		tray = GameObject.FindGameObjectWithTag("Tray").GetComponent<Tray>();
	}

	void OnMouseEnter() {
		GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTapCrosshair();
	}

	void OnMouseExit() {
		GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ClearCrosshair();
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
		drink.GetComponent<Whiskey>().filled = 0;
		yield return new WaitForSeconds(0.1f);

		// Fill glass
		float filled = 0f;
		while(filled < 1){
			filled += Time.deltaTime / fillTime;
			drink.GetComponent<Whiskey>().filled = Mathf.Min(filled, 1);
			yield return null;
		}

		float startTime = Time.time;

		while(Vector3.Distance(drink.transform.position, tray.transform.position) > 0.1f){
			if(!tray.isCarried) {
				Destroy(drink);
				isFilling = false;
				yield break;
			}

			float moveDistance = Vector3.Distance(drink.transform.position, tray.transform.position) / (startTime + drinkMoveTime - Time.time) * Time.deltaTime;
			drink.transform.position = Vector3.MoveTowards(drink.transform.position, tray.transform.position, moveDistance);
			yield return null;
		}

		Destroy(drink);
		tray.AddDrink(drinkType);
		isFilling = false;
	}
}
