using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUp : NetworkBehaviour
{
    public PowerUpInfo[] powerUps;

    public PowerUpInfo powerUpInfo;

    void Start()
    {
        if (powerUps.Length > 0)
        {
            powerUpInfo = powerUps[Random.Range(1, powerUps.Length)];
            SpriteRenderer render = GetComponent<SpriteRenderer>();
            if (render != null)
            {
                render.sprite = powerUpInfo.powerUpIcon;
                // Reset the Collider
                PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
                if(collider != null)
                {
                    collider.gameObject.SetActive(false);
                    collider.gameObject.SetActive(true);
                }
                // Aplly Color
                switch (powerUpInfo.PowerUpType)
                {
                    case PowerUpType.Heal:
                        render.color = Color.green;
                        break;
                    case PowerUpType.Strength:
                        render.color = Color.red;
                        break;
                    case PowerUpType.JumpStrength:
                        render.color = Color.yellow;
                        break;
                    case PowerUpType.Speed:
                        render.color = Color.cyan;
                        break;
                    case PowerUpType.Poison:
                        render.color = Color.magenta;
                        break;
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
