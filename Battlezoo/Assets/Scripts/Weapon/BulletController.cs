using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    //public float damage;
    //public float maxTravelDistance;
    //public float speed = 15;
    //private Vector3 startPoint;

    //private Rigidbody2D rb;
    //private bool isAddingForce;
    //private Vector2 forceToAdd;
    //public PowerUpManager powerUpManager;

    //// Bullet
    //[Header("Bullet Reloading")]
    //public int _bulletCount = 0;        // Storing the number of Fire Shoot
    //public int _maxAmmo = 6;           // Total amount of bullets in Magazine
    //public bool _canShootBullet = true;       // can player shoot the bullet
    //public float _bulletReloadTime = 5;  // total Time takes to reload the magazine

    public bool isEnemyKilled = false;
    private int _randomPowerUp;

    public BulletControllerScriptableObject bulletScriptable;
    public PowerUpManager powerup;

    public GameObject[] powerUps = new GameObject[3];

    //void Update()
    //{
    //    if (Vector3.Distance(startPoint, transform.position) >= bulletScriptable.maxTravelDistance)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //void FixedUpdate()
    //{
    //    if (isAddingForce)
    //    {
    //        rb.AddForce(forceToAdd);
    //    }
    //}
    private void LateUpdate()
    {
        
        if (isEnemyKilled)
        {
            Vector2 position = transform.position;
            Debug.Log("Ypu are in update");
            // if (powerup.powerUp.Length != 0)
            //{
            //    // Spawn Random PowerUp
            //    _randomPowerUp = Random.Range(1, powerup.powerUp.Length);
            //    var randomPowerUp = powerup.powerUp[_randomPowerUp];
            //    var powerupInstantiate = Instantiate(randomPowerUp, position, Quaternion.identity);

            //    Debug.Log(_randomPowerUp);
            //    isEnemyKilled = false;
            //    Destroy(powerupInstantiate, 2);
            //}

            // Instantiate Random PowerUp
            _randomPowerUp = Random.Range(1, powerUps.Length);
            var randomPowerUp = powerUps[_randomPowerUp];
            var powerup = Instantiate(randomPowerUp, position, Quaternion.identity);
            isEnemyKilled = false;
            Destroy(powerup, 2);

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Stat stat = other.gameObject.GetComponent<Stat>();
            stat.TakeDamage(bulletScriptable.damage);
            Destroy(gameObject);
        }


        if (other.gameObject.tag == "Scientist")
        {
            
            Debug.Log(isEnemyKilled);
            Destroy(other.gameObject);      // Destroy Scientist
            Destroy(gameObject);            // Destroy Bullet
            isEnemyKilled = true;
            SpawnPowerUp();
        }

    }

    private void SpawnPowerUp()
    {
        Vector2 position = transform.position;
        Debug.Log("Ypu are in update");
        if (powerup.powerUp.Length != 0)
        {
            // Spawn Random PowerUp
            _randomPowerUp = Random.Range(1, powerup.powerUp.Length);
            var randomPowerUp = powerup.powerUp[_randomPowerUp].Object;
            var powerupInstantiate = Instantiate(randomPowerUp, position, Quaternion.identity);

            Debug.Log(_randomPowerUp);
            isEnemyKilled = false;
            Destroy(powerupInstantiate, 2);
        }

        //// Instantiate Random PowerUp
        //_randomPowerUp = Random.Range(1, powerUps.Length);
        //var randomPowerUp = powerUps[_randomPowerUp];
        //var powerup = Instantiate(randomPowerUp, position, Quaternion.identity);
        //isEnemyKilled = false;
        //Destroy(powerup, 2);
    }

    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Stat stat = other.gameObject.GetComponent<Stat>();
    //        stat.TakeDamage(bulletScriptable.damage);
    //        Destroy(gameObject);
    //    }


    //    if (other.gameObject.tag == "Scientist")
    //    {
    //        Destroy(other.gameObject);      // Destroy Scientist
    //        Destroy(gameObject);            // Destroy Bullet
    //        isEnemyKilled = true;
    //    }
    //}


    //public void ApplyForce(Vector2 force, float time)
    //{
    //    StartCoroutine(applyForce(time));
    //    forceToAdd = force;
    //}

    //IEnumerator applyForce(float time)
    //{
    //    isAddingForce = true;
    //    yield return new WaitForSeconds(time);
    //    isAddingForce = false;
    //}


}
