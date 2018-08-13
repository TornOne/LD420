using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BillboardScript : MonoBehaviour {

	private Image image;
	private Transform player;
	private IconState state;

	public enum IconState{
		hidden,
		shown,
		blinking
	}

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		player = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.LookAt(player);
		if(state == IconState.blinking) image.color = Time.time % 1 < 0.5f ? new Color(1f, 1f, 1f, 1f) : new Color(0, 0, 0, 0);
	}

	public void SetState(IconState newState){
		state = newState;
		switch(newState){
			case IconState.hidden:
				image.color = new Color(0, 0, 0, 0);
				break;
			case IconState.shown:
				image.color = new Color(1f, 1f, 1f, 1f);
				break;
		}
	}
}
