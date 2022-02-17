using UnityEngine;
using RPG.CharacterControl;
using RPG.Core;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using RPG.QuickSlot;
using UnityEngine.SceneManagement;

namespace RPG.PlyerController
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IAttackable, IDamagable
    {
        #region Variables
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
        public Transform target;

        private Camera camera;

        private bool isCoroutine = true;

        private int skillIndex = 0;

        private int pointerID = 0;

        [NonSerialized]
        public string currentMapName;

        public GameObject potalUI;

        bool isOnUI = false;
        #endregion Variables

        #region Properties
        public bool AttackInProgress { get; private set; } = false;
        #endregion Properties

        #region Unity Methods
        private void Awake()
        {
            var obj = FindObjectsOfType<PlayerController>();

            if (obj.Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
#if UNITY_EDITOR
            pointerID = 0;
#endif

#if UNITY_ANDROID
            pointerID = 0;
#endif

            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
            camera = Camera.main;
            InitAttackBehaviour();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Update()
        {
            if (!IsAlive)
            {
                return;
            }

            CheckAttackBehaviour();
            CheckSkillAttackBehaviour();

            if (EventSystem.current == null)
            {
                return;
            }
            else
            {
                isOnUI = EventSystem.current.IsPointerOverGameObject(pointerID);
            }

            if (!isOnUI && playerInput.AttackInput && !AttackInProgress&& playerStats.Mana >0)
            {
                Attack();
            }

            if (!isOnUI && playerInput.NPCInteractInput)
            {
                GameManager.Instance.CloseUI();

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    target = hit.collider.transform;
                    NPCInteract(target);
                }
            }

            if (playerStats.MaxMana > playerStats.Mana && isCoroutine)
            {
                StartCoroutine(RecoveryMana());
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<GroundItem>())
            {

                GroundItem item = other.GetComponent<GroundItem>();
                if (item)
                {
                    if (inventory.AddItem(new Item(item.itemObject), 1))
                        Destroy(other.gameObject);
                }
            }
            else if (other.GetComponent<PotalSystem>())
            {
                PotalSystem loadSceneName = other.GetComponent<PotalSystem>();
                currentMapName = loadSceneName.potalName;
                potalUI.gameObject.SetActive(true);
            }
        }

        #endregion Unity Methods

        #region Methods
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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
           
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
                    if (behaviour.animationIndex == skillIndex)
                    {
                        SkillCurrentAttackBehaviour = behaviour;
                    }
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

        public void SkillAttack(int useMana, int animationIndex)
        {
            skillIndex = animationIndex;

            playerStats.AddMana(-useMana);
            animator.SetTrigger(skillattackTriggerHash);
            //animator.SetInteger(skillAttackIndexHash, SkillCurrentAttackBehaviour.animationIndex);
            animator.SetInteger(skillAttackIndexHash, skillIndex);
        }
        public void OnClickGO()
        {
            if (currentMapName == "Home")
            {
                transform.position = GameObject.Find("StartPoint").transform.position;
                potalUI.gameObject.SetActive(false);
            }
            else
            {
                potalUI.gameObject.SetActive(false);
                LoadingSceneManager.LoadScene(currentMapName);
            }
        }

        public void OnClickAttackButton()
        {
            if (playerStats.Mana > 0)
            {
                Attack();
            }
        }

        #endregion Methods

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
        public void OnUseItem(QuickSlotObject quickSlotObject)
        {
            ItemObject itemObject = quickSlotObject.itemObject;

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
                        quickSlotObject.amount -= 1;
                    }
                }
            }
        }
        #endregion  Inventory

        #region NPCInteract
        private void NPCInteract(Transform target)
        {
            if (target != null)
            {
                if (target.GetComponent<IInteractable>() != null)
                {
                    IInteractable interactable = target.GetComponent<IInteractable>();
                    interactable.Interact(this.gameObject);
                    target = null;
                }
            }
         }

        #endregion NPCInteract
    }
}
