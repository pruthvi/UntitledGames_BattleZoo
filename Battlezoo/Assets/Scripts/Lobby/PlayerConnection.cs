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

        // Use this for initialization

        void Start()
        {
            if (isServer)
            {
                SpawnMyCharacter();
            }
        }
        
        void Update()
        {
            // TODO: If player dies make them spectating
        }

        // Notice the server to spawn the character that represent the player
        void SpawnMyCharacter()
        {
            if (!isServer)
            {
                return;
            }
            GameObject character = Instantiate(characterList[characterIndex].gamePrefab, transform.position, transform.rotation);
            character.transform.parent = transform;
            character.GetComponent<Character>().playerName = playerName;

            // Set the start position
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                character.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            NetworkServer.SpawnWithClientAuthority(character, connectionToClient);
        }
    }
}

