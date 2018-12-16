using UnityEngine;
using UnityEngine.UI;

namespace UntitledGames.Lobby.Menu
{
    public class LobbyTopMenuPanel : LobbyMenuPanel
    {
        public Button backButton;

        protected override void Start()
        {
            base.Start();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(BackToPreviousPanel);
        }
    }
}
