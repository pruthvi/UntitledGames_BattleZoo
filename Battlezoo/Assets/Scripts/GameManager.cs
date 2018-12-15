using UnityEngine;
using UntitledGames.Lobby;
using UnityEngine.Networking;
public class GameManager : NetworkBehaviour
{
    private LobbyManager lobbyManager;

    public GameObject powerUp;

    public Transform[] powerUpPosition;

    public float spawnRate = 0;
    private float nextSpawnTime;

    void Start()
    {
        lobbyManager = LobbyManager.instance;
        DontDestroyOnLoad(gameObject);
    }

    void SpawnPowerUp()
    {
        if (!isServer)
        {
            return;
        }
        if (powerUpPosition.Length > 0)
        {
            Vector3 randomPos = getRandomPosition();
            GameObject pu = Instantiate(powerUp, getRandomPosition(), Quaternion.identity) as GameObject;
            NetworkServer.Spawn(pu);
            Destroy(pu, spawnRate);
        }

    }

    Vector3 getRandomPosition()
    {
        return powerUpPosition[Random.Range(0, powerUpPosition.Length)].position;
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
}