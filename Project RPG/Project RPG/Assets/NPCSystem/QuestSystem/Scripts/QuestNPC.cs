using RPG.Core;
using RPG.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.StatsSystem;

namespace RPG.QuestSystem
{
    public class QuestNPC : MonoBehaviour, IInteractable
    {

        #region Variables
        public QuestObject questObject;

        public Dialogue readyDialogue;
        public Dialogue acceptedDialogue;
        public Dialogue completedDialogue;

        bool isStartQuestDialogue = false;
        GameObject interactGO = null;

        [SerializeField]
        private GameObject questEffectGO;
        [SerializeField]
        private GameObject questRewardGO;

        #endregion Variables

        #region Unity Methods
        private void Awake()
        {
            var obj = FindObjectsOfType<QuestNPC>();

            if (obj.Length == 2)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            questEffectGO.SetActive(false);
            questRewardGO.SetActive(false);
            if (questObject.status == QuestStatus.None)
            {
                questEffectGO.SetActive(true);
            }
            else if (questObject.status == QuestStatus.Completed)
            {
                questRewardGO.SetActive(true);
            }
            QuestManager.Instance.OnCompletedQuest += OnCompletedQuest;
        }
        #endregion Unity Methods

        #region IInteractable Interface
        private float distance = 2.0f;
        public float Distance => distance;

        public void Interact(GameObject other)
        {
            float calcDistance = Vector3.Distance(other.transform.position, transform.position);
            if (calcDistance > Distance)
            {
                return;
            }

            if (isStartQuestDialogue)
            {
                return;
            }

            interactGO = other;
            isStartQuestDialogue = true;

            if (questObject.status == QuestStatus.None)
            {
                DialogueManager.Instance.StartDialogue(readyDialogue);
                questObject.status = QuestStatus.Accepted;
                QuestManager.Instance.StartQuest(questObject);
            }
            else if (questObject.status == QuestStatus.Accepted)
            {
                DialogueManager.Instance.StartDialogue(acceptedDialogue);
            }
            else if (questObject.status == QuestStatus.Completed)
            {
                DialogueManager.Instance.StartDialogue(completedDialogue);

                questObject.status = QuestStatus.Rewarded;

                StatsManager.Instance.QuestReward(questObject.data.rewardExp, questObject.data.rewardGold);
                QuestManager.Instance.EndQuest(questObject);

                questEffectGO.SetActive(false);
                questRewardGO.SetActive(false);
            }


            DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        }

        public void StopInteract(GameObject other)
        {
            isStartQuestDialogue = false;
        }
        #endregion IInteractable Interface

        #region Methods
        private void OnEndDialogue()
        {
            StopInteract(interactGO);
        }

        private void OnCompletedQuest(QuestObject questObject)
        {
            if (questObject.data.id == this.questObject.data.id && questObject.status == QuestStatus.Completed)
            {
                questEffectGO.SetActive(false);
                questRewardGO.SetActive(true);
            }
        }
        #endregion Methods


    }
}