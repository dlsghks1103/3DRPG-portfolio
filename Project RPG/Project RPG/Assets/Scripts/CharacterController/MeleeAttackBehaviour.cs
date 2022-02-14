using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.CharacterControl
{
    public class MeleeAttackBehaviour : AttackBehaviour
    {
        #region Variables
        public ManualCollision attackCollision;
        public StatsObject statsObject;
        #endregion Variables

        #region Methods
        public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
        {
            Collider[] colliders = attackCollision?.CheckOverlapBox(targetMask);

            foreach (Collider col in colliders)
            {
                col.gameObject.GetComponent<IDamagable>()?.TakeDamage(PlayerDamage(), effectPrefab);
            }

            calcCoolTime = 0.0f;
        }

        public override void ExecuteSkillAttack(GameObject target = null, Transform startPoint = null)
        {
            Collider[] colliders = attackCollision?.CheckOverlapBox(targetMask);

            foreach (Collider col in colliders)
            {
                col.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage * PlayerDamage(), effectPrefab);
            }

            calcCoolTime = 0.0f;
        }

        private int PlayerDamage()
        {
            if (statsObject == null)
            {
                return damage;
            }

            return statsObject.GetModifiedValue(AttributeType.Agility) + statsObject.GetModifiedValue(AttributeType.Strength);
        }
        #endregion Methods
    }
}
