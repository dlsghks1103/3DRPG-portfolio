using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.QuestSystem
{
    public class QuestUI : MonoBehaviour
    {
        public Text[] questTextList;
        private Dictionary<int, string> questList = new Dictionary<int, string>();

        void Start()
        {
            QuestManager.Instance.OnAcceptedQuest += UpdateQuestList;
            QuestManager.Instance.OnRewardQuest += ClearQuest;
        }

        public void UpdateQuestList(QuestObject questObject)
        {
            if (questList.Count > 2)
            {
                Debug.Log("����Ʈ ����Ʈ�� �����ϴ�.");
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
    }
}
