using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UntitledGames.Lobby;

public class Character : NetworkBehaviour
{
    public enum FaceDirection { Left = -1, Right = 1 };

    [Header("Player Info")]
    public Transform characterSprites;
    public LayerMask whatIsGround;
    public bool isGrounded;
    private Rigidbody2D rBody;

    [Header("Character Info")]
    public float Speed = 10;
    public float JumpForce = 10;
    public FaceDirection InitialFacing = FaceDirection.Right;
    [SyncVar(hook = ("OnDirectionChanged"))]
    public int Direction;
    private Transform ceilingCheck;
    private Transform groundCheck;

    [Header("Weapon")]
    public int ammoPerMagazine = 30;
    [SyncVar]
    public int ammoLeft = 30;
    public float reloadTime = 2;
    [SyncVar]
    public float fireRate = 0.1f; // Fire interval between bullets
    [SyncVar]
    public float timeUntilNextShot = 0;
    public float bulletSpeed = 20;
    public GameObject bulletPrefab;

    public Transform barrel;
    public Transform muzzle;
    [SyncVar(hook = ("OnBarrelAngleChanged"))]
    public float barrelAngle;
    public float maxBarrelAngle;
    public float minBarrelAngle;
    [SyncVar]
    public bool needToReload;
    [SyncVar]
    public bool isReloading;

    [Header("HUD")]
    public Text nameTag;
    public Text announcementText;
    // [SyncVar(hook = ("OnAnnouncementChanged"))]
    [SyncVar]
    public string announcement;
    [SyncVar]
    public bool newMessage;

    private Transform mainCamera;

    private Vector3 offset = new Vector3(0, 0, -1);

    public string playerName;

    [HideInInspector]
    public PlayerConnection connection;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        groundCheck = transform.GetChild(0).GetChild(0);
        whatIsGround = LayerMask.GetMask("Platform");
        Direction = (int)InitialFacing;

        if (barrel == null)
        {
            barrel = transform.GetChild(transform.childCount - 2);
            if (muzzle == null)
            {
                muzzle = barrel.transform.GetChild(0);
            }
        }

        nameTag.text = playerName;

        announcementText = GameObject.FindGameObjectWithTag("Announcement").GetComponent<Text>();

        mainCamera = Camera.main.transform;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        // Disable self name tag
        nameTag.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        mainCamera.position = transform.position + offset;

        // Movement
        float movementX = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        transform.Translate(movementX, 0, 0);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rBody.AddForce(JumpForce * Vector2.up, ForceMode2D.Impulse);
            isGrounded = false;
        }

        

        // Reload
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (needToReload && !isReloading)
            {
                CmdReload();
            }
        }

        // Update the facing direction
        UpdateDirectionByMousePosition();
        // Update the barrel rotation
        RotateBarrelByMousePosition();

        // Update the shot interval
        if (timeUntilNextShot > 0)
        {
            timeUntilNextShot -= Time.deltaTime;
        }
        

        // Fire
        if (Input.GetMouseButton(0))
        {
            CmdFire();
        }

    }

    void FixedUpdate()
    {
        if (!hasAuthority)
        {
            return;
        }
        //isGrounded = Physics.Linecast(groundCheck.position, groundCheck.position + Vector3.down, 1);
        isGrounded = Physics2D.OverlapPoint(groundCheck.position, whatIsGround);
    }

    // Weapon Related
    
    // Firing

    [Command]
    void CmdFire()
    {
        if (timeUntilNextShot <= 0 && !needToReload && ammoLeft > 0)
        {
            Debug.Log("Spawning Bullet");
            ammoLeft--;
            timeUntilNextShot = fireRate;
            if (ammoLeft <= 0)
            {
                Debug.Log("Ammo Less than 1");
                needToReload = true;
            }
            // the direction of the barrel from muzzle to barrell
            Vector3 bulletDirection = (muzzle.transform.position - barrel.transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.AngleAxis(barrelAngle, Vector3.forward));
            // set the velocity of the bullet, the server does not have to track the bullet position it will be calcuated on the client
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
            NetworkServer.Spawn(bullet);
        }
    }

    // Announcement

    [ClientRpc]
    void RpcBroadcastAll(string message, float time)
    {
        StartCoroutine(BroadcastWithTime(message, time));
        newMessage = true; // Set this after so that 
    }

    IEnumerator BroadcastWithTime(string message, float time)
    {
        // Check if server announce new message
        if (newMessage && announcement != message)
        {
            newMessage = false;
            yield break;
        }
        announcement = message;
        announcementText.text = announcement;
        yield return new WaitForSeconds(time);
        announcement = "";
        announcementText.text = announcement;
    }

    //void OnAnnouncementChanged(string message)
    //{
    //    newMessage = true;
        
    //}

    // =============

    // Reloading

    [Command]
    void CmdReload()
    {
        StartCoroutine("ReloadBullet");
    }

    IEnumerator ReloadBullet()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammoLeft = ammoPerMagazine;
        isReloading = false;
        needToReload = false;
    }

    // ===========

    // Barrel Angle

    void OnBarrelAngleChanged(float newAngle)
    {
        barrelAngle = newAngle;
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, barrelAngle));
        barrel.transform.rotation = Direction == -1 ? Quaternion.Inverse(rotation) : rotation;
    }

    void OnChangingBarrelAngle(float newAngle)
    {
        CmdChangeBarrelAngle(newAngle);
    }

    [Command]
    void CmdChangeBarrelAngle(float newAngle)
    {
        barrelAngle = newAngle;
    }

    // ============

    // Player Direction

    // Call back for SyncVar - Direction
    // This will change the local property when Sync from the server
    void OnDirectionChanged(int newDir)
    {
        Direction = newDir;
        characterSprites.localScale = new Vector3(newDir, transform.localScale.y, transform.localScale.z);
    }
    // Called when change value
    void OnChangingDirection(int newDir)
    {
        CmdChangeDirecion(newDir);
    }
    // Notice the server
    [Command]
    void CmdChangeDirecion(int newDir)
    {
        //  Debug.Log("CmdChangeDirecion | " + Direction);
        Direction = newDir;
    }

    // ============

    void OnGUI()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        GUI.color = Color.red;
        GUI.Label(new Rect(10, 50, 200, 30), "is Reloading: " + isReloading);
        GUI.Label(new Rect(10, 70, 200, 30), "Ammo: " + ammoLeft);
        GUI.Label(new Rect(10, 90, 200, 30), "Time Until next shot : " + timeUntilNextShot);
        GUI.Label(new Rect(10, 110, 200, 30), "need to reload : " + needToReload);
    }

    // Draw the firing angle on the scene view
    void OnDrawGizmos()
    {
     //   Debug.DrawLine(barrel.transform.position, barrel.transform.position + (new Vector3(Mathf.Cos(minBarrelAngle * Mathf.Deg2Rad), Mathf.Sin(minBarrelAngle * Mathf.Deg2Rad))) * 5 * Direction);
     //   Debug.DrawLine(barrel.transform.position, barrel.transform.position + (new Vector3(Mathf.Cos(maxBarrelAngle * Mathf.Deg2Rad), Mathf.Sin(maxBarrelAngle * Mathf.Deg2Rad))) * 5 * Direction);
    }

    // ============ Helper Functions

    /// <summary>
    /// Update the player direction based on the mouse position
    /// which is relative to the player position rather than middle of the screen
    /// </summary>
    void UpdateDirectionByMousePosition()
    {
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;

        int newDirection = 0;
        if (x >= 0)
        {
            newDirection = 1;
        }
        else
        {
            newDirection = -1;
        }
        // Only Sync if player change direction
        if (newDirection != Direction)
        {
            OnChangingDirection(newDirection);
        }
    }

    void RotateBarrelByMousePosition()
    {
        // Direction from barrel(pivot point) to mouse
        Vector2 dirTowardsMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - barrel.transform.position;
        // Angle from barrel to mouse
        float newAngle = Mathf.Atan2(dirTowardsMouse.y, (Direction) * dirTowardsMouse.x) * Mathf.Rad2Deg;

        // If angle changed
        if (newAngle != barrelAngle)
        {
            // If angle is within the max and min angle
            if (newAngle > minBarrelAngle && newAngle < maxBarrelAngle)
            {
                OnChangingBarrelAngle(newAngle);
            }
        }
    }

    // ============
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            Destroy(gameObject);
        }
        
    }
}