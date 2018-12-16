using UnityEngine;
using UnityEngine.UI;

namespace UntitledGames.Lobby.Menu
{
    public class LobbyCharacterSelectionMenu : LobbyMenuPanel
    {
        public InputField playerNameInputField;
        public Button readyButton;

        public CountdownPanel countdownPanel;

        public Button[] characterButtons;
        public CharacterSelectionInfo[] characters;

        protected override void Start()
        {
            base.Start();
            for (int i = 0; i < characters.Length; i++)
            {
                characterButtons[i].GetComponent<Button>().transform.GetChild(0).GetComponent<Text>().text = characters[i].characterName;
                characterButtons[i].GetComponent<Image>().sprite = characters[i].characterSelectionIcon;
            }
        }

        public void LockInCharacterSelection(bool lockIn)
        {
            readyButton.transform.GetChild(0).GetComponent<Text>().text = lockIn ? "Cancel" : "Ready";
            for (int i = 0; i < characterButtons.Length; i++)
            {
                characterButtons[i].interactable = !lockIn;
            }
            playerNameInputField.interactable = !lockIn;
            lobbyManager.topMenuPanel.gameObject.SetActive(!lockIn);
        }

        public void CancelMatch()
        {
            readyButton.transform.GetChild(0).GetComponent<Text>().text = "Ready";
            for (int i = 0; i < characterButtons.Length; i++)
            {
                characterButtons[i].interactable = true;
            }
            playerNameInputField.interactable = true;
            countdownPanel.gameObject.SetActive(false);
        }

        // Reset the UI
        public void ResetControls()
        {
            readyButton.transform.GetChild(0).GetComponent<Text>().text = "Ready";
            playerNameInputField.interactable = true;
            playerNameInputField.text = "";
            // Make the buttons not interactable instead of disable the whole character selection panel
            for (int i = 0; i < characterButtons.Length; i++)
            {
                characterButtons[i].interactable = true;
            }
            countdownPanel.gameObject.SetActive(false);
        }
    }
}
