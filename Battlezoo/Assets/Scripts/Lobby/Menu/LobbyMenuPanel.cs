using UnityEngine;

namespace UntitledGames.Lobby.Menu
{
    // Parent class for all the lobby page
    public abstract class LobbyMenuPanel : MonoBehaviour
    {
        protected LobbyManager lobbyManager;

        public LobbyMenuPanel previousPanel;

        protected virtual void Start()
        {
            lobbyManager = LobbyManager.instance;
        }

        public void BackToPreviousPanel()
        {
            if (previousPanel != null)
            {
                if (previousPanel == lobbyManager.setupPanel)
                {
                    lobbyManager.BackToSetup();
                    return;
                }
                lobbyManager.SwitchPanel(previousPanel);
            }
        }
    }
}
