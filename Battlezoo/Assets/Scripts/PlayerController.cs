using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    [Header("Player Info")]
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public bool isGrounded;
    public float jumpForce;
    public float jumpFall;
    public float jumpLowFall;
    
    public float speed = 10.0f;

    public Weapon weapon;

    // Private Variables
    private Rigidbody2D rBody;
    private Animator anim;
    private bool isJump;
    [SyncVar(hook= "OnCharacterFacing")]
    private bool isFacingRight;

    private BarrelRotator barrelRotator;

    public Transform character;

    // Use this for initialization
    void Start () {
        character = transform.GetChild(0);
        rBody = GetComponent<Rigidbody2D>();
        weapon = character.GetComponent<Weapon>();
        barrelRotator = character.GetComponent<BarrelRotator>();
        groundCheck = character.GetChild(0).transform;
        anim = character.GetComponent<Animator>();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        // Movement
        float x = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(x * speed, rBody.velocity.y, 0.0f);
        rBody.velocity = move;

        Flip(x);    // flipping the Character

        // Play the Walking animation if player is moving
        if (Mathf.Abs(x) > 0.0f)
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

        if (Input.GetButton("Fire1"))
        {
            CmdFire();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
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

    private void Flip(float horizontal)
    {
        bool currentFacingRight = isFacingRight;
        if (currentFacingRight != horizontal < 0)
        {
            OnFacingChanged(horizontal < 0);
        }

       
        //if (horizontal > 0 & isFacingRight || horizontal < 0 && !isFacingRight)
        //{
        //    isFacingRight = !isFacingRight;

        //    OnCharacterFacingChanged(isFacingRight);
        //}
    }

    // Local player changed direction, send request to server
    public void OnFacingChanged(bool facingRight)
    {
        CmdChangeFacing(facingRight);
    }

    // Server change the value and sync the new facing
    [Command]
    void CmdChangeFacing(bool newFacing)
    {
        isFacingRight = newFacing;
    }

    // Client gets the new facing and update
    /* Call back */
    public void OnCharacterFacing(bool newFacing)
    {
        isFacingRight = newFacing;

        Vector3 theScale = transform.localScale;
        theScale.x *= isFacingRight ? -1 : 1;
        transform.localScale = theScale;
        barrelRotator.isCharacterFlipped = isFacingRight;   // flip the Barrel Rotator
    }

   

    [Command]
    void CmdFire()
    {
        // Get Direction
        Vector2 direction = weapon.muzzle.position - weapon.barrel.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        var objBullet = Instantiate(weapon.bullet, weapon.muzzle.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;

        //switch (travelMode)
        //{
        //    case BulletTravelMode.Linear:
        objBullet.GetComponent<Rigidbody2D>().velocity = direction * weapon.bulletController.speed;
       //         break;
        //    case BulletTravelMode.WithForce:
        //        bulletController.ApplyForce(direction * weapon.bulletController.speed, 3);
        //        break;
        //}
        NetworkServer.Spawn(objBullet);
    }
}
