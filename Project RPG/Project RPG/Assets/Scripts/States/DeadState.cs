using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Enemy;
using System;
using RPG.CharacterControl;

namespace RPG.AI
{
    [Serializable]
    public class DeadState : State<EnemyController>
    {
        #region Variables
        private Animator animator;

        protected int isAliveHash = Animator.StringToHash("IsAlive");
        #endregion Variables

        #region Methods
        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            animator?.SetBool(isAliveHash, false);
        }

        public override void Update(float deltaTime)
        {
            if (stateMachine.ElapsedTimeInState > 1.0f)
            {
                GameObject.Destroy(context.gameObject);
            }
        }

        public override void OnExit()
        {
        }
        #endregion Methods
    }
}
