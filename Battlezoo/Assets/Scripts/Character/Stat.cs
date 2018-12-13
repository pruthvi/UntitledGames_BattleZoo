using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour {

    public float maxHp = 100;
    public float currentHp;

	// Use this for initialization
	void Start () {
        currentHp = maxHp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {

            Debug.Log("Player is Killed!");
        }
    }
}
