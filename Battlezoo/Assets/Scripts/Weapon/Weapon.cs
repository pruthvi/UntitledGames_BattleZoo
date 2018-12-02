using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {



    public enum BulletTravelMode { Linear, WithForce }

    public BulletTravelMode travelMode;
    public Transform fireBarrel;

    [Header("Bullet Info")]
    private int _bulletCount = 0;        // Storing the number of Fire Shoot
    public bool _canShootBullet = true;       // can player shoot the bullet

    private Animator anim;
    //public BulletController bulletController;
    public BulletControllerScriptableObject bulletScriptable;

    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponent<Animator>();
        _bulletCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Vector2 direction = fireBarrel.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    
            if (_canShootBullet)
            {
                // Incrementing the Bullet Fired Counter
                _bulletCount++;
                //Debug.Log(bulletController._bulletCount);

                // Check Magazine and Reload, if needed
                ReloadBullet(bulletScriptable._bulletReloadTime);

                // Instantiating Bullet in the barrel's direction
                var objBullet = Instantiate(bulletScriptable.bullet, fireBarrel.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
                Destroy(objBullet, bulletScriptable.bulletLifeTime/2);
                switch (travelMode)
                {
                    case BulletTravelMode.Linear:
                        objBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletScriptable.speed;
                        break;
                    case BulletTravelMode.WithForce:
                        //bulletController.ApplyForce(direction * bulletScriptable.speed, 3);
                        break;
                }
                anim.SetBool("IsFiring", true);
                
                
            }
           
        }
        else
        {
            anim.SetBool("IsFiring", false);
        }

        // Destroying the bullet
        //if (Vector3.Distance(bulletStartPoint, bulletScriptable.bullet.transform.position) >= bulletScriptable.maxTravelDistance)
        //{
        //    Destroy(objBullet);
        //}

    }


    //------------- Reloading Bullet Count--------------------//
    public void ReloadBullet(float _bulletReloadTime)
    {
        if (_bulletCount >= bulletScriptable._maxAmmo)
        {
            // Recharge the Ammo
            StartCoroutine(ReloadBulletTimmer(_bulletReloadTime));
        }
    }
    IEnumerator ReloadBulletTimmer(float _bulletReloadTime)
    {
        _canShootBullet = false;                // Player cannot Shoot any bullet
        _bulletCount = 0;                       // Magazine Reloaded
        yield return new WaitForSeconds(_bulletReloadTime);
        _canShootBullet = true;                 // Player can Shoot bullets
    }
}

