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
    // Use this for initialization
    void Start()
    {
        //GameObject raceManager = GameObject.FindGameObjectsWithTag("DataToNextScene")[0];
        nameHeader.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLetter(string letter)
    {
        if (letter == "OK")
        {
            RaceDataHolder.isNew.PlayerName = nameHeader.text;
            SceneManager.LoadScene(RaceDataHolder.isNew.SelectedTrack);

        }

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
