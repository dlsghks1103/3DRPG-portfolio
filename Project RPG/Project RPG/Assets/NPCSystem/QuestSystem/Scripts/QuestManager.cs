using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RPG.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        #region Variables
        private static QuestManager instance;

        public QuestDatabaseObject questDatabase;

        public event Action<QuestObject> OnCompletedQuest;
        public event Action<QuestObject> OnAcceptedQuest;
        public event Action<QuestObject> OnRewardQuest;

        #endregion Variables

        #region Properties
        public static QuestManager Instance => instance;
        #endregion Properties

        #region Unity Methods
        private void Awake()
        {
            instance = this;
        }
        #endregion Unity Methods

        #region Methods
        public void StartQuest(QuestObject questObject)
        {
            OnAcceptedQuest?.Invoke(questObject);
        }

        public void EndQuest(QuestObject questObject)
        {
            OnRewardQuest.Invoke(questObject);
        }

        public void ProcessQuest(QuestType type, int targetId)
        {
            foreach (QuestObject questObject in questDatabase.questObjects)
            {
                if (questObject.status == QuestStatus.Accepted && questObject.data.type == type && questObject.data.targetId == targetId)
                {
                    questObject.data.completedCount++;
                    OnAcceptedQuest?.Invoke(questObject);
                    if (questObject.data.completedCount >= questObject.data.count)
                    {
                        questObject.status = QuestStatus.Completed;
                        OnCompletedQuest?.Invoke(questObject);
                    }
                }
            }
        }
        #endregion Methods
    }
}
