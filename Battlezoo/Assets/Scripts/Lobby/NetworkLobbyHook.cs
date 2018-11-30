using UnityEngine;
using UnityEngine.Networking;

namespace UntitledGames.Lobby
{
    // This handles set up the player data when they enter the Game
    public class NetworkLobbyHook : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            LobbyPlayer lobbyP = lobbyPlayer.GetComponent<LobbyPlayer>();
            SetupLocalPlayer localPlayer = gamePlayer.GetComponent<SetupLocalPlayer>();

            localPlayer.playerName = lobbyP.playerName;
            localPlayer.characterIndex = lobbyP.characterIndex;
        }
    }
}

