using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class PlayerNameManager : MonoBehaviour {

    [SerializeField]
    public InputField nameTextField;
    // Use this for initialization
    void Start () {
        nameTextField.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetLetter(string letter)
    {
        if(letter == "OK")
        {
            SceneManager.LoadScene("Pista1");

        }

        if (letter != "BA")
        {
            if (letter == "SB") letter = " ";
            nameTextField.text += letter;
        }
        else
        {
            if(nameTextField.text != "")
            {
                nameTextField.text = nameTextField.text.Remove(nameTextField.text.Length - 1);
            }
        }
    }
}
