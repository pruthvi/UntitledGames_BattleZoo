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


        rb = GetComponent<Rigidbody2D>();
        cloudImg = GetComponent<SpriteRenderer>();
        Opacity = Random.Range(0.01f, 0.80f);


        col.a = Opacity;

        cloudImg.color = col;

        posY = this.transform.position.y;

    }

    void Update()
    {
        Vector2 movement = new Vector2(-1, 0);

        this.GetComponent<Rigidbody2D>().velocity = movement * speed;

        if (transform.position.x < -20.0f)
        {

            Vector2 newPos = new Vector2(20, posY);
            this.transform.position = newPos;

        }

    }

}
