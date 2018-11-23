/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour {

    #region Variables

    public InputField nameInput;
    public Text errorText;

    string playerName;

	#endregion

	void Start ()
	{
        errorText.enabled = false;

	}
	
	void Update ()
	{
		
	}

   public void OnClick()
    {
        if(nameInput == null)
        {
            errorText.enabled = true;
            errorText.text = "Player Name is Mandatory";
            nameInput.Select();
            nameInput.ActivateInputField();
        }
        else
        {
            playerName = nameInput.text;
            PlayerPrefs.SetString("Player Name", playerName);
            SceneManager.LoadScene("MainScene");

        }


    }

}
