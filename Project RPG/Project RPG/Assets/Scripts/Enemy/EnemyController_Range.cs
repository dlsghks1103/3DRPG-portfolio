using System.Collections;
using System.Collections.Generic;
using RPG.QuestSystem;
using UnityEngine;
using RPG.AI;
using RPG.Core;
using RPG.UIs;
using RPG.CharacterControl;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;
using RPG.StatsSystem;

namespace RPG.Enemy
{
    public class EnemyController_Range : EnemyController, IAttackable, IDamagable
    {
        #region Variables

        [SerializeField]
        public int EnemyID;

        public Transform hitPoint;
        public Transform[] waypoints;

        [SerializeField]
        private NPCBattleUI battleUI;

        [SerializeField]
        private GameObject itemPrefab;

        [SerializeField]
        private ItemDatabaseObject itemDatabase;

        public float maxHealth => 100f;
        private float health;

        private int hitTriggerHash = Animator.StringToHash("HitTrigger");

        [SerializeField]
        private Transform projectilePoint;

        [SerializeField]
        private int enemyExp;

        [SerializeField]
        private int enemyGold;

        private float gravity = -9.81f;
        private bool isGrounded;
        private Vector3 calcVelocity = Vector3.zero;

        private CharacterController controller;

        #endregion Variables

        #region Proeprties
        public override float AttackRange => CurrentAttackBehaviour?.range ?? 6.0f;

        public override bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                {
                    return false;
                }

                float distance = Vector3.Distance(transform.position, Target.position);
                return (distance <= AttackRange);
            }
        }

        #endregion Properties

        #region Unity Methods

        protected override void Start()
        {
            controller = GetComponent<CharacterController>();

            base.Start();

            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());

            health = maxHealth;

            if (battleUI)
            {
                battleUI.MinimumValue = 0.0f;
                battleUI.MaximumValue = maxHealth;
                battleUI.Value = health;
            }

            InitAttackBehaviour();
        }

        protected override void Update()
        {
            CheckAttackBehaviour();

            base.Update();
        }
        #endregion Unity Methods

        #region Helper Methods
        private void InitAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                if (CurrentAttackBehaviour == null)
                {
                    CurrentAttackBehaviour = behaviour;
                }

                behaviour.targetMask = TargetMask;
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

        #endregion Helper Methods

        #region IDamagable interfaces

        public bool IsAlive => (health > 0);

        public void TakeDamage(int damage, GameObject hitEffectPrefab)
        {
            if (!IsAlive)
            {
                return;
            }

            health -= damage;

            if (battleUI)
            {
                battleUI.Value = health;
                battleUI.TakeDamage(damage);
            }

            if (hitEffectPrefab)
            {
                Instantiate(hitEffectPrefab, hitPoint);
            }

            if (IsAlive)
            {
                animator?.SetTrigger(hitTriggerHash);
            }
            else
            {
                if (battleUI != null)
                {
                    battleUI.enabled = false;
                }
                StatsManager.Instance.statsObject.AddExp(enemyExp);
                StatsManager.Instance.statsObject.AddGold(enemyGold);
                ItemDrop();
                QuestManager.Instance.ProcessQuest(QuestType.DestroyEnemy, EnemyID);
                stateMachine.ChangeState<DeadState>();
            }
        }

        #endregion IDamagable interfaces

        #region IAttackable Interfaces


        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }

        public void OnExecuteAttack(int attackIndex)
        {
            if (CurrentAttackBehaviour != null && Target != null)
            {
                CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, projectilePoint);
            }
        }

        public void OnExecuteSkillAttack()
        { 
            
        }

        private void ItemDrop()
        {
            Vector3 itemY  = new Vector3(0,0.4f,0) ;

            if (itemPrefab != null)
            {
                GameObject itemGO = Instantiate(itemPrefab);
                itemGO.transform.position = transform.position + itemY;
                GroundItem groundItem = itemGO.GetComponent<GroundItem>();
                groundItem.itemObject = RandomDrop();
                groundItem.ItemIcon();
                itemGO.SetActive(true);
            }
        }

        private ItemObject RandomDrop()
        {
            int random =Random.Range(0, itemDatabase.itemObjects.Length-1);

            return itemDatabase.itemObjects[random];
        }

        #endregion IAttackable Interfaces
    }
}
