using UnityEngine;
using UnityEngine.UI;

namespace UntitledGames.Lobby.Menu
{
    public class LobbySettingsMenu : LobbyMenuPanel
    {
        public Slider masterVolumeSlider;
        public Slider bgmSlider;
        public Slider sfxSlider;

        public Button applyButtoin;

        public void OnApplyClicked()
        {
            // TODO: Apply the new settings
            lobbyManager.SwitchPanel(previousPanel);
        }
    }
}
