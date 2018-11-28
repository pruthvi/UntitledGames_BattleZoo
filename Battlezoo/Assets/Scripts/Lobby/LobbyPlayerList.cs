using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace UntitledGames.Lobby
{
    public class LobbyPlayerList : MonoBehaviour
    {
        public static LobbyPlayerList _instance;
        public RectTransform playerListContentTransform;

        protected VerticalLayoutGroup _layout;
        protected List<LobbyPlayer> _players = new List<LobbyPlayer>();
        public void OnEnable()
        {
            _instance = this;
            _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
        }

        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
            {
                return;
            }

            _players.Add(player);
            player.transform.SetParent(playerListContentTransform, false);

            //PlayerListModified();
        }

        public void PlayerListModified()
        {
            int i = 0;
            foreach(LobbyPlayer p in _players)
            {
                //p.OnPlayerListChanged(i);
                i++;
            }
        }
    }
}
