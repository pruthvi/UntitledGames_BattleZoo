using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Scientist : NetworkBehaviour
{
    [Header("Sprite Info")]
    public Transform sprites;
    public Transform groundCheck;

    private float stepRange;

    [Header("NPC Info")]
    public float speed = 15;
    public int direction = 1;
    public float health = 50;

    private Rigidbody2D rBody;

    public LayerMask platformLayer;

    private bool isGrounded;
    private bool isBlocked;

    public GameObject powerUpPrefab;

    void Start()
    {
        if(!isServer)
        {
            return;
        }
        // The sprites for the NPC, used for flipping
        sprites = transform.GetChild(0);
        groundCheck = transform.GetChild(1);

        // The size of the ground check
        stepRange = groundCheck.GetComponent<Collider2D>().bounds.extents.x;

        rBody = GetComponent<Rigidbody2D>();

        // Ignore gravity platform layer
        Physics.IgnoreLayerCollision(gameObject.layer, 13);
        // Ignore player layer
        Physics.IgnoreLayerCollision(gameObject.layer, 14);
         // Ignore npc
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
    }

    void FixedUpdate()
    {
        if(!isServer)
        {
            return;
        }
        // Get the edge position to check
        Vector2 checkPoint = groundCheck.position + groundCheck.right * stepRange;
        isGrounded = Physics2D.Linecast(checkPoint, checkPoint + Vector2.down, platformLayer.value);
        // isBlocked = Physics2D.Linecast(checkPoint, checkPoint - Vector2.right * direction, platformMask);
        // Check if it reaches edge, turn around if it does
        if (!isGrounded)
        {
            Flip();
        }

        Vector2 vel = rBody.velocity;
        vel.x = speed * direction;
        rBody.velocity = vel;
    }

    void Flip()
    {
        // Flip the direction
        direction *= -1;

        // Flip the sprites
        Vector3 rot = transform.eulerAngles;
        rot.y += 180;
        transform.eulerAngles = rot;

        //Vector2 scale = sprites.localScale;
        //scale.x *= direction;
        //sprites.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                health -= projectile.damage;
                Destroy(other.gameObject);
                if (health <= 0)
                {
                    SpawnPowerUp();
                    Destroy(this.gameObject);
                }
            }

        }
    }

    public void SpawnPowerUp()
    {
        GameObject powerup = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(powerup);
        powerup.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2, ForceMode2D.Impulse);
        Destroy(powerup, 10);
    }


}
