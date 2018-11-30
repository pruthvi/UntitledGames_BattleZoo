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
        
    }

    void Update ()
	{
    }
    

}
