using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayDispenser : MonoBehaviour {

	void OnMouseDown(){
		if(enabled){
			if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 3f){
				GameObject.FindGameObjectWithTag("Tray").GetComponent<Tray>().ReturnTray();
			}
		}
	}

	void OnMouseEnter(){
		if(enabled)
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTrayCrosshair();
	}

	void OnMouseExit(){
		if(enabled)
			GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ClearCrosshair();
	}
}
