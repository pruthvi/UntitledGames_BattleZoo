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

    void Start()
    {
        character.SetNameTag(playerName);
        if (!isLocalPlayer)
        {
            return;
        }
        currentHP = maxHP;
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
            // Apply damage based on the source
            OnRecevieDamage(source);
            Destroy(other);
            // Let the projectile decied what to do on contact
            // ProjectileAI projectile = other.GetComponent<ProjectileAI>();
            // if(projectile != null)
            // {
            //     projectile.OnContact();
            // }
        }
    }

    public void OnRecevieDamage(GameObject source)
    {
        // If the source is projectile
        ProjectileAI projectile = source.GetComponent<ProjectileAI>();
        if (projectile != null)
        {
            // If the projectile is 'from' player(which has character script)
            Character c = projectile.from;
            if (c != null)
            {
                // Check if projectile is from self, if yes do nothing(this is temporarily)
                if (c.connectionToClient.connectionId == connectionToClient.connectionId)
                {
                    return;
                }
                currentHP -= projectile.damage;
                c.data.totalDamageDealt += projectile.damage;
                character.data.totalDamageTaken += projectile.damage;
                if (currentHP <= 0)
                {
                    // ProjectileAI p = source.GetComponent<ProjectileAI>();
                    // if(p != null)
                    // {

                    // }
                    // Camera.main.GetComponent<CameraFollow>().target = .from.transform;
                    OnPlayerEliminated(projectile.from);
                }
            }
        }

    }

    public void OnPlayerEliminated(Character c)
    {
        isEliminated = true;
        // Excute on server
        CmdShowStats();
        StartCoroutine(AnnounceMessage(c.stats.playerName + " eliminated " + GetComponent<PlayerStats>().playerName, 3));
        //HUDManager.instance.UpdateSpectatingUI(c.stats.playerName);
        Camera.main.GetComponent<CameraFollow>().target = c.transform;
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
            LobbyManager.instance.gameResultPanel.ShowStats(character, false);
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
        // Move player out of the map
        RpcDisablePlayer();
    }

    // SyncVar call back, used to update HUD
    void OnHPChanged(float newHp)
    {
        if(!isLocalPlayer)
        {
            return;
        }
        HUDManager.instance.UpdatePlayerHP(newHp);
    }

    void OnAmmoChanged(int newAmmo)
    {
        if(!isLocalPlayer)
        {
            return;
        }
        HUDManager.instance.UpdatePlayerAmmo(newAmmo);
    }
}