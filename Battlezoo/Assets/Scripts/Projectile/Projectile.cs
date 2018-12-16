using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour, IDamageSource
{

    public enum TravelMode { TimeBased, DistanceBased }
    public enum OnHit { Destory, Pass, Bounce }

    public TravelMode travelMode;
    public OnHit onHit;

    public float maxTimeTravel = 2;
    public float maxDistanceToTravel = 10;
    private Vector2 startPoint;

    public IDamageSource from;

    [SerializeField]
    private float damage;
    public float speed = 50;

    public float Damage
    {
        get
        {
            return this.damage;
        }
        set
        {
            if (value >= 0)
            {
                damage = value;
            }
        }
    }

    private string weaponName;
    public string Name
    {
        get
        {
            return weaponName;
        }
    }

    public Transform Transform
    {
        get
        {
            return from.Transform;
        }
    }

    public DamageSourceType DamageSourceType
    {
        get
        {
            return DamageSourceType.Player;
        }
    }

    void Start()
    {
        switch (travelMode)
        {
            case TravelMode.TimeBased:
                Destroy(gameObject, maxTimeTravel);
                break;
            case TravelMode.DistanceBased:
                startPoint = transform.position;
                break;
        }
    }

    public void Initialize(Vector3 dir, string weaponName, float damage, float damageMultiplier, IDamageSource damageSource)
    {
        GetComponent<Rigidbody2D>().velocity = dir * speed;
        from = damageSource;
        this.damage = damage;
        damage *= damageMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if (travelMode == TravelMode.DistanceBased)
        {
            if (Vector2.Distance(transform.position, startPoint) >= maxDistanceToTravel)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Boundary"))
        {
            Destroy(this.gameObject);
        }
    }

    public void OnContact()
    {
        switch (onHit)
        {
            case OnHit.Destory:
                Destroy(gameObject);
                break;
            case OnHit.Bounce:
                // Reset the startPoint so that if bullet bounce off object it will only travel the maxDistanceToTravel
                startPoint = transform.position;
                GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity;
                break;
        }
    }
}
