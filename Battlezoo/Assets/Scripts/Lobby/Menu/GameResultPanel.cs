using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UntitledGames.Lobby.Menu
{
    public class GameResultPanel : MonoBehaviour
    {
        [Header("UI Reference")]
        public Button btnSpectating;
        public Button btnMainMenu;
        public Button btnExit;

        public Text txtGameResult;

        public GameObject recordDetailPanel;
        public GameObject recordDetailPrefab;

        public string[] labels = { "Total Damage Dealt:", "Total Damage Taken:", "Total Distance Travelled:", "Total Score Earned:" };
        public Text[] dataFields;
        void Start()
        {
            //SetupPanel();
            btnSpectating.onClick.RemoveAllListeners();
            btnSpectating.onClick.AddListener(OnSpectatingClicked);
            btnMainMenu.onClick.RemoveAllListeners();
            btnMainMenu.onClick.AddListener(() => { LobbyManager.instance.BackToSetup(); gameObject.SetActive(false); });
            btnExit.onClick.RemoveAllListeners();
            btnExit.onClick.AddListener(() => { Application.Quit(); });
        }

        void SetupPanel()
        {
            // for(int i = 0; i < labels.Length; i++){
            //     GameObject recordDetail = Instantiate(recordDetailPrefab) as GameObject;
            //     recordDetail.transform.GetChild(0).GetComponent<Text>().text = labels[i];
            //     // Prevent the child scaling
            //     Vector3 scale = recordDetailPanel.transform.localScale;
            //     recordDetail.GetComponent<RectTransform>().SetParent(recordDetailPanel.transform);
            //     recordDetail.transform.localScale = scale;
            //     dataFields[i] = recordDetail.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
            // }
        }

        public void ShowStats(Character c, bool win)
        {
            HUDManager.instance.ToggleTopPanel(false);
            gameObject.SetActive(true);
            if (c.data != null)
            {
                if (txtGameResult != null)
                {
                    txtGameResult.text = win ? "You Win!" : "You Lose!";
                }
                float[] fields = { c.data.totalDamageDealt, c.data.totalDamageTaken, c.data.totalDistanceTravelled, calculateScore(c.data) };

                for (int i = 0; i < labels.Length; i++)
                {
                    if (dataFields[i] != null)
                    {
                        dataFields[i].text = ((int)fields[i]).ToString();
                    }
                }
            }
        }

        private float calculateScore(PlayerData data)
        {
            // Score formula
            // Kill * (damageDealt * 2 + 0.1 * damageTaken + distanceTravelled)
            return (int)((data.totalPlayerEliminated + 1) * (data.totalDamageDealt * 2 + data.totalDamageTaken * 0.1f + data.totalDistanceTravelled));
        }

        void OnSpectatingClicked()
        {
            gameObject.SetActive(false);
            HUDManager.instance.Spectating();
        }
    }
}