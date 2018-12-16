using System.Collections;
using UnityEngine;
using UntitledGames.Lobby;
using UnityEngine.Networking;
public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    private LobbyManager lobbyManager;

    public PoisonZone poisonZone;

    void Awake()
    {
        lobbyManager = LobbyManager.instance;
        instance = this;
        poisonZone.enableDamage = true;
    }

    void Update()
    {
        if (lobbyManager.isInGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                lobbyManager.inGameMenuPanel.ToggleVisible();
            }
        }
    }

    void FixedUpdate()
    {
        if (isServer)
        {
            switch (poisonZone.GasZoneState)
            {
                case GasZoneState.SafeTimeBegins:
                    StartCoroutine(SafeTimeEndsCountdown(poisonZone.shrinkInterval));
                    break;
                case GasZoneState.SafeTime:
                    // Game Update such as spawn random power up should be here
                    break;
                case GasZoneState.SafeTimeEnds:
                    poisonZone.GasZoneState = GasZoneState.ShrinkingBegins;
                    break;
                case GasZoneState.ShrinkingBegins:
                    StartCoroutine(ShrinkingEndsCountdown(poisonZone.shrinkDuration));
                    break;
                case GasZoneState.Shrinking:
                    // Where shrinking happens
                    RpcShrinkGasZone(poisonZone.shrinkSpeed);
                    break;
                case GasZoneState.ShrinkingEnds:
                    poisonZone.GasZoneState = GasZoneState.SafeTimeBegins;
                    break;
            }
            // Where damage tick happens
            if (poisonZone.enableDamage)
            {
                StartCoroutine(OnPoisonInterval(poisonZone.damageInterval));
            }
        }
    }

    [ClientRpc]
    void RpcShrinkGasZone(float speed)
    {
        poisonZone.gasZoneStartPositionLeft.position = Vector3.MoveTowards(poisonZone.gasZoneStartPositionLeft.position, poisonZone.poisonGasEndPosition, speed * Time.deltaTime);
        poisonZone.gasZoneStartPositionRight.position = Vector3.MoveTowards(poisonZone.gasZoneStartPositionRight.position, poisonZone.poisonGasEndPosition, speed * Time.deltaTime);
    }
    IEnumerator SafeTimeEndsCountdown(float duration)
    {
        poisonZone.GasZoneState = GasZoneState.SafeTime;
        HUDManager.instance.QuickAnnouncement("Shrinking Begins in " + duration + " seconds", 2, Color.red);
        yield return new WaitForSeconds(duration);
        poisonZone.GasZoneState = GasZoneState.SafeTimeEnds;
    }

    IEnumerator ShrinkingEndsCountdown(float duration)
    {
        HUDManager.instance.QuickAnnouncement("Shrinking Begins", 2, Color.red);
        poisonZone.GasZoneState = GasZoneState.Shrinking;
        yield return new WaitForSeconds(duration);
        HUDManager.instance.QuickAnnouncement("Shrinking Ends", 2, Color.red);
        poisonZone.GasZoneState = GasZoneState.ShrinkingEnds;
    }

    IEnumerator OnPoisonInterval(float duration)
    {
        poisonZone.enableDamage = false;
        yield return new WaitForSeconds(duration);
        poisonZone.enableDamage = true;
    }
}