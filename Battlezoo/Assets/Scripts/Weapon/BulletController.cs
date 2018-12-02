using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    private bool isEnemyKilled = false;
    private int _randomPowerUp;

    public float _powerupLife = 10.0f;
    public BulletControllerScriptableObject bulletScriptable;
    public PowerUpManager powerup;


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
            Destroy(other.gameObject);      // Destroy Scientist
            Destroy(gameObject);            // Destroy Bullet
            isEnemyKilled = true;
            SpawnPowerUp();
        }

    }

    private void SpawnPowerUp()
    {
        Vector2 position = transform.position;
        if (powerup.powerUp.Length != 0)
        {
            // Spawn Random PowerUp
            _randomPowerUp = Random.Range(1, powerup.powerUp.Length);
            var randomPowerUp = powerup.powerUp[_randomPowerUp].Object;
            var powerupInstantiate = Instantiate(randomPowerUp, position, Quaternion.identity);

            isEnemyKilled = false;
            Destroy(powerupInstantiate, _powerupLife);        // PowerUp Will Destroy in Certain Time
        }

    }



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
