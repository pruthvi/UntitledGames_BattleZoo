/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region Variables

    public float speed = 2.0f;
    private Rigidbody2D rBody;

    public GameObject bullet;



    #endregion

    void Start ()
	{
        rBody = this.GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rBody.velocity = movement * speed;


        if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetButtonDown("Fire1")))
        {
            if(GameManager.Ammo > 0)
            {
                Vector2 bulletPos = new Vector2(this.transform.position.x + 1f, this.transform.position.y);
                Instantiate(bullet, this.transform.position, this.transform.rotation);
                GameManager.Ammo--;
            }

               


        }
            


	}

}
