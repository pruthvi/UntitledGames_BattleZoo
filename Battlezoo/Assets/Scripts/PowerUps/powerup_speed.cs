using UnityEngine;
using System.Collections;

public class powerup_speed : MonoBehaviour {

    public float increaseSpeedBy = 5;       // Increase Player Speed by 5x
    public float timeToLast = 10;       // Time to last the modifier

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject player = collision.gameObject;

        if (player.tag == "Player")
        {
       //     player.GetComponent<PlayerMovement>().SpeedModifier(increaseSpeedBy, timeToLast);

            Destroy(this.gameObject);
        }

    }


}
