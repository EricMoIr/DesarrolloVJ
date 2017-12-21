using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishButtoms : MonoBehaviour {
    [SerializeField]
    private Button buttonRestart;
    [SerializeField]
    private Button buttonMenu;

    // Use this for initialization
    void Start () {
        HideButton();
	}
	
	// Update is called once per frame
	void Update () {
        if (CarBehavior.finished == RaceData.RaceDataHolder.isNew.NumberOfPlayers) { 
            StartCoroutine(GoToMenu());

        }
    }

    IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void ShowButton()
    {
        buttonRestart.enabled = true;
        buttonRestart.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
        buttonMenu.enabled = true;
        buttonMenu.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
    }

    public void HideButton()
    {
        buttonRestart.enabled = false;
        buttonRestart.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
        buttonMenu.enabled = false;
        buttonMenu.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
    }


}
