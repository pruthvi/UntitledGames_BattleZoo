using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    [Header("Sprite Info")]
    public Transform sprites;
    public Transform groundCheck;

    private float stepRange;

    [Header("NPC Info")]
    public float speed = 15;
    public int direction = 1;

    private Rigidbody2D rBody;

    public LayerMask platformMask;

    private bool isGrounded;
    private bool isBlocked;

    void Start()
    {
        // The sprites for the NPC, used for flipping
        sprites = transform.GetChild(0);
        groundCheck = transform.GetChild(1);

        // The size of the ground check
        stepRange = groundCheck.GetComponent<Collider2D>().bounds.extents.x;

        rBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Get the edge position to check
        Vector2 checkPoint = groundCheck.position + groundCheck.right * stepRange;
        Debug.DrawLine(checkPoint, checkPoint + Vector2.down, Color.blue);

        isGrounded = Physics2D.Linecast(checkPoint, checkPoint + Vector2.down, platformMask);
       // isBlocked = Physics2D.Linecast(checkPoint, checkPoint - Vector2.right * direction, platformMask);
        // Check if it reaches edge, turn around if it does
        if (!isGrounded)
        {
            Debug.Log("Flip");
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

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Bullet")
    //    {
    //        Destroy(gameObject);            // Destroy Bullet
    //        Destroy(other.gameObject);      // Destroy Scientist
    //        SpawnPowerUp();
    //    }
    //}

    //private void SpawnPowerUp()
    //{
    //    Vector2 position = transform.position;
    //    if (powerup.powerUp.Length != 0)
    //    {
    //        // Spawn Random PowerUp
    //        _randomPowerUp = Random.Range(1, powerup.powerUp.Length);

    //        var randomPowerUp = powerup.powerUp[_randomPowerUp].Object;
    //        var powerupInstantiate = Instantiate(randomPowerUp, position, Quaternion.identity);

    //        Destroy(powerupInstantiate, _powerupLife);        // PowerUp Will Destroy in Certain Time
    //    }

    //}


}
