using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UntitledGames.Lobby.Menu
{
    public class LobbyInfoPanel : MonoBehaviour
    {
        public Text infoText;
        public Text buttonText;
        public Button button;

        public void Display(string info, string buttonInfo, UnityAction buttonClbk)
        {
            infoText.text = info;

            buttonText.text = buttonInfo;

            button.onClick.RemoveAllListeners();

            // Add call back to the button
            if (buttonClbk != null)
            {
                button.onClick.AddListener(buttonClbk);
            }

            // This listener set the active state of the info panel to false when player clicks
            button.onClick.AddListener(() => { gameObject.SetActive(false); });

            // Show the panel
            gameObject.SetActive(true);
        }
    }
}
