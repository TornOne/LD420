using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollisions : MonoBehaviour {
	// Use this for initialization
	void Awake () {
		Collider[] colliders = GetComponentsInChildren<Collider>();
		for(int i = 0; i < colliders.Length; i++){
			for(int j = i+1; j < colliders.Length; j++){
				Physics.IgnoreCollision(colliders[i], colliders[j]);
				//Debug.Log(colliders[i] + ";" + colliders[j]);
			}
		}
	}
}
