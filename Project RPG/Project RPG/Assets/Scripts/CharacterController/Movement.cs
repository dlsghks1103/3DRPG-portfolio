using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CharacterControl;

namespace RPG.PlyerController
{
    [RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator)), RequireComponent(typeof(PlayerInput))]
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;

        [SerializeField]
        private float jumpHeight = 2f;

        [SerializeField]
        private float gravity = -9.81f;

        [SerializeField]
        private float dashDistance = 5f;

        [SerializeField]
        private Vector3 drags;

        private CharacterController characterController;
        private Vector3 calcVelocity = Vector3.zero;

        private Vector2 lastMovementInput;
        private float DecelerationOnStop = 0.00f;

        private Animator animator;
        private PlayerInput playerInput;
        private Combat combat;

        readonly int InputX = Animator.StringToHash("InputX");
        readonly int InputY = Animator.StringToHash("InputY");
        readonly int dashTriggerHash = Animator.StringToHash("DashTrigger");

        private bool isGrounded;

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
            combat = GetComponent<Combat>();
        }

        // Update is called once per frame
        void Update()
        {
            // Check grounded
           isGrounded = characterController.isGrounded;
            if (isGrounded && calcVelocity.y < 0)
            {
                calcVelocity.y = 0f;
            }

            if (animator == null) return;

            if (combat.AttackInProgress)
            {
                StopMovementOnAttack();
            }
            else 
            {
                ProcessMove();
                //ProcessDash();
                //ProcessJump();
            }
           
            // Process gravity
            calcVelocity.y += gravity * Time.deltaTime;

            // Process dash ground drags
            calcVelocity.x /= 1 + drags.x * Time.deltaTime;
            calcVelocity.y /= 1 + drags.y * Time.deltaTime;
            calcVelocity.z /= 1 + drags.z * Time.deltaTime;

            characterController.Move(calcVelocity * Time.deltaTime);
        }

        private void ProcessMove()
        {
            var _x = playerInput.MovementInput.x;
            var _z = playerInput.MovementInput.y;

            Vector3 moveDirection = new Vector3(_x, 0, _z);
            //moveDirection = transform.TransformDirection(moveDirection);
            characterController.Move(moveDirection * Time.deltaTime * speed);
            
            if (moveDirection != Vector3.zero)
            {
                transform.forward = moveDirection;
            }

            animator.SetFloat(InputX, _x);
            animator.SetFloat(InputY, _z);
        }

        private void ProcessJump()
        {
            // Process jump input
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                calcVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        private void ProcessDash()
        {
            // Process dash input
            if (Input.GetButtonDown("Dash"))
            {
                Debug.Log("Dash");
                calcVelocity += Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drags.x + 1)) / -Time.deltaTime),
                    0,
                    (Mathf.Log(1f / (Time.deltaTime * drags.z + 1)) / -Time.deltaTime))
                    );
                animator.SetTrigger(dashTriggerHash);
            }
        }

        private void StopMovementOnAttack()
        {
            var temp = lastMovementInput;
            temp.x -= DecelerationOnStop;
            temp.y -= DecelerationOnStop;
            lastMovementInput = temp;

            animator.SetFloat("InputX", lastMovementInput.x);
            animator.SetFloat("InputY", lastMovementInput.y);
        }
    }
}
