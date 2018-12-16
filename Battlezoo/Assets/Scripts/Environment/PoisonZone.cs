using UnityEngine;

public class PoisonZone : MonoBehaviour, IDamageSource
{
    public Transform gasZoneStartPositionLeft;
    public Transform gasZoneStartPositionRight;
    public Vector3 poisonGasEndPosition;

    public float shrinkInterval = 20;
    public float shrinkDuration = 5;
    public float shrinkSpeed = 5;

    [SerializeField]
    private float damagePerSecond = 1;

    public float damageInterval = 1;

    public bool enableDamage;

    public Transform Transform
    {
        get
        {
            return null;
        }
    }

    public GasZoneState GasZoneState = GasZoneState.SafeTimeBegins;

    public float Damage
    {
        get
        {
            return damagePerSecond;
        }
    }

    public string Name
    {
        get
        {
            return "Poison Zone";
        }
    }

    public DamageSourceType DamageSourceType
    {
        get
        {
            return DamageSourceType.PoisonZone;
        }
    }
}