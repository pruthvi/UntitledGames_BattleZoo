using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up",menuName ="BattleZoo/Power Up/New PowerUp")]
public class PowerUpScriptableObject : ScriptableObject {

    public string powerUpName ;
    public string description;
    public float enhanceAmount;                     // Boost Multiplier
    public float timeLength;
    public GameObject Object;

}
