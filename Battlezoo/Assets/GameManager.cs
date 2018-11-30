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

  //  public GameObject lines;

    public string playerName;
    private int characterNo;


    public GameObject camel;
    public GameObject chicken;
    public GameObject meerket;
    public GameObject turtle;

    public Transform spawnPoint1;
    #endregion


    void Start ()
	{
        Ammo = 100;
        Health = 100;
        playerName = PlayerPrefs.GetString("Player Name", "No Name");
        PlayerNameText.text = playerName;
        characterNo = PlayerPrefs.GetInt("CharacterNo");
        PlayerBirth(characterNo);
    }

    void Update ()
	{
        HealthText.text = "Health: " + Health;
        AmmoText.text = "Ammunition: " + Ammo;

        //RandomLines();
    }
    /*
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
        */


    void PlayerBirth(int charNo)
    {
        switch (charNo)
        {
            case 1:
                Instantiate(camel, spawnPoint1.transform.position, spawnPoint1.transform.rotation);

                
                break;
            case 2:
                Instantiate(chicken, spawnPoint1.transform.position, spawnPoint1.transform.rotation);

                break;
            case 3:
                Instantiate(meerket, spawnPoint1.transform.position, spawnPoint1.transform.rotation);

                break;
            case 4:
                Instantiate(turtle, spawnPoint1.transform.position, spawnPoint1.transform.rotation);

                break;
            default:
                break;

        }
    }

}
