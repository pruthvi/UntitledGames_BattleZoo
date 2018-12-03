using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public BulletControllerScriptableObject bulletScriptable;

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
