using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CorpseCleaner : MonoBehaviour {

	public float cleaningInterval = 5f;
	private BoxCollider collider;

	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider>();
		StartCoroutine(CorpseCleaningCoroutine());
	}

	IEnumerator CorpseCleaningCoroutine(){
		yield return new WaitForSeconds(cleaningInterval);
		CustomerAI[] ais = GameObject.FindObjectsOfType<CustomerAI>();
		foreach(CustomerAI ai in ais){
			if(collider.bounds.Contains(ai.transform.position)){
				if(ai.state == CustomerAI.State.dead || ai.state == CustomerAI.State.struggling){
					Destroy(ai.gameObject);
				}
			}
		}
		StartCoroutine(CorpseCleaningCoroutine());
	}
}
