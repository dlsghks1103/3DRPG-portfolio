using UnityEngine;

namespace RPG.QuestSystem
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quests/New Quest")]
    public class QuestObject : ScriptableObject
    {
        #region Variables
        public Quest data = new Quest();

        public QuestStatus status;
        #endregion Variables
    }
}
