using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RaceData;

public class lvlManager : MonoBehaviour
{
    [SerializeField]
    private List<Button> hiddenButtons;
    [SerializeField]
    private string timeTrackName;
    // Use this for initialization
    void Start()
    {
        for(int i = 0; i < hiddenButtons.Count; i++)
        {
            HideButton(hiddenButtons.ElementAt(i));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideButton(Button button )
    {
        button.enabled = false;
        button.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
    }

    public void SetTrack(string trackName)
    {
        RaceDataHolder.isNew.SelectedTrack = trackName;
    }

    public void SetNumberOfPlayers(int number)
    {
        RaceDataHolder.isNew.NumberOfPlayers = number;
    }

    public void ShowButton(Button button)
    {
        button.enabled = true;
        button.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
    }

    

    public void loadLvl(string trackName) {
        SceneManager.LoadScene(trackName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReloadLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}