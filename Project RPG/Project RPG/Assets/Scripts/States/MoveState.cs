using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Enemy;
using UnityEngine.AI;
using RPG.CharacterControl;

namespace RPG.AI
{
    public class MoveState : State<EnemyController>
    {
        #region Variables
        private Animator animator;
        private CharacterController controller;
        private NavMeshAgent agent;
        private Vector3 calcVelocity = Vector3.zero;

        private float gravity = -9.81f;
        private bool isGrounded;

        private int isMovehash = Animator.StringToHash("IsMove");
        private int moveSpeedHash = Animator.StringToHash("MoveSpeed");
        #endregion Variables

        #region Methods
        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();

            agent = context.GetComponent<NavMeshAgent>();
        }

        public override void OnEnter()
        {
            agent.stoppingDistance = context.AttackRange;
            agent?.SetDestination(context.Target.position);

            animator?.SetBool(isMovehash, true);
        }

        public override void Update(float deltaTime)
        {
            if (context.Target)
            {
                agent.SetDestination(context.Target.position);
            }

            isGrounded = controller.isGrounded;
            if (isGrounded && calcVelocity.y < 0)
            {
                calcVelocity.y = 0f;
            }

            calcVelocity.y += gravity * Time.deltaTime;

            //controller.Move(calcVelocity * Time.deltaTime);
    
            controller.Move(agent.velocity * Time.deltaTime);

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                animator.SetFloat(moveSpeedHash, agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);
            }
            else
            {

                if (!agent.pathPending)
                {
                    animator.SetFloat(moveSpeedHash, 0);
                    animator.SetBool(isMovehash, false);
                    agent.ResetPath();

                    stateMachine.ChangeState<IdleState>();
                }
            }
        }

        public override void OnExit()
        {
            agent.stoppingDistance = 0.0f;
            agent.ResetPath();

            animator?.SetBool(isMovehash, false);
            animator?.SetFloat(moveSpeedHash, 0.0f);
        }
        #endregion Methods
    }
}
