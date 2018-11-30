/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;

public class Bullet : MonoBehaviour {

    #region Variables

   public float speed = 1f;
   private Vector2 _direction;

    #endregion

    void Start ()
	{
        _direction = new Vector2(1, 0);
	}
	
	void Update ()
	{
        Vector2 position = transform.position;
    
        position += _direction * speed * Time.deltaTime;
        transform.position = position;


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            Destroy(gameObject);
        }

        if (other.tag == "Player")
        {
            GameManager.Health--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Scientist")
        {
            Destroy(this.gameObject);
            Debug.Log("Scientist Hit");
        }
    }

}
