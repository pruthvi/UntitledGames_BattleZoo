using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float damage;
    public float maxTravelDistance;
    public float speed = 1;
    private Vector3 startPoint;

    private Rigidbody2D rb;

    private bool isAddingForce;
    private Vector2 forceToAdd;

    void Start()
    {
        startPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
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
