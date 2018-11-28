using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UntitledGames.Lobby
{
    public class LobbyManager : NetworkLobbyManager
    {

        static public LobbyManager instance;

        [Header("UI Reference")]
        public RectTransform lobbyPanel;

        [Header("Unity UI Lobby")]
        [Tooltip("Time in second between all players ready & match start")]
        public float prematchCountdown = 5.0f;

        //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
        //of players, so that even client know how many player there is.
        [HideInInspector]
        public int _playerNumber = 0;

        //used to disconnect a client properly when exiting the matchmaker
        [HideInInspector]
        public bool _isMatchmaking = false;

        protected bool _disconnectServer = false;

        protected ulong _currentMatchID;

        protected LobbyHook _lobbyHooks;


        //allow to handle the (+) button to add/remove player
        public void OnPlayersNumberModified(int count)
        {
            //TODO add this function
        }

        public override void OnStartHost()
        {
            base.OnStartHost();

            SwitchPanel(lobbyPanel);
            //backDelegate = StopHostClbk;
            //SetServerInfo("Hosting", networkAddress);
        }

        public void SwitchPanel(RectTransform newPanel)
        {

        }
    }
}
