/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Variables

    public static int Ammo;
    public static int Health;

    public Text AmmoText;
    public Text HealthText;
    public Text PlayerNameText;

    public GameObject lines;

    public string playerName;

	#endregion

	void Start ()
	{
        Ammo = 100;
        Health = 100;

	}
	
	void Update ()
	{
        HealthText.text = "Health: " + Health;
        AmmoText.text = "Ammunition: " + Ammo;
        playerName =  PlayerPrefs.GetString("Player Name", "No Name");
        PlayerNameText.text = playerName;
        //RandomLines();
    }

    void RandomLines()
    {
        int x = 10;
        if (x > 0)
        {
            float width = Random.Range(5f, 35f);
            float stroke = Random.Range(0.1f, 0.3f);
            float LinePosY = Random.Range(4, -1);
            Vector2 linePosition = new Vector2(-6, LinePosY);
            GameObject newLine = Instantiate(lines, linePosition, this.transform.rotation);
            newLine.transform.localScale = new Vector3(width, stroke, 1);
            x++;
        }

    }

}
