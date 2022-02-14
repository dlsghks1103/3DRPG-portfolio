using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.QuickSlot.Skill
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Skill System/Skill")]
    public class SkillObject : ScriptableObject
    {
        #region Properties
        public int slotIndex;
        public Sprite skillImage;
        public GameObject effectPrefabs;

        public float coolTime = 0;
        
        public int useMana = 0;

        public int animationIndex = 0;
        #endregion Properties
    }
}
