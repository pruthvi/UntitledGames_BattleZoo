using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UntitledGames.Lobby.Menu
{
    public class LobbyMainMenu : LobbyMenuPanel
    {
        public Button StartButton;
        public Button SettingButton;
        public Button ExitButton;

        public LobbySetupMenu SetUpPanel;
        public LobbySettingsMenu SettingsPanel;

        public void OnStartClicked()
        {
            lobbyManager.SwitchPanel(SetUpPanel);
        }

        public void OnSettingsClicked()
        {
            lobbyManager.SwitchPanel(SettingsPanel);
        }

        public void OnExitClicked()
        {
            Application.Quit();
        }
    }
}
