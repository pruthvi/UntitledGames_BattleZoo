using UnityEngine;
using UnityEngine.Networking;

namespace UntitledGames.Lobby
{
    // This handles set up the player data when they enter the Game
    public class NetworkLobbyHook : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(LobbyManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            LobbyPlayer lobbyP = lobbyPlayer.GetComponent<LobbyPlayer>();
            PlayerStats playerStats = gamePlayer.GetComponent<PlayerStats>();

            playerStats.playerName = lobbyP.playerName;
        }
    }
}

