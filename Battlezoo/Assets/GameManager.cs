/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Variables

    public static int Ammo;
    public static int Health;

    public Text AmmoText;
    public Text HealthText;

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
	}

}
