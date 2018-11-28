/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;

public class FlyingClouds : MonoBehaviour
{

    #region Variables



    public float speed;
    private Rigidbody2D rb;


    private SpriteRenderer cloudImg;
    public Color col;
    private float Opacity;

    private float posY;


    #endregion

    void Start()
    {

       // speed = Random.Range(minSpeed, maxSpeed);

        rb = GetComponent<Rigidbody2D>();
        cloudImg = GetComponent<SpriteRenderer>();
        Opacity = Random.Range(0.01f, 0.80f);
        Debug.Log("Opacity: " + Opacity);


        col.a = Opacity;

        cloudImg.color = col;

        posY = this.transform.position.y;

    }

    void Update()
    {
        //float speed = Random.Range(0.0f, 2.0f);
        Vector2 movement = new Vector2(-1, 0);

        this.GetComponent<Rigidbody2D>().velocity = movement * speed;

        //        Debug.Log("Cloud Speed :" + speed);

        if (transform.position.x < -20.0f)
        {

            Vector2 newPos = new Vector2(20, posY);
            this.transform.position = newPos;

        }

    }

}
