using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Image crosshair;
	public RectTransform happinessArrow;
	public Text moneyIndicator;

	public Sprite tapSprite, whiskeySprite, grabSprite, holdSprite, traySprite, repairSprite, crosshairSprite;
	public float happinessCap = 100f;
	public float happiness = 50f;
	public int money = 0;

	public static float grabHappiness = -5f, deathHappiness = -15f, fightingHappiness = -5f, leaveHappiness = 25f;

	void Update(){
		happiness = Mathf.Min(happiness, happinessCap);
		happinessArrow.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(90, 0, happiness/happinessCap));
		moneyIndicator.text = "$" + money;
	}

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
