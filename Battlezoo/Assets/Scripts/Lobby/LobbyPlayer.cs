using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UntitledGames.Lobby
{
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        static string[] Name = new string[] { "Camel", "Turtle", "Chicken", "Meerkat" };

        public Image playerAvatar;
        public Text playerNameText;
        public Text playerStatusText;

        //OnMyName function will be invoked on clients when server change the value of playerName
      //  [SyncVar(hook = "OnMyName")]
        public string playerName = "";

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
        //        SetupLocalPlayer();
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

        void SetupOtherPlayer()
        {
         //   nameInput.interactable = false;
        //    removePlayerButton.interactable = NetworkServer.active;

         //   ChangeReadyButtonColor(NotReadyColor);

        //    readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
       //     readyButton.interactable = false;

            OnClientReady(false);
        }

       // void SetupLocalPlayer()
       // {
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
       //     if (playerName == "")
       //         CmdNameChanged("Player" + (LobbyPlayerList._instance.playerListContentTransform.childCount - 1));

       //     //we switch from simple name display to name input
       //     colorButton.interactable = true;
       //     nameInput.interactable = true;

       //     nameInput.onEndEdit.RemoveAllListeners();
       //     nameInput.onEndEdit.AddListener(OnNameChanged);

       //     colorButton.onClick.RemoveAllListeners();
       //     colorButton.onClick.AddListener(OnColorClicked);

       //     readyButton.onClick.RemoveAllListeners();
       //     readyButton.onClick.AddListener(OnReadyClicked);

       //     //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
       //     //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
       //     if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
       // }

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
       // public void OnColorClicked()
       // {
       //     CmdColorChange();
       // }

       // public void OnReadyClicked()
       // {
       //     SendReadyToBeginMessage();
       // }

       // public void OnNameChanged(string str)
       // {
       //     CmdNameChanged(str);
       // }

       // [ClientRpc]
       // public void RpcUpdateCountdown(int countdown)
       // {
       //     LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
       //     LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
       // }

       // [ClientRpc]
       // public void RpcUpdateRemoveButton()
       // {
       //     CheckRemoveButton();
       // }

       // //====== Server Command

       // [Command]
       // public void CmdColorChange()
       // {
       //     int idx = System.Array.IndexOf(Colors, playerColor);

       //     int inUseIdx = _colorInUse.IndexOf(idx);

       //     if (idx < 0) idx = 0;

       //     idx = (idx + 1) % Colors.Length;

       //     bool alreadyInUse = false;

       //     do
       //     {
       //         alreadyInUse = false;
       //         for (int i = 0; i < _colorInUse.Count; ++i)
       //         {
       //             if (_colorInUse[i] == idx)
       //             {//that color is already in use
       //                 alreadyInUse = true;
       //                 idx = (idx + 1) % Colors.Length;
       //             }
       //         }
       //     }
       //     while (alreadyInUse);

       //     if (inUseIdx >= 0)
       //     {//if we already add an entry in the colorTabs, we change it
       //         _colorInUse[inUseIdx] = idx;
       //     }
       //     else
       //     {//else we add it
       //         _colorInUse.Add(idx);
       //     }

       //     playerColor = Colors[idx];
       // }

       // [Command]
       // public void CmdNameChanged(string name)
       // {
       //     playerName = name;
       // }

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
    