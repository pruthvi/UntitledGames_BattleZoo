using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotator : MonoBehaviour {
    
    public float minAngle;
    public float maxAngle;

    // Private Variables
    public bool isCharacterFlipped = true;    // check if Character is Flipped
	
	// Update is called once per frame
	void Update () {

        if (!isCharacterFlipped)
        {
            BarrelRotation(1);
        }

        if (isCharacterFlipped)
        {
            BarrelRotation(-1);
        }
	}

    void BarrelRotation(int dir)
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
                rotation.eulerAngles = new Vector3(0, 0, isCharacterFlipped ? maxAngle : minAngle);
            }
            else if (direction.x < 0)// if in 3rd quadrant
            {
                rotation.eulerAngles = new Vector3(0, 0, isCharacterFlipped ? minAngle : maxAngle);
            }
        }
        if(dir == -1)
        {
            rotation = Quaternion.Inverse(rotation);
        }
        transform.rotation = rotation;
    }

}
