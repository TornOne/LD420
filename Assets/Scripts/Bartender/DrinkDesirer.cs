using UnityEngine;
using System.Collections;

public class DrinkDesirer : MonoBehaviour {
	public CustomerAI AI;
	BartenderLogic player;
	Tray tray;
	string drinkType = "";
	public string[] drinkTypes;
	public bool isDrinking = false;
	public GameObject drinkPrefab;
	public Transform hand;
	public float drinkingTime = 10f;
	public float drinkAlcoholContent = 0.2f;
	public AudioClip punchSfx;

	public float aggressionMoneyMult = 0.1f;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<BartenderLogic>();
		tray = GameObject.FindGameObjectWithTag("Tray").GetComponent<Tray>();
	}

	void OnMouseDown() {
		if (Vector3.Distance(player.transform.position, transform.position) > 3f) return;
		if (!tray.isCarried && !player.isCarrying) {
			player.isCarrying = true;
			player.carry = AI.gameObject;
			AI.state = CustomerAI.State.struggling;
			AI.transform.parent = Camera.main.transform;
			AI.transform.localPosition = new Vector3(0, 0, 2);
			AudioSource.PlayClipAtPoint(punchSfx, player.transform.position);
			AI.Hp = Mathf.Min(AI.Hp, 2);
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetHoldCrosshair();
		} else if (AI.state == CustomerAI.State.waiting && (tray.transform.position - transform.position).magnitude <= 2 && tray.RemoveDrink(drinkType) && !isDrinking) {
			drinkType = "";
			StartCoroutine(DrinkingCoroutine());
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().money += (int) (5 + (100f - AI.AggressionLevel) * aggressionMoneyMult);
		}
	}

	void OnMouseOver(){
		if(Vector3.Distance(player.transform.position, transform.position) > 3f){
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ClearCrosshair();
		}else{
			if (!tray.isCarried && !player.isCarrying) {
				GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetGrabCrosshair();
			} else if (AI.state == CustomerAI.State.waiting && !isDrinking) {
				GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetWhiskeyCrosshair();
			}
		}
	}

	void OnMouseExit(){
		GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ClearCrosshair();
	}

	public void DesireDrink() {
		drinkType = drinkTypes[Random.Range(0, drinkTypes.Length)];
	}

	IEnumerator DrinkingCoroutine(){
		isDrinking = true;
		AI.AggressionLevel -= 50f;
		GameObject drink = Instantiate(drinkPrefab, hand.position, hand.rotation);
		drink.GetComponent<Collider>().enabled = false;
		drink.GetComponent<Rigidbody>().isKinematic = true;
		drink.transform.parent = hand;

		float drinkLeft = 1f;
		while(drinkLeft > 0){
			yield return null;
			drinkLeft -= Time.deltaTime / drinkingTime;
			drink.GetComponent<Whiskey>().filled = drinkLeft;
		}

		drink.transform.parent = null;
		drink.GetComponent<Rigidbody>().isKinematic = false;
		drink.GetComponent<Collider>().enabled = true;

		AI.DrinkCount--;
		AI.GetComponent<MuscleController>().consciousness -= drinkAlcoholContent;
		isDrinking = false;
	}
}
