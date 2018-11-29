using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UntitledGames.Lobby
{
    public class LobbyMainMenu : MonoBehaviour
    {
        public LobbyManager lobbyManager;

        public RectTransform lobbyPanel;

        public InputField ipInput;
        public InputField matchNameInput;

        public Button readyButton;

        public void OnClickJoin()
        {
            lobbyManager.SwitchPanel(lobbyPanel);
            
            lobbyManager.networkAddress = ipInput.text;
            lobbyManager.StartClient();

            //lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            //lobbyManager.DisplayIsConnecting();

            //lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
        }

        public void OnClickHost()
        {
            lobbyManager.StartHost();
        }
    }
}
