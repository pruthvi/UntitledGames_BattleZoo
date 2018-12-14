using UnityEngine;
using UnityEngine.UI;

namespace UntitledGames.Lobby.Menu
{
    public class InGameMenuPanel : MonoBehaviour
    {
        [Header("UI Reference")]
        public Button btnSettings;
        public Button btnQuitGame;

        void Start()
        {
            btnSettings.onClick.RemoveAllListeners();
            // TODO Add Settings Listener;
            
            // Add button listener to QuitGame button
            btnQuitGame.onClick.RemoveAllListeners();
            btnQuitGame.onClick.AddListener(LobbyManager.instance.QuitGame);
        }
        
        // Toggle the visibility of the panel
        public void ToggleVisible(){
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }
}
