using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {



    public enum BulletTravelMode { Linear, WithForce }

    public BulletTravelMode travelMode;
    public Transform fireBarrel;

    [Header("Bullet Info")]
    public GameObject bullet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Vector2 direction = fireBarrel.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var objBullet = Instantiate(bullet, fireBarrel.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
            BulletController bulletController = objBullet.GetComponent<BulletController>();
            switch (travelMode)
            {
                case BulletTravelMode.Linear:
                    objBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletController.speed;
                    break;
                case BulletTravelMode.WithForce:
                //    bulletController.ApplyForce(direction * bulletController.speed, 3);
                    break;
            }
        }
	}
}
