using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour
{
    [Header("Statistics")]
    [SyncVar]
    public float totalDamageDealt = 0;
    [SyncVar]
    public float totalDamageTaken = 0;
    [SyncVar]
    public float totalPlayerEliminated = 0;
    [SyncVar]
    public float totalNPCEliminated = 0;
    [SyncVar]
    public float totalDistanceTravelled = 0;
    [SyncVar]
    public float totalScore = 0;
    //public float timeSurvived = 0;

    void Start(){

    }
}