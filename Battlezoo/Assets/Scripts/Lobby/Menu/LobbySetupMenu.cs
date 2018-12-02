using UnityEngine;
using UnityEngine.UI;

namespace UntitledGames.Lobby.Menu
{
    public class LobbySetupMenu : LobbyMenuPanel
    {
        public InputField ipAddressInput;
        
        public LobbyCharacterSelectionMenu characterSelectionPanel;

        public void OnClickJoin()
        {
            lobbyManager.networkAddress = ipAddressInput.text;

            lobbyManager.StartClient();

            lobbyManager.SwitchPanel(characterSelectionPanel);

            lobbyManager.backDelegate = lobbyManager.StopClientCallback;
            
            lobbyManager.DisplayIsConnecting();
        }

        public void OnClickHost()
        {
            lobbyManager.StartHost();
        }
    }
}
