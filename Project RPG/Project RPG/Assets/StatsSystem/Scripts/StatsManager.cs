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
        private void Awake()
        {
            instance = this;
        }
        #endregion Unity Methods

        #region Methods
        public void SetEXP(int exp)
        {
            statsObject.AddExp(exp);
        }

        #endregion Methods
    }
}
