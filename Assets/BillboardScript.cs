using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillboardScript : MonoBehaviour {

	public Image frame, fill;
	private Transform player;
	private bool isBlinking = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.LookAt(player);
		if(isBlinking) fill.color = Time.time % 1 < 0.5f ? new Color(1f, 1f, 1f, 1f) : new Color(0, 0, 0, 0);
	}

	public void SetAggressionLevel(float aggressionLevel){
		if(aggressionLevel >= 0.9f){
			isBlinking = true;
		}else if(aggressionLevel >= 0.5f){
			fill.color = new Color(1, 1, 1, 1);
			frame.color = new Color(1, 1, 1, 1);
			isBlinking = false;
		}else{
			fill.color = new Color(0, 0, 0, 0);
			frame.color = new Color(0, 0, 0, 0);
			isBlinking = false;
		}
		fill.fillAmount = (aggressionLevel - 0.5f) * 2;
	}
}
