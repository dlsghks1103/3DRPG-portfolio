using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CharacterControl;
using RPG.PlyerController;

namespace RPG.Skill
{
    public class SkillController : MonoBehaviour
    {
        #region Variables
        public StatsObject playerStats;
        public GameObject effectPrefabs;
        public Transform effectPoint;

        public float coolTime = 0;
        public Image coolTimeImage;

        public int useMana = 0;

        public int animationIndex = 0;

        public bool IsCoolTime = true;
        #endregion Variables

        #region Unity Methods
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && IsCoolTime)
            {
                UseSkill();
            }
        }
        #endregion Unity Methods

        #region Methods
        public void UseSkill()
        {
            float skillCoolTime = coolTime;

            if (IsCoolTime && playerStats.Mana > useMana)
            {
                GameManager.Instance.playerController.SkillAttack(useMana, animationIndex);
                if (effectPrefabs)
                {
                    Instantiate<GameObject>(effectPrefabs, effectPoint);
                }

                StartCoroutine(SkillCoolTime(skillCoolTime));
            }
        }

        IEnumerator SkillCoolTime(float skillCoolTime)
        {
            IsCoolTime = false;

            while (skillCoolTime > 1.0f)
            {
                skillCoolTime -= Time.deltaTime;
                coolTimeImage.fillAmount = (1.0f / skillCoolTime);
                yield return new WaitForFixedUpdate();
            }
            IsCoolTime = true;
        }
        #endregion Methods
    }
}
