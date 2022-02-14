using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Enemy;
using RPG.CharacterControl;

namespace RPG.AI
{
    public class IdleState : State<EnemyController>
    {
        #region Variables
        private Animator animator;
        private CharacterController controller;

        protected int isMoveHash = Animator.StringToHash("IsMove");
        protected int moveSpeedHash = Animator.StringToHash("MoveSpeed");
        #endregion Variables

        #region Methods
        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
        }

        public override void OnEnter()
        {
            animator?.SetBool(isMoveHash, false);
            animator.SetFloat(moveSpeedHash, 0);
            controller?.Move(Vector3.zero);
        }

        public override void Update(float deltaTime)
        {
            if (context.Target)
            {
                if (context.IsAvailableAttack)
                {
                    stateMachine.ChangeState<AttackState>();
                }
                else
                {
                    stateMachine.ChangeState<MoveState>();
                }
            }
        }

        public override void OnExit()
        {
        }
        #endregion Methods
    }
}
