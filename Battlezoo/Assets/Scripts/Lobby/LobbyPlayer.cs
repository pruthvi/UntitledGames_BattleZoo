using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using UntitledGames.Lobby.Menu;

namespace UntitledGames.Lobby
{
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        public InputField inputPlayerName;
        public Button buttonReady;

        public Image playerAvatar;
        public Text playerNameText;
        public Text playerStatusText;

        public Image localPlayerBackground;

        //OnMyName function will be invoked on clients when server change the value of playerName
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";
        public string defaultName = "";
        [SyncVar(hook = "OnMyCharacter")]
        public int characterIndex;

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            LobbyPlayerList._instance.AddPlayer(this);
            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }
            
            OnMyName(playerName);
            OnMyCharacter(0);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            SetUpCharacterSelection();
            SetupLocalPlayer();
        }

        void SetUpCharacterSelection()
        {
            LobbyCharacterSelectionMenu characterSelectionMenu = LobbyManager.instance.characterSelectionPanel;
            for (int i = 0; i < characterSelectionMenu.characterButtons.Length; i++)
            {
                //make 'i' as a temporary variable and prevent it from being changed inside lambda expression
                int characterIndex = i;
                Button b = characterSelectionMenu.characterButtons[i];
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(() => { OnCharacterChanged(characterIndex); }); // OnCharacterChanged(i) will not work because it will be changed when looping
            }
        }

        void SetupOtherPlayer()
        {
            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            localPlayerBackground.gameObject.SetActive(true);
            if (playerName == "")
            {
                defaultName = "Player " + (LobbyPlayerList._instance.playerListContentTransform.childCount);
                CmdNameChanged(defaultName);
            }

            inputPlayerName = LobbyManager.instance.characterSelectionPanel.playerNameInputField;
            inputPlayerName.onEndEdit.RemoveAllListeners();
            inputPlayerName.onEndEdit.AddListener(OnNameChanged);

            buttonReady = LobbyManager.instance.characterSelectionPanel.readyButton;
            buttonReady.onClick.RemoveAllListeners();
            buttonReady.onClick.AddListener(OnReadyClicked);
        }
       
        public void OnReadyClicked()
        {
            if (readyToBegin)
            {
                SendNotReadyToBeginMessage();
                LobbyManager.instance.characterSelectionPanel.LockInCharacterSelection(false);
            }
            else
            {
                SendReadyToBeginMessage();
                LobbyManager.instance.characterSelectionPanel.LockInCharacterSelection(true);
            }
        }

        //===== UI Handler

        // Note that those handler use Command function, as we need to change the value on the server not locally
        // so that all client get the new value throught syncvar
        
        // This handles the Ready States of the player
        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                playerStatusText.text = "Ready";
                playerStatusText.color = Color.green;
            }
            else
            {
                playerStatusText.text = "Not Ready";
                playerStatusText.color = Color.red;
                LobbyManager.instance.requestCancelMatch = true;
            }
        }

        public void OnCharacterChanged(int characterIndex)
        {
            CmdCharacterChange(characterIndex);
        }

        // SyncVar call back which update the correct character avatar in the list
        public void OnMyCharacter(int newCharacterIndex)
        {
            CharacterSelectionInfo info = LobbyManager.instance.characterSelectionPanel.characters[newCharacterIndex];
            playerAvatar.sprite = info.playerListIcon;
        }

        [Command]
        public void CmdCharacterChange(int index)
        {
            characterIndex = index;
        }

        // Name Change in Lobby
        // Called when the text in name input field changed
        public void OnNameChanged(string str)
        {
            if (str.Equals(""))
                str = defaultName;
            CmdNameChanged(str);
        }

        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        // SyncVar call back
        public void OnMyName(string newName)
        {
            playerName = newName;
            playerNameText.text = playerName;
        }

        // Countdown

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.instance.characterSelectionPanel.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.instance.characterSelectionPanel.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        // Currently nothing to clean

        public override void OnClientExitLobby()
        {
            base.OnClientExitLobby();
            LobbyManager.instance.requestCancelMatch = true;
        }
        public void OnDestroy()
        {
            LobbyPlayerList._instance.RemovePlayer(this);
            //if (LobbyManager.instance != null)
            //{
            //    LobbyManager.instance.OnPlayersNumberModified(-1);
            //}
            // TODO: Set the default player name of the next player to number of the remaining players

        }
        // Currently nothing happens
        public void OnPlayerListChanged(int idx)
        {
        //    GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        // ========================
    }
}
    