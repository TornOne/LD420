using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Whiskey : MonoBehaviour {

	[Range(0, 1)] public float filled = 0;
	public GameObject liquid, iceCubes;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		liquid.transform.localScale = new Vector3(1, 1, filled);
		iceCubes.transform.localPosition = new Vector3(0, 0, -0.3f + filled * (0.2f + Mathf.Sin(Time.time) * 0.05f));
	}
}
