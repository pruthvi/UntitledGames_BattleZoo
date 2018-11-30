using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UntitledGames.Lobby
{
    public class LobbyManager : NetworkLobbyManager
    {

        static public LobbyManager instance;

        [Header("UI Reference")]
        public RectTransform background;
        public RectTransform mainMenuPanel;
        public RectTransform lobbyPanel;
        public CountdownPanel countdownPanel;

        public PlayerInfoPanel playerInfoPanel;

        protected RectTransform currentPanel;


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

        private bool isInGame;

        public int characterIndex;

        [HideInInspector]
        public bool requestCancelMatch;

        void Start()
        {
            instance = this;
            _lobbyHooks = GetComponent<LobbyHook>();
            currentPanel = mainMenuPanel;

            DontDestroyOnLoad(gameObject);
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            if (SceneManager.GetSceneAt(0).name == lobbyScene)
            {
                //if (topPanel.isInGame)
                //{
                //    ChangeTo(lobbyPanel);
                //    if (_isMatchmaking)
                //    {
                //        if (conn.playerControllers[0].unetView.isServer)
                //        {
                //            backDelegate = StopHostClbk;
                //        }
                //        else
                //        {
                //            backDelegate = StopClientClbk;
                //        }
                //    }
                //    else
                //    {
                //        if (conn.playerControllers[0].unetView.isClient)
                //        {
                //            backDelegate = StopHostClbk;
                //        }
                //        else
                //        {
                //            backDelegate = StopClientClbk;
                //        }
                //    }
                //}
                //else
                //{
                //    ChangeTo(mainMenuPanel);
                //}

                //topPanel.ToggleVisibility(true);
                //topPanel.isInGame = false;
            }
            else
            {
                background.gameObject.SetActive(false);
                SwitchPanel(null);

                //Destroy(GameObject.Find("MainMenuUI(Clone)"));

                //backDelegate = StopGameClbk;
                //topPanel.isInGame = true;
                //topPanel.ToggleVisibility(false);
            }
        }

        //allow to handle the (+) button to add/remove player
        public void OnPlayersNumberModified(int count)
        {
            //TODO add this function
            _playerNumber += count;
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
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }
            else
            {
                currentPanel.gameObject.SetActive(false);
            }
            currentPanel = newPanel;
        }

        //we want to disable the button Ready if we don't have enough player
        //But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

            LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
            newPlayer.ToggleReadyButton(numPlayers + 1 >= minPlayers);


            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.ToggleReadyButton(numPlayers + 1 >= minPlayers);
                }
            }

            return obj;
        }

        public void BackToSetup()
        {
            SwitchPanel(mainMenuPanel);
        }

        public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject obj = Instantiate(gamePlayerPrefab.gameObject) as GameObject;
            
            return obj;
        }
       

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            //This hook allows you to apply state data from the lobby-player to the game-player
            if (_lobbyHooks)
            {
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);
            }
               

            return true;
        }


        // --- Countdown management

        public override void OnLobbyServerPlayersReady()
        {
            bool allready = true;
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                    allready &= lobbySlots[i].readyToBegin;
            }

            if (allready)
            {
                requestCancelMatch = false;
                StartCoroutine(ServerCountdownCoroutine());
            }
               
        }

        public IEnumerator ServerCountdownCoroutine()
        {
            float remainingTime = prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                // Exit
                if (requestCancelMatch)
                {
                    // Break the loop allow the RpcUpdateCountdown to be called, which hide the countdown panel
                    break;
                }
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                            (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                    
                }

                
            }

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(0);
                }
            }
            if (requestCancelMatch)
            {
                requestCancelMatch = false;
            }
            else
            {
                isInGame = true;
                ServerChangeScene(playScene);
            }
        }

    }
}
