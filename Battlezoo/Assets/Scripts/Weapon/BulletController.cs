using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float damage;
    public float maxTravelDistance;
    public float speed = 15;
    private Vector3 startPoint;

    private Rigidbody2D rb;
    private bool isAddingForce;
    private Vector2 forceToAdd;

    // Bullet
    [Header("Bullet Reloading")]
    public int _bulletCount = 0;        // Storing the number of Fire Shoot
    public int _maxAmmo = 6;           // Total amount of bullets in Magazine
    public bool _canShootBullet = true;       // Can player shoot the Bullet
    public float _bulletReloadTime = 5;  // total Time takes to reload the magazine

    void Start()
    {
        startPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        _bulletCount = 0;
    }

    void Update()
    {
        if (Vector3.Distance(startPoint, transform.position) >= maxTravelDistance)
        {
            Destroy(gameObject);
        }


    }

    void FixedUpdate()
    {
        if (isAddingForce)
        {
            rb.AddForce(forceToAdd);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Stat stat = other.gameObject.GetComponent<Stat>();
            stat.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    public void ApplyForce(Vector2 force, float time)
    {
        StartCoroutine(applyForce(time));
        forceToAdd = force;
    }

    IEnumerator applyForce(float time)
    {
        isAddingForce = true;
        yield return new WaitForSeconds(time);
        isAddingForce = false;
    }

    
}
