using UnityEngine;
using RPG.CharacterControl;
using RPG.Core;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

namespace RPG.PlyerController
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class Combat : MonoBehaviour, IAttackable, IDamagable
    {
        [SerializeField]
        private InventoryObject equipment;

        [SerializeField]
        private InventoryObject inventory;

        private Animator animator;
        private PlayerInput playerInput;

        [SerializeField]
        private Transform hitPoint;

        [SerializeField]
        public StatsObject playerStats;

        readonly int attackTriggerHash = Animator.StringToHash("AttackTrigger");
        readonly int skillattackTriggerHash = Animator.StringToHash("SkillAttackTrigger");
        readonly int attackIndexHash = Animator.StringToHash("AttackIndex");
        readonly int skillAttackIndexHash = Animator.StringToHash("SkillAttackIndex");
        readonly int hitTriggerHash = Animator.StringToHash("HitTrigger");
        readonly int isAliveHash = Animator.StringToHash("IsAlive");

        [SerializeField]
        private LayerMask targetMask;

        private bool isCoroutine = true;

        public bool AttackInProgress { get; private set; } = false;
        private void Start()
        {
            inventory.OnUseItem += OnUseItem;

            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();

            InitAttackBehaviour();
        }

        private void Update()
        {
            if (!IsAlive)
            {
                return;
            }

            CheckAttackBehaviour();
            CheckSkillAttackBehaviour();

            bool isOnUI = EventSystem.current.IsPointerOverGameObject();

            if (!isOnUI && playerInput.AttackInput && !AttackInProgress&& playerStats.Mana >0)
            {
                Attack();
            }

            if (playerStats.MaxMana > playerStats.Mana && isCoroutine)
            {
                StartCoroutine(RecoveryMana());
            }
        }

        IEnumerator RecoveryMana()
        {
            isCoroutine = false;

            while (playerStats.MaxMana >= playerStats.Mana) 
            {
                yield return new WaitForSeconds(1);

                if (playerStats.MaxMana < playerStats.Mana + playerStats.GetModifiedValue(AttributeType.Intellect))
                {
                    playerStats.AddMana(playerStats.MaxMana - playerStats.Mana);
                }
                else
                {
                    playerStats.AddMana(playerStats.GetModifiedValue(AttributeType.Intellect));
                }

                if (playerStats.MaxMana <= playerStats.Mana)
                {
                    isCoroutine = true;

                    yield break;
                }
            }
            yield return null;
        }

        private void InitAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                behaviour.targetMask = targetMask;
            }

            foreach (AttackBehaviour behaviour in skillAttackBehaviours)
            {
                behaviour.targetMask = targetMask;
            }
        }

        private void CheckAttackBehaviour()
        {
            if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
            {
                CurrentAttackBehaviour = null;

                foreach (AttackBehaviour behaviour in attackBehaviours)
                {
                    if (behaviour.IsAvailable)
                    {
                        if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour.priority < behaviour.priority))
                        {
                            CurrentAttackBehaviour = behaviour;
                        }
                    }
                }
            }
        }

        private void CheckSkillAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in skillAttackBehaviours)
            {
                if (behaviour.IsAvailable)
                {
                    SkillCurrentAttackBehaviour = behaviour;
                }   
            }
        }

        private void SetAttackStart()
        {
            AttackInProgress = true;
        }

        private void SetAttackEnd()
        {
            AttackInProgress = false;
        }

        private void Attack()
        {
            playerStats.AddMana(-10);
            animator.SetTrigger(attackTriggerHash);
            animator.SetInteger(attackIndexHash, CurrentAttackBehaviour.animationIndex);
        }

        public void SkillAttack(int useMana)
        {
            playerStats.AddMana(-useMana);
            animator.SetTrigger(skillattackTriggerHash);
            animator.SetInteger(skillAttackIndexHash, SkillCurrentAttackBehaviour.animationIndex);
        }

        #region IAttackable Interfaces
        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();
        [SerializeField]
        private List<AttackBehaviour> skillAttackBehaviours = new List<AttackBehaviour>();

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }

        public AttackBehaviour SkillCurrentAttackBehaviour
        {
            get;
            private set;
        }

        public void OnExecuteAttack(int attackIndex)
        {
            if (CurrentAttackBehaviour != null)
            {
                CurrentAttackBehaviour.ExecuteAttack();
            }
        }

        public void OnExecuteSkillAttack()
        {
            if (SkillCurrentAttackBehaviour != null)
            {
                SkillCurrentAttackBehaviour.ExecuteSkillAttack();
            }
        }

        #endregion IAttackable Interfaces

        #region IDamagable Interfaces

        public bool IsAlive => playerStats.Health > 0;

        public void TakeDamage(int damage, GameObject damageEffectPrefab)
        {
            if (!IsAlive)
            {
                return;
            }

            playerStats.AddHealth(-damage);

            if (damageEffectPrefab)
            {
                Instantiate<GameObject>(damageEffectPrefab, hitPoint);
            }

            if (IsAlive)
            {
                animator?.SetTrigger(hitTriggerHash);
            }
            else
            {
                animator?.SetBool(isAliveHash, false);
            }
        }

        #endregion IDamagable Interfaces

        #region Inventory
        private void OnUseItem(InventorySlot inventorySlot)
        {
            ItemObject itemObject = inventorySlot.ItemObject;

            if (playerStats.Health >= playerStats.MaxHealth)
            {
                return;
            }
            else
            {
                foreach (ItemBuff buff in itemObject.data.buffs)
                {
                    if (buff.stat == AttributeType.Health)
                    {
                        if (playerStats.Health + buff.value > playerStats.MaxHealth)
                        {
                            playerStats.AddHealth(playerStats.MaxHealth - playerStats.Health);
                        }
                        else
                        {
                            playerStats.AddHealth(buff.value);
                        }
                        inventorySlot.UpdateSlot(inventorySlot.item, inventorySlot.amount - 1);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<GroundItem>();
            if (item)
            {
                if (inventory.AddItem(new Item(item.itemObject), 1))
                    Destroy(other.gameObject);
            }
        }
        #endregion Inventory

        public void OnClickAttackButton()
        {
            Attack();
        }

    }
}
