using UnityEngine;
using UnityEngine.Networking;

public class ProjectileAI : MonoBehaviour
{

    public enum TravelMode { TimeBased, DistanceBased }
    public enum OnHit { Destory, Pass, Bounce }

    public TravelMode travelMode;
    public OnHit onHit;

    public float maxTimeTravel = 2;
    public float maxDistanceToTravel = 10;
    private Vector2 startPoint;

    public Character from;

    public float damage;
    public float speed = 50;

    // Use this for initialization
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

    public void Initialize(Vector3 dir, Character fromPlayer)
    {
        GetComponent<Rigidbody2D>().velocity = dir * speed;
        from = fromPlayer;
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Boundary"))
        {
            Destroy(this.gameObject);
            return;
        }
        if(other.gameObject.tag == "Player")
        {
            Character character = other.gameObject.GetComponent<Character>();
            if (character != null)
            {
                // if (character.connectionToClient.connectionId == from.connectionToClient.connectionId)
                // {
                //     return;
                // }
                //character.stats.OnDealDamage(character, damage);
                character.stats.OnRecevieDamage(from, damage);
                damage = 0;
            }
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
