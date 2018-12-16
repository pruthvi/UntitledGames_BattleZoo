using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UntitledGames.Lobby;
using System.Collections.Generic;
public class Character : NetworkBehaviour
{
    public enum FaceDirection { Left = -1, Right = 1 };

    [Header("Player Info")]
    public Transform characterSprites;
    public bool isGrounded;
    public Rigidbody2D rBody;
    public FaceDirection InitialFacing = FaceDirection.Right;
    [SyncVar(hook = ("OnDirectionChanged"))]
    public int Direction;
    private Transform ceilingCheck;
    private Transform groundCheck;

    public Transform barrel;
    public Transform muzzle;
    [SyncVar(hook = ("OnBarrelAngleChanged"))]
    public float barrelAngle;
    public float maxBarrelAngle;
    public float minBarrelAngle;

    [Header("HUD")]
    public Text nameTag;

    public LayerMask whatIsGround;

    public PlayerStats stats;
    public PlayerData data;

    public SpriteRenderer[] spriteRenderers;

    public Dictionary<int, Character> alivePlayers;

    public int NumberOfPlayers
    {
        get
        {
            return LobbyManager.instance._playerNumber;
        }
    }

    void Start()
    {
        // if (isServer)
        // {
        //     alivePlayers = new Dictionary<int, Character>();
        //     alivePlayers.Add(connectionToClient.connectionId, this);
        // }
        rBody = GetComponent<Rigidbody2D>();
        groundCheck = transform.GetChild(0).GetChild(0).transform;
        Direction = (int)InitialFacing;

        if (barrel == null)
        {
            barrel = transform.GetChild(transform.childCount - 2);
            if (muzzle == null)
            {
                muzzle = barrel.transform.GetChild(0);
            }
        }

        if (isLocalPlayer)
        {
            // Disable self name tag
            nameTag.gameObject.SetActive(false);
            Camera.main.GetComponent<CameraFollow>().target = gameObject.transform;
        }
    }

    public void SetNameTag(string name)
    {
        nameTag.text = name;
    }

    void FixedUpdate()
    {
        if (isServer)
        {
            // Fire interval check has to be done in server otherwise the time on client will not match
            if (stats.timeUntilNextShot >= 0)
            {
                stats.timeUntilNextShot -= Time.deltaTime;
            }
        }
        if (!isLocalPlayer || stats.isEliminated)
        {
            return;
        }

        // Movement
        float movementX = Input.GetAxis("Horizontal") * stats.Speed * stats.SpeedMultiplier * Time.deltaTime;
        transform.Translate(movementX, 0, 0);
        if (movementX > 0)
        {
            data.totalDistanceTravelled += movementX;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rBody.AddForce(stats.JumpForce * stats.JumpMultiplier * Vector2.up, ForceMode2D.Impulse);
        }

        // Reload
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (stats.needToReload && !stats.isReloading)
            {
                CmdReload();
            }
        }


        // Fire
        if (Input.GetButton("Fire1"))
        {
            CmdFire();
        }

        // Update the facing direction
        UpdateDirectionByMousePosition();
        // Update the barrel rotation
        RotateBarrelByMousePosition();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 1f, whatIsGround);
    }
    // Weapon Related

    // Firing

    [Command]
    void CmdFire()
    {
        if (stats.timeUntilNextShot <= 0 && !stats.needToReload && stats.ammoLeft > 0)
        {
            stats.ammoLeft--;
            stats.timeUntilNextShot = stats.fireRate;
            if (stats.ammoLeft <= 0)
            {
                stats.needToReload = true;
            }
            // the direction of the barrel from muzzle to barrell
            Vector3 projectileDirection = (muzzle.transform.position - barrel.transform.position).normalized;

            var projectile = (GameObject)Instantiate(stats.projectilePrefab, muzzle.transform.position, Quaternion.AngleAxis(barrelAngle, Vector3.forward));
            Projectile p = projectile.GetComponent<Projectile>();
            // set the velocity of the projectile, the server does not have to track the projectile position it will be calcuated on the client
            p.Initialize(projectileDirection, stats.playerName, p.Damage, stats.DamageMultiplier, stats);
            NetworkServer.Spawn(projectile);
        }
    }
    // =============

    // Reloading

    [Command]
    void CmdReload()
    {
        StartCoroutine("Reloadprojectile");
    }

    IEnumerator Reloadprojectile()
    {
        stats.isReloading = true;
        HUDManager.instance.ShowReloading();
        yield return new WaitForSeconds(stats.reloadTime);
        stats.ammoLeft = stats.ammoPerMagazine;
        stats.isReloading = false;
        stats.needToReload = false;
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
}