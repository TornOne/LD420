using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public Light startGameLight;

	void OnMouseEnter(){
		startGameLight.intensity = 10;
	}

	void OnMouseExit(){
		startGameLight.intensity = 0;
	}

	void OnMouseDown(){
		SceneManager.LoadScene("UduScene");
	}
}
