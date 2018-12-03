using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlatform : MonoBehaviour
{

    public float fallDistance = 5;
    public float correctSpeed = 5;
    private Rigidbody2D rb2d;

    private Vector3 startPosition;

    private bool raising = false;
    private bool falling = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        startPosition = transform.position;
    }

    void Update()
    {

        float platformDistanceFromStart = startPosition.y - transform.position.y;

        if ((platformDistanceFromStart >= fallDistance && falling) || (platformDistanceFromStart <= 0 && raising))
        {
            rb2d.bodyType = RigidbodyType2D.Static;
            if (platformDistanceFromStart <= 0 && raising)
            {
                rb2d.transform.position = Vector2.MoveTowards(transform.position, startPosition, correctSpeed * Time.deltaTime);
            }
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            falling = true;
            raising = false;
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.gravityScale = 1;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            raising = true;
            falling = false;
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.gravityScale = -1;
        }
    }

}