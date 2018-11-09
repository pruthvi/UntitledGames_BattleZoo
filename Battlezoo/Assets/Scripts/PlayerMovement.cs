/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region Variables

    public float speed = 2.0f;
    private Rigidbody2D rBody;

    public GameObject bullet;

    private Animator anim;


    #endregion

    void Start ()
	{
        rBody = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveHorizontal > 0.0f)
        {
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            rBody.velocity = movement * speed;

            Debug.Log("moveHorizontal : " + movement);

            anim.SetBool("IsMovingRight", true);

        }
        if(moveHorizontal < 0.0f)
        {
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            rBody.velocity = movement * speed;

            Debug.Log("moveHorizontal : " + movement);

            anim.SetBool("IsMovingLeft", true);
        }
        else
        {
            anim.SetBool("IsMovingRight", false);
            anim.SetBool("IsMovingLeft", false);


        }

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
