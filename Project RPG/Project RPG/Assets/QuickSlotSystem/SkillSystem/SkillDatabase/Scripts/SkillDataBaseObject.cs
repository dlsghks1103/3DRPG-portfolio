using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.QuickSlot.Skill
{
    [CreateAssetMenu(fileName = "New SkillDataBase", menuName = "SkillDataBase System/SkillDataBase")]
    public class SkillDataBaseObject : ScriptableObject
    {
        #region Variables
        public SkillObject[] skillObject;
        public GameObject[] skillImage;
        #endregion Variables

        #region Unity Methods
        public void OnValidate()
        {
            for (int i = 0; i < skillObject.Length; ++i)
            {
                skillObject[i].slotIndex = i;
                skillImage[i].GetComponent<Image>().sprite = skillObject[i].skillImage;
            }
        }
        #endregion Unity Methods
    }
}
