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
        #region Variables
        private bool attackInput;
        private Vector2 movementInput;
        private bool npcInteractInput;
        //private bool jumpInput;
        #endregion Variables

        #region Properties
        public bool AttackInput { get => attackInput; }
        public bool NPCInteractInput { get => npcInteractInput; }
        public Vector2 MovementInput { get => movementInput; }
        //public bool JumpInput { get => jumpInput; }
        #endregion Properties

        #region Unity Methods
        private void Update()
        {
            //attackInput = Input.GetMouseButtonDown(0);
            npcInteractInput = Input.GetMouseButtonDown(0);
            movementInput.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //jumpInput = Input.GetButton("Jump");
        }
        #endregion Unity Methods
    }
}
