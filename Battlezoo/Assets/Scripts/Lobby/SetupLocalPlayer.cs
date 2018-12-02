using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UntitledGames.Lobby
{
    public class SetupLocalPlayer : NetworkBehaviour
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
            GameObject go = Instantiate(characterList[characterIndex].gamePrefab, transform.position, transform.rotation);
            go.transform.parent = transform;

            //Set the Camera to follow this player

            Canvas c = go.GetComponentInChildren<Canvas>();
            c.GetComponentInChildren<Text>().text = playerName;

            if (isLocalPlayer)
            {
                spawnPoints = FindObjectsOfType<NetworkStartPosition>();

                if (spawnPoints != null && spawnPoints.Length > 0)
                {
                    transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                }

                // Disable name tag for local player
                c.gameObject.SetActive(false);
                Camera.main.GetComponent<CameraFollow>().target = gameObject;
            }

            
           

        }
    }
}

