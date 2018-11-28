using UnityEngine.UI;
using UnityEngine;

namespace UntitledGames.Lobby
{
    public class CharacterSelectionInfo : MonoBehaviour
    {

        [Header("Info")]
        public string characterName;
        [TextArea]
        public string description;
        public Sprite playerListIcon;
        public Sprite characterSelectionIcon;
        public GameObject gamePrefab;

        //Setting Up the UI for this character
        void Start()
        {
            transform.GetChild(0).GetComponent<Text>().text = characterName;
            transform.GetComponent<Image>().sprite = characterSelectionIcon;
        }
    }
}

