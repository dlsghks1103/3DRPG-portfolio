using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.QuestSystem
{
    public class QuestUI : MonoBehaviour
    {
        #region Variables
        public Text[] questTextList;
        private Dictionary<int, string> questList = new Dictionary<int, string>();
        #endregion Variables

        #region Unity Methods
        void Start()
        {
            QuestManager.Instance.OnAcceptedQuest += UpdateQuestList;
            QuestManager.Instance.OnRewardQuest += ClearQuest;
        }
        #endregion Unity Methods

        #region Methods
        public void UpdateQuestList(QuestObject questObject)
        {
            if (questList.Count > 2)
            {
                Debug.Log("퀘스트 리스트가 많습니다.");
                return;
            }
            else
            {
                if (!questList.ContainsKey(questObject.data.id))
                {
                    string questText = questObject.data.title + "\n" + questObject.data.completedCount + "/" + questObject.data.count;
                    questList.Add(questObject.data.id, questText);
                }
                else if (questList.ContainsKey(questObject.data.id))
                {
                    string questText = questObject.data.title + "\n" + questObject.data.completedCount + "/" + questObject.data.count;
                    questList[questObject.data.id] = questText;
                }
            }
            SetQuestUI(questList);
        }

        public void ClearQuest(QuestObject questObject)
        {
            questList.Remove(questObject.data.id);
            SetQuestUI(questList);
        }

        public void SetQuestUI(Dictionary<int, string> questList)
        {
            for (int i = 0; i < questTextList.Length; i++)
            {
                questTextList[i].text = "";
            }

            int index = 0;
            foreach (KeyValuePair<int, string> item in questList)
            {
                questTextList[index].text = item.Value;
                index++;
            }
        }
        #endregion Methods
    }
}
