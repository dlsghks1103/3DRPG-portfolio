using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;

namespace RPG.PlyerController
{
    [RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
    public class PlayerInput : MonoBehaviour
    {
        private bool attackInput;
        private Vector2 movementInput;
        //private bool jumpInput;

        public bool AttackInput { get => attackInput; }
        public Vector2 MovementInput { get => movementInput; }
        //public bool JumpInput { get => jumpInput; }

        private void Update()
        {
            attackInput = Input.GetMouseButtonDown(0);
            movementInput.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //jumpInput = Input.GetButton("Jump");
        }
    }
}
