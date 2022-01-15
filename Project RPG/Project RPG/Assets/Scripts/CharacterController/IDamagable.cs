using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public interface IDamagable
    {
        bool IsAlive
        {
            get;
        }

        void TakeDamage(int damage, GameObject hitEffect);
    }
}
