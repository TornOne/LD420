using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject[] spawnables;
	public float spawnTime;

	void Start() {
		StartCoroutine(Spawn(spawnTime));
	}

	IEnumerator Spawn(float time) {
		Instantiate(spawnables[(int) Random.Range(0, spawnables.Length)], transform.position, Quaternion.identity);
		yield return new WaitForSeconds(spawnTime);
		StartCoroutine(Spawn(spawnTime));
	}
}
