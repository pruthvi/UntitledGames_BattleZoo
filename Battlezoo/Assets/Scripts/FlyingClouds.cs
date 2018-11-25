/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;

public class FlyingClouds : MonoBehaviour {

    #region Variables
    [Range(1.0f, 20.0f)]
    public float speed;
    private Rigidbody2D rb;

	#endregion

	void Start ()
	{

        speed = Random.Range(1.0f, 50.0f);
        rb = GetComponent<Rigidbody2D>();


	}
	
	void Update ()
	{
        float speed = Random.Range(0.0f, 2.0f);
        Vector2 movement = new Vector2(-1, 0);

        this.GetComponent<Rigidbody2D>().velocity = movement * speed;

        //        Debug.Log("Cloud Speed :" + speed);

        if(transform.position.x < -20.0f)
        {
            Destroy(this.gameObject);
        }

    }

}
