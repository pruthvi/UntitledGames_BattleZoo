using UnityEngine;
using UntitledGames.Lobby;

public class GameManager : MonoBehaviour
{

    private LobbyManager lobbyManager;

    void Start()
    {
        lobbyManager = LobbyManager.instance;
        DontDestroyOnLoad(gameObject);
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