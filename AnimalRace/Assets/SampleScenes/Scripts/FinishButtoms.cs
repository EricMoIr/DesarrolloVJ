using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishButtoms : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HideButton();
	}
	
	// Update is called once per frame
	void Update () {
        if (CarBehavior.finished == RaceData.RaceDataHolder.isNew.NumberOfPlayers)
            ShowButton();
    }

    public void ShowButton()
    {
        enabled = true;
        GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
    }

    public void HideButton()
    {
        enabled = false;
        GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
    }


}
