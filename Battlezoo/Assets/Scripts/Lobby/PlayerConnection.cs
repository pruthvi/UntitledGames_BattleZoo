using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UntitledGames.Lobby
{
    public class PlayerConnection : NetworkBehaviour
    {
        [Header("Selectable Characters")]
        public CharacterSelectionInfo[] characterList;

        [SyncVar]
        public string playerName;

        [SyncVar]
        public int characterIndex;

        private NetworkStartPosition[] spawnPoints;

        public GameObject character;

        void Start()
        {
     //       SpawnMyCharacter();
        }

        void Update()
        {
            // TODO: If player dies make them spectating
        }

        // Notice the server to spawn the character that represent the player
        public void SpawnMyCharacter()
        {
            character = Instantiate(characterList[characterIndex].gamePrefab, transform.position, transform.rotation);
            character.transform.parent = transform;
            Character c = character.GetComponent<Character>();
            c.playerName = playerName;
            c.connection = this;
            // Set the start position
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                character.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            if (isServer)
            {
                NetworkServer.SpawnWithClientAuthority(character, connectionToClient);
            }
        }
    }
}

