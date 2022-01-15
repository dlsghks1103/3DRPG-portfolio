using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CharacterControl
{
    public abstract class AttackBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [Multiline]
        public string developmentDescription = "";
#endif  // UNITY_EDITOR
        public int animationIndex;

        //public AttackType type;
        public int priority;

        public int damage;
        public float range = 3f;

        [SerializeField]
        public float coolTime;

        public GameObject effectPrefab;

        protected float calcCoolTime = 0.0f;

        [HideInInspector]
        public LayerMask targetMask;

        [SerializeField]
        public bool IsAvailable => calcCoolTime >= coolTime;

        protected virtual void Start()
        {
            calcCoolTime = coolTime;
        }

        // Update is called once per frame
        protected void Update()
        {
            if (calcCoolTime < coolTime)
            {
                calcCoolTime += Time.deltaTime;
            }
        }

        public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);

        public abstract void ExecuteSkillAttack(GameObject target = null, Transform startPoint = null);
    }
}


