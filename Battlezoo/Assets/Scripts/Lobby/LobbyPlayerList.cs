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
        protected List<int> _selectedCharacter = new List<int>();
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
            _selectedCharacter.Add(0);
            player.transform.SetParent(playerListContentTransform, false);

            PlayerListModified();
        }

        public void PlayerListModified()
        {
            int i = 0;
            foreach(LobbyPlayer p in _players)
            {
                p.OnPlayerListChanged(i);
                i++;
            }
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            _players.Remove(player);
            PlayerListModified();
        }
    }
}
