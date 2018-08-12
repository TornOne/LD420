using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Image crosshair;
	public Sprite tapSprite, whiskeySprite, grabSprite, holdSprite, traySprite;

	public void SetTapCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = tapSprite;
	}

	public void SetGrabCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = grabSprite;
	}

	public void ClearCrosshair(){
		crosshair.enabled = false;
		crosshair.sprite = null;
	}

}
