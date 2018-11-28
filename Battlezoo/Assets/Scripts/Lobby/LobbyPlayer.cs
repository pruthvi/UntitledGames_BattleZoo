﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UntitledGames.Lobby
{
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        static string[] Name = new string[] { "Camel", "Turtle", "Chicken", "Meerkat" };

        public InputField inputPlayerName;

        public Image playerAvatar;
        public Text playerNameText;
        public Text playerStatusText;

        public Button btnReady;

        //OnMyName function will be invoked on clients when server change the value of playerName
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";

        public string defaultName = "";

        void Start()
        {
            // Get the player list panel and set the ready button
            btnReady = transform.parent.parent.GetChild(1).GetComponent<Button>();
      //      playerNameText.text = LobbyManager.instance.
        }

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();
            if (LobbyManager.instance != null)
            {
                LobbyManager.instance.OnPlayersNumberModified(1);
            }

            LobbyPlayerList._instance.AddPlayer(this);

            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
           // OnMyName(playerName);
           // OnMyColor(playerColor);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
            //readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

            SetupLocalPlayer();
        }

        void SetupOtherPlayer()
        {
            //   nameInput.interactable = false;
            //    removePlayerButton.interactable = NetworkServer.active;

            //   ChangeReadyButtonColor(NotReadyColor);

            //    readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            //     readyButton.interactable = false;
            btnReady = transform.parent.parent.GetChild(1).GetComponent<Button>();
            btnReady.onClick.RemoveAllListeners();
            btnReady.onClick.AddListener(OnReadyClicked);
            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            ////     nameInput.interactable = true;
            //     remoteIcone.gameObject.SetActive(false);
            //     localIcone.gameObject.SetActive(true);

            //     CheckRemoveButton();

            //     if (playerColor == Color.white)
            //         CmdColorChange();

            //     ChangeReadyButtonColor(JoinColor);

            //     readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
            //     readyButton.interactable = true;

            //     //have to use child count of player prefab already setup as "this.slot" is not set yet
            if (playerName == "")
            {
                defaultName = "Player " + (LobbyPlayerList._instance.playerListContentTransform.childCount);
                CmdNameChanged(defaultName);
            }
                

            //     //we switch from simple name display to name input
            //     colorButton.interactable = true;
            //     nameInput.interactable = true;
            inputPlayerName = LobbyManager.instance.playerInfoPanel.playerNameInput;
            inputPlayerName.onEndEdit.RemoveAllListeners();
            inputPlayerName.onEndEdit.AddListener(OnNameChanged);

            for(int i = 0; i < CharacterSelectionList.instance.characterButtons.Length; i++)
            {
                Button b = CharacterSelectionList.instance.characterButtons[i];
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(delegate { OnCharacterChanged(i); });
            }

            //     colorButton.onClick.RemoveAllListeners();
            //     colorButton.onClick.AddListener(OnColorClicked);

            //     //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //     //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            //     if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
        }
       
        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }
        // ///===== callback from sync var

        // public void OnMyName(string newName)
        // {
        //     playerName = newName;
        //     nameInput.text = playerName;
        // }

        // public void OnMyColor(Color newColor)
        // {
        //     playerColor = newColor;
        //     colorButton.GetComponent<Image>().color = newColor;
        // }

        // //===== UI Handler

        // //Note that those handler use Command function, as we need to change the value on the server not locally
        // //so that all client get the new value throught syncvar
        public void OnCharacterChanged(int characterIndex)
        {
            CmdCharacterChange(characterIndex);
        }
        
        public void OnNameChanged(string str)
        {
            if (str.Equals(""))
                str = defaultName;
            CmdNameChanged(str);
        }

        // public void OnReadyClicked()
        // {
        //     SendReadyToBeginMessage();
        // }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                Text textComponent = btnReady.transform.GetChild(0).GetComponent<Text>();
                playerStatusText.text = "Ready";
                playerStatusText.color = Color.green;
            }
            else
            {
                Text textComponent = btnReady.transform.GetChild(0).GetComponent<Text>();
                playerStatusText.text = "Not Ready";
                playerStatusText.color = Color.red;
            }
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.instance.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.instance.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        // [ClientRpc]
        // public void RpcUpdateRemoveButton()
        // {
        //     CheckRemoveButton();
        // }

        //====== Server Command

        [Command]
        public void CmdCharacterChange(int characterIndex)
        {
            Debug.Log(characterIndex);
         //   CharacterSelectionInfo info = CharacterSelectionList.instance.characters[characterIndex];
         //   playerAvatar.sprite = info.playerListIcon;
        }

        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        //===== callback from sync var

        public void OnMyName(string newName)
        {
            playerName = newName;
            playerNameText.text = playerName;
          //  LobbyManager.instance.playerInfoPanel.playerNameInput.text = playerName;
        }

        // ========================

        // //Cleanup thing when get destroy (which happen when client kick or disconnect)
        // public void OnDestroy()
        // {
        //     LobbyPlayerList._instance.RemovePlayer(this);
        //     if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);
        //     int idx = System.Array.IndexOf(Colors, playerColor);

        //     if (idx < 0)
        //         return;

        //     for (int i = 0; i < _colorInUse.Count; ++i)
        //     {
        //         if (_colorInUse[i] == idx)
        //         {//that color is already in use
        //             _colorInUse.RemoveAt(i);
        //             break;
        //         }
        //     }
        // }

    }
}
    