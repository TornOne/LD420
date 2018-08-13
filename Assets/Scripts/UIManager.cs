using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Image crosshair;
	public Sprite tapSprite, whiskeySprite, grabSprite, holdSprite, traySprite, repairSprite, crosshairSprite;

	public void SetTapCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = tapSprite;
	}

	public void SetGrabCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = grabSprite;
	}

	public void SetTrayCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = traySprite;
	}

	public void SetWhiskeyCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = whiskeySprite;
	}

	public void SetHoldCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = holdSprite;
	}

	public void SetRepairCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = repairSprite;
	}

	public void ClearCrosshair(){
		crosshair.enabled = true;
		crosshair.sprite = crosshairSprite;
	}

}
