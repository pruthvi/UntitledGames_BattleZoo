using UnityEngine.UI;
using UnityEngine;

namespace UntitledGames.Lobby
{
    [CreateAssetMenu(fileName = "Character", menuName = "Character/Character", order = 1)]
    public class CharacterSelectionInfo : ScriptableObject
    {
        [Header("Info")]
        public string characterName;
        [TextArea]
        public string description;
        public Sprite playerListIcon;
        public Sprite characterSelectionIcon;
        public GameObject gamePrefab;
    }
}

