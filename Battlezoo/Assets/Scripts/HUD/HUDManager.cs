using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public PlayerStats stats;

    [Header("UI Reference")]
    public GameObject topPanel;
    public Text hudHP;
    public Text hudAmmo;
    public Text Announcement;

    public GameObject spectatingPanel;
    public Text txtSpectating;

	public static HUDManager instance;

	void Start()
	{
		instance = this;
	}

    public void UpdatePlayerHP(float newHP)
    {
        if (stats != null)
        {
            if(stats.isEliminated)
            {
                updateHUDText(hudHP, "-/-");
            }
            else{
                updateHUDText(hudHP, newHP + "/" + stats.maxHP);
            }
        }
    }

    public void UpdatePlayerAmmo(float newAmmo)
    {
        if (stats != null)
        {
            if(stats.isEliminated)
            {
                updateHUDText(hudAmmo, "-/-");
            }
            else{
                updateHUDText(hudAmmo, newAmmo + "/" + stats.ammoPerMagazine);
            }
        }
    }

    private void updateHUDText(Text uiText, string text){
        if(uiText != null)
        {
            uiText.text = text;
        }
    }

    public void ToggleTopPanel(bool visible)
    {
        topPanel.SetActive(visible);
    }

    public void Spectating()
    {
        spectatingPanel.gameObject.SetActive(true);
    }

    public void UpdateSpectatingUI(string killer)
    {
        txtSpectating.text = "Spectating " + killer;
    }
}
