/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour {

    #region Variables

    public InputField nameInput;
    public Text errorText;

    string playerName;

    public Text selectedCharacter;

    int characterNo;


    #endregion

    void Start ()
	{
        errorText.enabled = false;
        characterNo = 0;
	}
	

   public void OnClick()
    {
        if(nameInput.text == "")
        {
            errorText.enabled = true;
            errorText.text = "Player Name is Mandatory!";
            nameInput.Select();
            nameInput.ActivateInputField();
        }
        else if(characterNo == 0){
            errorText.enabled = true;
            errorText.text = "Select Character";
        }
        else
        {
            playerName = nameInput.text;
            PlayerPrefs.SetString("Player Name", playerName);
            PlayerPrefs.SetInt("CharacterNo", characterNo);

            SceneManager.LoadScene("MainScene");

        }
        
    }

    public void Camel()
    {
        selectedCharacter.text = "Camel";
        characterNo = 1;
    }

    public void Chicken()
    {
        selectedCharacter.text = "Chicken";
        characterNo = 2;
    }
    public void Meerket()
    {
        selectedCharacter.text = "Meerket";
        characterNo = 3;
    }
    public void Turtle()
    {
        selectedCharacter.text = "Turtle";
        characterNo = 4;
    }
}
