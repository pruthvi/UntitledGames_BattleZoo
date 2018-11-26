/*  Copyright (c) Pruthvi  |  http://pruthv.com  */

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    #region Variables

    // Public Variables
    [Header("Player")]
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public bool isGrounded;
    public float jumpForce;
    public float jumpFall;
    public float jumpLowFall;

    public float speed = 2.0f;
    //public float x;

    [Header("Bullet")]
    public GameObject bulletPrefab;   // Bullet prefab
    public float _bulletSpeed = 10.0f;
    public Transform bulletSpawnPoint;

    // Private Variables
    private Rigidbody2D rBody;
    private Animator anim;
    private bool isJump;
    private bool isFacingRight;

    [SerializeField]
    private BarrelRotator barrelRotator; // Getting Refernce of BarrekRotator Script
    #endregion

    void Start ()
	{
        rBody = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        //barrelRotator = gameObject.GetComponent<BarrelRotator>();

    }

    void Update()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        //if (moveHorizontal > 0.0f)
        //{
        //    Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        //    rBody.velocity = movement * speed;

        //    Debug.Log("moveHorizontal : " + movement);

        //    anim.SetBool("IsMovingRight", true);

        //}
        //if(moveHorizontal < 0.0f)
        //{
        //    Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        //    rBody.velocity = movement * speed;

        //    Debug.Log("moveHorizontal : " + movement);

        //    anim.SetBool("IsMovingLeft", true);
        //}
        //else
        //{
        //    anim.SetBool("IsMovingRight", false);
        //    anim.SetBool("IsMovingLeft", false);


        //}

        //if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetButtonDown("Fire1")))
        //{
        //    if(GameManager.Ammo > 0)
        //    {
        //        Vector2 bulletPos = new Vector2(this.transform.position.x + 1f, this.transform.position.y);
        //        Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        //        GameManager.Ammo--;
        //    }

        //}

        // Movement
        float x = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(x * speed, rBody.velocity.y, 0.0f);
        rBody.velocity = move;

        Flip(x);    // flipping the Character


        // Play the Walking animation if player is moving
        if(Mathf.Abs(x) > 0.0f)
        {
            anim.SetFloat("MovingSpeed", Mathf.Abs(x));
            
        }
        else
        {
            anim.SetFloat("MovingSpeed", Mathf.Abs(x));          
        }



        // Jump will only work if the Player is on Ground
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // Shooting
        if (Input.GetButtonDown("Fire1"))
        {
            //Shoot();
        }



        //Flip Side
        //if (x < 0)
        //{
        //    //transform.localRotation = Quaternion.Euler(0, 180, 0);
        //}

        //else
        //{
        //    //transform.localRotation = Quaternion.Euler(0, 0, 0);
        //}
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapPoint(groundCheck.position, whatIsGround);

        // Better Jumping
        if (rBody.velocity.y < 0 && isGrounded)
        {
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (jumpFall - 1) * Time.deltaTime;
        }
        else if (rBody.velocity.y > 0 && !Input.GetKey(KeyCode.K))
        {
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (jumpLowFall - 1) * Time.deltaTime;
        }
    }

    // flipping the Character
    private void Flip(float horizontal)
    {
        if(horizontal > 0 & isFacingRight || horizontal < 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            barrelRotator.isCharacterFlipped = !barrelRotator.isCharacterFlipped;   // flip the Barrel Rotator
        }
    }

}
