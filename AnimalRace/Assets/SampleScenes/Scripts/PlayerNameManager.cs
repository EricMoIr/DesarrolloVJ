using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RaceData;
public class PlayerNameManager : MonoBehaviour
{

    //[SerializeField]
    //public InputField nameTextField;
    [SerializeField]
    public Text nameHeader;
    [SerializeField]
    public Text fixedHeader;
    public int currentPlayer;
    // Use this for initialization
    void Start()
    {
        //GameObject raceManager = GameObject.FindGameObjectsWithTag("DataToNextScene")[0];
        nameHeader.text = "";
        fixedHeader.text = "Nombre Jugador 1: ";
        currentPlayer = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLetter(string letter)
    {
        if (letter == "OK" && nameHeader.text != "" )
        {
            switch (currentPlayer)
            {
                case 1:
                    RaceDataHolder.isNew.PlayerName1 = nameHeader.text;
                    if (currentPlayer == RaceDataHolder.isNew.NumberOfPlayers)
                    {
                        SceneManager.LoadScene(RaceDataHolder.isNew.SelectedTrack);
                    }
                    else
                    {
                        nameHeader.text = "";
                        currentPlayer++;
                        fixedHeader.text = "Nombre Jugador " + currentPlayer +": ";
                    }
                    break;
                case 2:
                    RaceDataHolder.isNew.PlayerName2 = nameHeader.text;
                    if (currentPlayer == RaceDataHolder.isNew.NumberOfPlayers)
                    {
                        SceneManager.LoadScene(RaceDataHolder.isNew.SelectedTrack);
                    }
                    else
                    {
                        nameHeader.text = "";
                        currentPlayer++;
                        fixedHeader.text = "Nombre Jugador " + currentPlayer +": ";

                    }
                    break;
                case 3:
                    RaceDataHolder.isNew.PlayerName3 = nameHeader.text;
                    if (currentPlayer == RaceDataHolder.isNew.NumberOfPlayers)
                    {
                        SceneManager.LoadScene(RaceDataHolder.isNew.SelectedTrack);
                    }
                    else
                    {
                        nameHeader.text = "";
                        currentPlayer++;
                        fixedHeader.text = "Nombre Jugador " + currentPlayer +": ";
                    }
                    break;
                case 4:
                    RaceDataHolder.isNew.PlayerName4 = nameHeader.text;
                    if (currentPlayer == RaceDataHolder.isNew.NumberOfPlayers)
                    {
                        SceneManager.LoadScene(RaceDataHolder.isNew.SelectedTrack);
                    }
                    else
                    {
                        nameHeader.text = "";
                        currentPlayer++;
                    }
                    break;
            }


        }
        else
        {
            if (letter != "BA")
            {
                if (letter == "SB") letter = " ";
                nameHeader.text += letter;
            }
            else
            {
                if (nameHeader.text != "")
                {
                    nameHeader.text = nameHeader.text.Remove(nameHeader.text.Length - 1);
                }
            }
        }

      
    }
}
