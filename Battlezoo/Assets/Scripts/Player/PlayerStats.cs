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

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Projectile"))
    //     {
    //         // Get the source of the projectile
    //         GameObject source = other.gameObject;
    //         ProjectileAI projectile = source.GetComponent<ProjectileAI>();
    //         if(projectile != null)
    //         {
    //             // Apply damage based on the source
    //             Character from = projectile.from;
    //             //Update the damage dealt data, this can also be used to calculate 'healing on hit'
    //             TargetOnDealDamage(from.connectionToClient, projectile.damage);
    //             //Update the damage taken and also update the new health
    //             TargetOnRecevieDamage(connectionToClient, projectile.damage, from.stats.playerName);
    //             Destroy(other);
    //             // Let the projectile decied what to do on contact
    //             // ProjectileAI projectile = other.GetComponent<ProjectileAI>();
    //             // if(projectile != null)
    //             // {
    //             //     projectile.OnContact();
    //             // }
    //         }
            
    //     }
    // }

    
    public void OnDealDamage(Character to, float damage){
        character.data.totalDamageDealt += damage;
    }

    public void OnRecevieDamage(Character from, float damage){
        CmdOnRecevieDamage(from.stats.playerName, damage);
    }

    [TargetRpc]
    public void TargetOnDealDamage(NetworkConnection conn, float damage)
    {
        CmdOnDealDamage(damage);
    }

    [Command]
    public void CmdOnDealDamage(float damage){
        character.data.totalDamageDealt += damage;
    }

    [TargetRpc]
    public void TargetOnRecevieDamage(NetworkConnection conn, float damage, string from)
    {
     //   CmdOnRecevieDamage(damage);
    }

    [Command]
    public void CmdOnRecevieDamage(string from, float damage){
        character.data.totalDamageTaken -= damage;
        currentHP -= damage;
        if (currentHP <= 0)
        {
            OnPlayerEliminated(from);
        }
    }

    public void OnPlayerEliminated(string name)
    {
        isEliminated = true;
        // Excute on server
        CmdShowStats();
        StartCoroutine(AnnounceMessage(name + " eliminated " + character.stats.playerName, 3));
        //HUDManager.instance.UpdateSpectatingUI(c.stats.playerName);
        //Camera.main.GetComponent<CameraFollow>().target = c.transform;
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