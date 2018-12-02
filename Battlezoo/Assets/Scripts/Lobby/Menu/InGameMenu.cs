using UnityEngine;
using UnityEngine.UI;

namespace UntitledGames.Lobby.Menu
{
    public class InGameMenu : LobbyMenuPanel
    {
        public Button backButton;
        public bool isInGame;

        protected override void Start()
        {
            base.Start();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(BackToPreviousPanel);
        }

        void OnEnable()
        {
        }
    }
}
