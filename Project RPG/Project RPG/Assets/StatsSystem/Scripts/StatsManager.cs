using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StatsSystem
{
    public class StatsManager : MonoBehaviour
    {
        #region Variables
        private static StatsManager instance;

        public StatsObject statsObject;

        #endregion Variables

        #region Properties
        public static StatsManager Instance => instance;
        #endregion Properties

        #region Unity Methods
        private void Start()
        {
            instance = this;
        }
        #endregion Unity Methods

        #region Methods
        public void QuestReward(int exp, int gold)
        {
            statsObject.AddExp(exp);
            statsObject.AddGold(gold);
        }
        #endregion Methods
    }
}
