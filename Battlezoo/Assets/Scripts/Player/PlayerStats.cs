using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UntitledGames.Lobby;
public class PlayerStats : NetworkBehaviour
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

    private Character killer;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            // Get the source of the projectile
            GameObject source = other.gameObject;
            Projectile projectile = source.GetComponent<Projectile>();
            if (projectile != null)
            {
                if (projectile.connectionToClient.connectionId == connectionToClient.connectionId)
                {
                    return;
                }
                // Apply damage based on the source
                Character from = projectile.from;
                //Update the damage dealt data, this can also be used to calculate 'healing on hit'
                //TargetOnDealDamage(from.connectionToClient, projectile.damage);
                //Update the damage taken and also update the new health
                //TargetOnRecevieDamage(connectionToClient, projectile.damage, from.stats.playerName);
                if (from != null)
                {
                    from.stats.OnDealDamage(projectile.damage);
                }
                OnRecevieDamage(from, projectile.damage);
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

    public void OnRecevieDamage(Character from, float damage)
    {
        character.data.totalDamageTaken += damage;
        currentHP -= damage;
        if (currentHP <= 0)
        {
            OnPlayerEliminated(from);
        }
        //CmdOnRecevieDamage(from.stats.playerName, damage);
    }

    public void OnPlayerEliminated(Character from)
    {
        isEliminated = true;
        // Excute on server
        //CmdShowStats();
        if (character.data != null)
        {
            killer = from;
            // Tell the target client to show the game result;
            CmdShowStats();
        }
        StartCoroutine(AnnounceMessage(from.stats.playerName + " eliminated " + character.stats.playerName, 3));
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
    public void CmdShowStats()
    {
        TargetShowStats(connectionToClient);
    }

    [TargetRpc]
    public void TargetShowStats(NetworkConnection conn)
    {
        if (character.data != null)
        {
            // Set to null so that camera stays on where player died
            Camera.main.GetComponent<CameraFollow>().target = null;
            LobbyManager.instance.gameResultPanel.ShowStats(character, false, killer);
            CmdDisablePlayer();
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