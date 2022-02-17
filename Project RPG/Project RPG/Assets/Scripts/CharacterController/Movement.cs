using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CharacterControl;
using RPG.PlayerCameraSystem;

namespace RPG.PlyerController
{
    [RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator)), RequireComponent(typeof(PlayerInput))]
    public class Movement : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private PlayerCamera playerCamera;

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

        [SerializeField]
        private bl_Joystick Joystick;

        private CharacterController characterController;
        private Vector3 calcVelocity = Vector3.zero;

        private Vector2 lastMovementInput;
        private float DecelerationOnStop = 0.00f;

        private Animator animator;
        private PlayerInput playerInput;
        private PlayerController PlayerController;

        readonly int InputX = Animator.StringToHash("InputX");
        readonly int InputY = Animator.StringToHash("InputY");
        readonly int dashTriggerHash = Animator.StringToHash("DashTrigger");

        private bool isGrounded;

        Vector3 moveDirection;
        #endregion Variables

        #region Unity Methods
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
            PlayerController = GetComponent<PlayerController>();
        }
        private void FixedUpdate()
        {
            if (animator == null) return;

            if (PlayerController.AttackInProgress)
            {
                StopMovementOnAttack();
            }
            else
            {
                isGrounded = characterController.isGrounded;
                ProcessMove();
            }
        }
        #endregion Unity Methods

        #region Methods
        private void ProcessMove()
        {
            float _x = Joystick.Horizontal;
            float _z = Joystick.Vertical;

            Vector3 offset = playerCamera.transform.forward;
            offset.y = 0;
            transform.LookAt(characterController.transform.position + offset);

            moveDirection = new Vector3(_x, 0, _z);
            moveDirection = characterController.transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (moveDirection != Vector3.zero)
            {
                characterController.SimpleMove(moveDirection);
                //characterController.Move(moveDirection * Time.deltaTime);
                transform.forward = moveDirection;
            }

            animator.SetFloat(InputX, _x);
            animator.SetFloat(InputY, _z);
           



            /*
            float _x = Joystick.Horizontal;
            float _z = Joystick.Vertical;

            moveDirection = new Vector3(_x, 0, _z);
            //moveDirection = transform.TransformDirection(moveDirection);

            if (moveDirection != Vector3.zero)
            {
                characterController.Move(moveDirection * Time.deltaTime * speed);
                transform.forward = moveDirection;
            }
            */
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
        #endregion Methods
    }
}
