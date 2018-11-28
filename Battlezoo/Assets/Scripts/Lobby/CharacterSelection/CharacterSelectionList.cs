using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UntitledGames.Lobby
{
    public class CharacterSelectionList : MonoBehaviour
    {

        public static CharacterSelectionList instance;

        public Button[] characterButtons;

        public CharacterSelectionInfo[] characters;

        //Setting Up the UI for this character
        void Start()
        {
            instance = this;

            characterButtons = new Button[transform.childCount];
            characters = new CharacterSelectionInfo[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                characterButtons[i] = transform.GetChild(i).GetComponent<Button>();
                characters[i] = characterButtons[i].GetComponent<CharacterSelectionInfo>();
            }
        }


    }
}
