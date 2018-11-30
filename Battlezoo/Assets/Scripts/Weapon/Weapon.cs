using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weapon : MonoBehaviour {


    public enum BulletTravelMode { Linear, WithForce }

    public BulletTravelMode travelMode;
    public Transform muzzle;
    public Transform barrel;

    [Header("Bullet Info")]
    public GameObject bullet;

    // Private Variable
    private Animator anim;
    public BulletController bulletController;

    // Use this for initialization
    void Start () {
//        anim = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	//void Update () {

 //       if (!hasAuthority)
 //       {
 //           return;
 //       }

 //       if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
 //       {
 //           Vector2 direction = muzzle.position - barrel.position;
 //           float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

 //           if (bulletController._canShootBullet)
 //           {
 //               // Incrementing the Bullet Fired Counter
 //               bulletController._bulletCount++;
 //               //Debug.Log(bulletController._bulletCount);

 //               // Check Magazine and Reload, if needed
 //               ReloadBullet(bulletController._bulletReloadTime);

 //               // Instantiating Bullet in the barrel's direction
 //               var objBullet = Instantiate(bullet, muzzle.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
 //               switch (travelMode)
 //               {
 //                   case BulletTravelMode.Linear:
 //                       objBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletController.speed;
 //                       break;
 //                   case BulletTravelMode.WithForce:
 //                       bulletController.ApplyForce(direction * bulletController.speed, 3);
 //                       break;
 //               }
 //               anim.SetBool("IsFiring", true);

 //           }
 //       }
 //       else
 //       {
 //           anim.SetBool("IsFiring", false);
 //       }
	//}

        
    /*
    //------------- Reloading Bullet Count--------------------//
    public void ReloadBullet(float _bulletReloadTime)
    {
        if (bulletController._bulletCount >= bulletController._maxAmmo)
        {
            // Recharge the Ammo
            StartCoroutine(ReloadBulletTimmer(_bulletReloadTime));
        }
    }
    IEnumerator ReloadBulletTimmer(float _bulletReloadTime)
    {
        bulletController._canShootBullet = false;                // Player cannot Shoot any bullet
        bulletController._bulletCount = 0;                       // Magazine Reloaded
        yield return new WaitForSeconds(_bulletReloadTime);
        bulletController._canShootBullet = true;                 // Player can Shoot bullets
    }

    */
}

