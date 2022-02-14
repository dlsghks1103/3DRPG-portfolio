using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Enemy;
using RPG.CharacterControl;

namespace RPG.AI
{
    public class AttackState : State<EnemyController>
    {
        #region Variables
        private Animator animator;
        private AttackStateController attackStateController;
        private IAttackable attackable;

        protected int attackTriggerHash = Animator.StringToHash("AttackTrigger");
        protected int attackIndexHash = Animator.StringToHash("AttackIndex");
        #endregion Variables

        #region Methods
        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            attackStateController = context.GetComponent<AttackStateController>();
            attackable = context.GetComponent<IAttackable>();
        }

        public override void OnEnter()
        {
            if (attackable == null || attackable.CurrentAttackBehaviour == null)
            {
                stateMachine.ChangeState<IdleState>();
                return;
            }


            attackStateController.enterAttackHandler += OnEnterAttackState;
            attackStateController.exitAttackHandler += OnExitAttackState;

            animator?.SetInteger(attackIndexHash, attackable.CurrentAttackBehaviour.animationIndex);
            animator?.SetTrigger(attackTriggerHash);
        }

        public override void Update(float deltaTime)
        {
        }

        public override void OnExit()
        {
            attackStateController.enterAttackHandler -= OnEnterAttackState;
            attackStateController.exitAttackHandler -= OnExitAttackState;
        }

        public void OnEnterAttackState()
        {
            UnityEngine.Debug.Log("OnEnterAttackState()");
        }

        public void OnExitAttackState()
        {
            UnityEngine.Debug.Log("OnExitAttackState()");
            stateMachine.ChangeState<IdleState>();
        }
        #endregion Methods
    }
}
