/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    #region Variables
    public string Start;
	#endregion

	
    public void StartGame()
    {
        SceneManager.LoadScene(Start);
    }
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void Exit()
    {
        Application.Quit();
    }

}
