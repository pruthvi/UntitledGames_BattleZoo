using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UntitledGames.Lobby;
public class PlayerStats : NetworkBehaviour, IDamageSource
{
    [Header("Stats")]
    [SyncVar]
    public string playerName;
    [SyncVar]
    public bool isEliminated = false;
    public float maxHP = 100;
    [SyncVar(hook = ("OnHPChanged"))]
    public float currentHP;
    [SyncVar]
    public float Speed = 10;
    [SyncVar]
    public float JumpForce = 10;
    [SyncVar]
    public float DamageMultiplier = 1;
    [SyncVar]
    public float SpeedMultiplier = 1;
    [SyncVar]
    public float JumpMultiplier = 1;

    [Header("Weapon")]
    public int ammoPerMagazine = 30;
    [SyncVar(hook = ("OnAmmoChanged"))]
    public int ammoLeft = 30;
    public float reloadTime = 2;
    [SyncVar]
    public float fireRate = 0.1f; // Fire interval between projectiles
    [SyncVar]
    public float timeUntilNextShot = 0;
    [SyncVar]
    public bool needToReload;
    [SyncVar]
    public bool isReloading;
    public GameObject projectilePrefab;

    public Character character;

    private IDamageSource killer;

    private bool enableCheckVictory;

    public Transform Transform
    {
        get
        {
            return transform;
        }
    }

    public DamageSourceType DamageSourceType
    {
        get
        {
            return DamageSourceType.Player;
        }
    }

    public float Damage
    {
        get
        {
            return 0;
        }
    }

    public string Name
    {
        get
        {
            return playerName;
        }
    }

    void Start()
    {
        if (isServer)
        {
            currentHP = maxHP;
        }
        character.SetNameTag(playerName);
        if (!isLocalPlayer)
        {
            return;
        }
        HUDManager.instance.stats = this;
        HUDManager.instance.UpdatePlayerHP(currentHP);
        HUDManager.instance.UpdatePlayerAmmo(ammoLeft);
    }

    void FixedUpdate()
    {
        if (!isServer)
        {
            return;
        }

        // if (Input.GetKeyUp(KeyCode.Tab))
        // {
        //     foreach (Character c in character.alivePlayers.Values)
        //     {
        //         Debug.Log(c.stats.playerName);
        //     }
        // }
        // Check Winning
        //if (character.alivePlayers.Values.Count == 1)
        //{
        //    CmdShowStats(true);
        //}
        // If player inside gas zone
        if (transform.position.x < GameManager.instance.poisonZone.gasZoneStartPositionLeft.position.x || transform.position.x > GameManager.instance.poisonZone.gasZoneStartPositionRight.position.x)
        {
            if (GameManager.instance.poisonZone.enableDamage)
            {
                // Add a small relative force to show that it is receiving damage
                if (character.rBody != null)
                {
                    if (character.isGrounded)
                    {
                        // No force on x to prevent player AFK and go in to safezone
                        Vector2 direction = new Vector2(0, Random.Range(0, 6));
                        character.rBody.AddRelativeForce(direction, ForceMode2D.Impulse);
                    }
                }
                if (currentHP > 0)
                {
                    OnRecevieDamage(GameManager.instance.poisonZone, GameManager.instance.poisonZone.Damage);
                }

            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            // Get the source of the projectile
            GameObject source = other.gameObject;
            Projectile projectile = source.GetComponent<Projectile>();
            if (projectile != null)
            {
                // Apply damage based on the source
                IDamageSource from = projectile.from;
                //Update the damage dealt data, this can also be used to calculate 'healing on hit'
                //TargetOnDealDamage(from.connectionToClient, projectile.damage);
                //Update the damage taken and also update the new health
                //TargetOnRecevieDamage(connectionToClient, projectile.damage, from.stats.playerName);
                // if (from != null)
                // {
                //     from.OnDealDamage(projectile.Damage);
                // }
                OnRecevieDamage(from, projectile.Damage);
                Destroy(other.gameObject);
                // Let the projectile decied what to do on contact
                // ProjectileAI projectile = other.GetComponent<ProjectileAI>();
                // if(projectile != null)
                // {
                //     projectile.OnContact();
                // }
            }

        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            PowerUp pu = other.gameObject.GetComponent<PowerUp>();
            if (pu != null)
            {
                OnApplyPowerUp(pu.powerUpInfo);
                Destroy(other.gameObject);
            }
        }
    }

    public void OnDealDamage(float damage)
    {
        character.data.totalDamageDealt += damage;
    }

    IEnumerator ShowDamageEffect()
    {
        foreach (SpriteRenderer r in character.spriteRenderers)
        {
            if (r != null)
            {
                r.color = new Color(r.color.r, r.color.g, r.color.b, 0.5f);
            }
        }
        yield return new WaitForSeconds(1);
        foreach (SpriteRenderer r in character.spriteRenderers)
        {
            if (r != null)
            {
                r.color = new Color(r.color.r, r.color.g, r.color.b, 1);
            }
        }
    }

    public void OnRecevieDamage(IDamageSource from, float damage)
    {
        StartCoroutine(ShowDamageEffect());
        if (currentHP > 0)
        {
            character.data.totalDamageTaken += damage;
            currentHP -= damage;
        }
        if (currentHP <= 0)
        {
            OnPlayerEliminated(from);
            //CmdRemovePlayerFromAlivePlayers(connectionToClient.connectionId);
        }
    }



    public void OnPlayerEliminated(IDamageSource from)
    {
        isEliminated = true;
        // Excute on server
        //CmdShowStats();
        if (character.data != null)
        {
            killer = from;
            // Tell the target client to show the game result;
            CmdShowStats(false);
        }
        if (from.DamageSourceType == DamageSourceType.PoisonZone)
        {
            AnnounceMessage(character.stats.playerName + " died outside the safe zone", 3);
        }
        else if (from.DamageSourceType == DamageSourceType.Player)
        {
            AnnounceMessage(from.Name + " eliminated " + character.stats.playerName, 3);
        }
    }

    public void OnApplyPowerUp(PowerUpInfo powerUp)
    {
        if (powerUp != null)
        {

            switch (powerUp.PowerUpType)
            {
                case PowerUpType.Heal:
                    currentHP += powerUp.amount;
                    HUDManager.instance.QuickAnnouncement(powerUp.powerUpDescription, 2, Color.green);
                    break;
                case PowerUpType.Poison:
                    currentHP -= powerUp.amount;
                    HUDManager.instance.QuickAnnouncement(powerUp.powerUpDescription, 2, Color.red);
                    break;
                case PowerUpType.Speed:
                case PowerUpType.Strength:
                case PowerUpType.JumpStrength:
                    StartCoroutine(StartPowerUpCountdown(powerUp));
                    break;
            }
        }
    }

    IEnumerator StartPowerUpCountdown(PowerUpInfo powerUp)
    {
        HUDManager.instance.QuickAnnouncement(powerUp.powerUpDescription, 2, Color.black);
        switch (powerUp.PowerUpType)
        {
            case PowerUpType.Speed:
                if (powerUp.useMultiplier)
                {
                    SpeedMultiplier = powerUp.multiplier;
                }
                else
                {
                    Speed += powerUp.amount;
                }
                break;
            case PowerUpType.Strength:
                DamageMultiplier = powerUp.multiplier;
                break;
            case PowerUpType.JumpStrength:
                JumpMultiplier = powerUp.multiplier;
                break;
        }
        yield return new WaitForSeconds(powerUp.duration);
        switch (powerUp.PowerUpType)
        {
            case PowerUpType.Speed:
                if (powerUp.useMultiplier)
                {
                    SpeedMultiplier = 1;
                }
                else
                {
                    Speed -= powerUp.amount;
                }
                break;
            case PowerUpType.Strength:
                DamageMultiplier = 1;
                break;
            case PowerUpType.JumpStrength:
                JumpMultiplier = 1;
                break;
        }
        HUDManager.instance.QuickAnnouncement(powerUp.powerUpName + " ends", 2, Color.black);
    }


    [Command]
    public void CmdShowStats(bool isVictory)
    {
        TargetShowStats(connectionToClient, isVictory);
    }

    [Command]
    void CmdRemovePlayerFromAlivePlayers(int connectionId)
    {
        RpcRemovePlayerFromAlivePlayers(connectionId);
    }

    [ClientRpc]
    void RpcRemovePlayerFromAlivePlayers(int connectionId)
    {
        if (character.alivePlayers.ContainsKey(connectionId))
        {
            character.alivePlayers.Remove(connectionId);
        }
    }

    [TargetRpc]
    public void TargetShowStats(NetworkConnection conn, bool isVictory)
    {
        if (character.data != null)
        {
            if (!isVictory)
            {
                // Set to null so that camera stays on where player died
                Camera.main.GetComponent<CameraFollow>().target = null;
                CmdDisablePlayer();
            }
            LobbyManager.instance.gameResultPanel.ShowStats(character, isVictory, killer);
        }
    }

    [ClientRpc]
    void RpcDisablePlayer()
    {
        transform.position = new Vector3(-100, -100);
        transform.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    IEnumerator AnnounceMessage(string newMessage, float time)
    {
        RpcUpdateAnnouncement(newMessage);
        yield return new WaitForSeconds(time);
        if (HUDManager.instance.Announcement.text == newMessage)
        {
            RpcUpdateAnnouncement("");
        }
    }

    [ClientRpc]
    public void RpcUpdateAnnouncement(string newMessage)
    {
        HUDManager.instance.Announcement.text = newMessage;
    }

    [Command]
    void CmdDisablePlayer()
    {
        // Move player out of the map on every client
        RpcDisablePlayer();
    }

    // SyncVar call back, used to update HUD
    void OnHPChanged(float newHp)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        HUDManager.instance.UpdatePlayerHP(newHp);
    }

    void OnAmmoChanged(int newAmmo)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        HUDManager.instance.UpdatePlayerAmmo(newAmmo);
    }
}