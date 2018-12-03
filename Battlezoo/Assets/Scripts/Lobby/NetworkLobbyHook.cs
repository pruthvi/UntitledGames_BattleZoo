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
            PlayerConnection playerConnection = gamePlayer.GetComponent<PlayerConnection>();

            playerConnection.playerName = lobbyP.playerName;
            playerConnection.characterIndex = lobbyP.characterIndex;
            lobbyP.connectionToClient.isReady = true;
        }
    }
}

