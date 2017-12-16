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
    private Text header;
    [SerializeField]
    private Text body;
    [SerializeField]
    private string timeTrackName;
    // Use this for initialization
    void Start()
    {
        for(int i = 0; i < hiddenButtons.Count; i++)
        {
            HideButton(hiddenButtons.ElementAt(i));
        }
        header.text = "";
        body.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideButton(Button button )
    {
        button.enabled = false;
        button.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
        button.GetComponentInChildren<Text>().color = Color.clear;
        
    }

    public void SetTrack(string trackName)
    {
        RaceDataHolder.SelectedTrack = trackName;
    }

    public void SetNumberOfPlayers(int number)
    {
        RaceDataHolder.NumberOfPlayers = number;
    }

    public void ShowButton(Button button)
    {
        button.enabled = true;
        button.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
        button.GetComponentInChildren<Text>().color = Color.black;
    }

    public void loadLvl(string nameScene) {
        SceneManager.LoadScene(nameScene);
    }

    public void LoadSelectedLvl()
    {
        SceneManager.LoadScene(RaceDataHolder.SelectedTrack);
    }
}
