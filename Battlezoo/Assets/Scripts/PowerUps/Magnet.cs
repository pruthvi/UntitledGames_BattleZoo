using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

    Rigidbody2D rb;
    GameObject player;
    Vector2 _playerDirection;
    bool _attractPlayer;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        //modifier = GetComponent<AbilityModifier>();
    }

    // Update is called once per frame
    void Update()
    {

        // Attrack to Player Like MAGNET
        if (_attractPlayer)
        {
            _playerDirection = -(transform.position - player.transform.position).normalized;
            rb.velocity = new Vector2(_playerDirection.x, _playerDirection.y) * 50f * Time.time;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PowerUpMagnet")
        {          
            _attractPlayer = true;
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
