using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotator : MonoBehaviour {

    public PlayerMovement playerMovement;
    public float minAngle;
    public float maxAngle;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if (playerMovement.isFlipped)
        {
            RotateBarrel(-1);
        }
        else
        {
            RotateBarrel(1);
        }
	}

    private void RotateBarrel(int dir)
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, (dir) * direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotation.eulerAngles = new Vector3(0, 0, Mathf.Clamp(rotation.eulerAngles.z, minAngle, maxAngle));
        //Check if mouse is in the 3rd or 4th quadrant
        if (direction.y < 0)
        {
            if (direction.x > 0)// if in 4th quadrant
            {
                rotation.eulerAngles = new Vector3(0, 0, playerMovement.isFlipped ? maxAngle : minAngle);
            }
            else if (direction.x < 0)// if in 3rd quadrant
            {
                rotation.eulerAngles = new Vector3(0, 0, playerMovement.isFlipped ? minAngle : maxAngle);
            }
        }

        if (playerMovement.isFlipped)
        {
            rotation = Quaternion.Inverse(rotation);
        }

        transform.rotation = rotation;
    }
}
