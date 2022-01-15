using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CharacterControl
{
    public interface IAttackable
    {
        AttackBehaviour CurrentAttackBehaviour
        {
            get;
        }

        void OnExecuteAttack(int attackIndex);

        void OnExecuteSkillAttack();
    }
}
