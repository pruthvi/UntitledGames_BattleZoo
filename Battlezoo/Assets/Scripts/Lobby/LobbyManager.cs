using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UntitledGames.Lobby.Menu;

namespace UntitledGames.Lobby
{
    public class LobbyManager : NetworkLobbyManager
    {

        public static LobbyManager instance;

        [Header("UI Reference")]
        public RectTransform backgroundPanel;
        public LobbyMainMenu mainMenuPanel;
        public LobbySettingsMenu settingsPanel;
        public LobbySetupMenu setupPanel;
        public LobbyCharacterSelectionMenu characterSelectionPanel;
        public LobbyInfoPanel lobbyInfoPanel;
        public InGameMenu inGameMenuPanel;

        protected LobbyMenuPanel currentPanel;

        [Header("Match Info")]
        [Tooltip("Time in second between all players ready & match start")]
        public float prematchCountdown = 5.0f;

        //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
        //of players, so that even client know how many player there is.
        [HideInInspector]
        public int _playerNumber = 0;

        protected LobbyHook _lobbyHooks;

        [HideInInspector]
        public bool requestCancelMatch;

        void OnEnable()
        {
            instance = this;
        }

        void Start()
        {
            _lobbyHooks = GetComponent<LobbyHook>();
            // Disable all panels
            settingsPanel.gameObject.SetActive(false);
            setupPanel.gameObject.SetActive(false);
            characterSelectionPanel.gameObject.SetActive(false);
            inGameMenuPanel.gameObject.SetActive(false);
            lobbyInfoPanel.gameObject.SetActive(false);
            // Set default panel
            currentPanel = mainMenuPanel;
            SwitchPanel(mainMenuPanel);
            DontDestroyOnLoad(gameObject);
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            if (SceneManager.GetSceneAt(0).name == lobbyScene)
            {
                if (inGameMenuPanel.isInGame)
                {
                    SwitchPanel(setupPanel);
                    // TODO: UNet online match making
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
                }
                else
                {
                    SwitchPanel(mainMenuPanel);
                }
                inGameMenuPanel.isInGame = false;
            }
            else
            {
                backgroundPanel.gameObject.SetActive(false);
                SwitchPanel(null);
            }
        }
        
        public override void OnStartHost()
        {
            base.OnStartHost();
            SwitchPanel(characterSelectionPanel);
            // Set the backDelegate to StopHostCallback so that when back button clicked it will stop host
            backDelegate = StopHostCallback;
        }

        public void SwitchPanel(LobbyMenuPanel newPanel)
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
                if (newPanel != inGameMenuPanel)
                {
                    inGameMenuPanel.previousPanel = currentPanel;
                }
                else
                {
                    inGameMenuPanel.previousPanel = null;
                }
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
            if (currentPanel != mainMenuPanel && inGameMenuPanel.isInGame == false)
            {
                inGameMenuPanel.gameObject.SetActive(true);
            }
            else
            {
                inGameMenuPanel.gameObject.SetActive(false);
            }
        }

        //we want to disable the button Ready if we don't have enough player
        //But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

            //LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
            //newPlayer.ToggleReadyButton(numPlayers + 1 >= minPlayers);

            //for (int i = 0; i < lobbySlots.Length; ++i)
            //{
            //    LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

            //    if (p != null)
            //    {
            //        p.ToggleReadyButton(numPlayers + 1 >= minPlayers);
            //    }
            //}

            return obj;
        }

        //public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        //{
        //    GameObject obj = Instantiate(gamePlayerPrefab.gameObject) as GameObject;
            
        //    return obj;
        //}
       

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            //This hook allows you to apply state data from the lobby-player to the game-player
            if (_lobbyHooks)
            {
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);
            }

            return true;
        }

        // When all players ready start countdown
        public override void OnLobbyServerPlayersReady()
        {
            bool allready = true;
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                // Check the slot before accessing
                if (lobbySlots[i] != null)
                {
                    allready &= lobbySlots[i].readyToBegin;
                }
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
                {
                    //to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {
                            //there is maxPlayer slots, so some could be == null, need to test it before accessing!
                            (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }
            if (requestCancelMatch)
            {
                for (int i = 0; i < lobbySlots.Length; ++i)
                {
                    if (lobbySlots[i] != null)
                    {
                        // Set everyone's lockin to false;
                        (lobbySlots[i] as LobbyPlayer).RpcCancelMatch();
                    }
                }
                requestCancelMatch = false;
            }
            else
            {
                inGameMenuPanel.isInGame = true;
                ServerChangeScene(playScene);
            }
        }

        public void DisplayIsConnecting()
        {
            characterSelectionPanel.gameObject.SetActive(false);
            inGameMenuPanel.gameObject.SetActive(false);
            lobbyInfoPanel.Display("Connecting...", "Cancel", () => { BackToSetup(); });
        }

        // ----------------- Client callbacks ------------------

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            lobbyInfoPanel.gameObject.SetActive(false);

            if (!NetworkServer.active)
            {
                //only to do on pure client (not self hosting client)
                SwitchPanel(characterSelectionPanel);
                backDelegate = StopClientCallback;
            }
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            lobbyInfoPanel.Display("Disconnected due to time out", "OK", () => { BackToSetup(); });
        }

        // Back Button

        public delegate void BackButtonDelegate();
        public BackButtonDelegate backDelegate;

        // This is the listener that Back button listen to
        public void BackToSetup()
        {
            backDelegate();
            inGameMenuPanel.gameObject.SetActive(true);
            characterSelectionPanel.ResetControls();
            SwitchPanel(setupPanel);
            inGameMenuPanel.previousPanel = mainMenuPanel;
        }

        // Stop the server, this is set in the StartHost and called in BackToSetup()
        public void StopHostCallback()
        {
            StopHost();
        }

        // Stop the client, this will be set when Client click the Join button in SetUp Panel, and called in BackToSetup()
        public void StopClientCallback()
        {
            StopClient();
        }
    }
}
