using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject spawnable;
	public float spawnTime;

	void Start() {
		StartCoroutine(Spawn(spawnTime));
	}

	IEnumerator Spawn(float time) {
		Instantiate(spawnable, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(spawnTime);
	}
}
