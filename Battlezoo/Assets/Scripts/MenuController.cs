using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject[] Panels;
    public MainMenuPanel ActivePanel;


	void Start () {
        //Active the first panel and disable all the rest
        SwitchPanel(0);
    }

    public void SwitchPanel(int index)
    {
        for(int i = 0; i < Panels.Length; i++)
        {
            if (i == index)
            {
                Panels[i].SetActive(true);
            }
            else
            {
                Panels[i].SetActive(false);
            }
        }
        ActivePanel = (MainMenuPanel)index;
    }
}
