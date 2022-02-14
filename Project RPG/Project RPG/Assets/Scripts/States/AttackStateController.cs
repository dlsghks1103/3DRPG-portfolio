using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Enemy;
using RPG.CharacterControl;

public class AttackStateController : MonoBehaviour
{
    #region Variables
    public delegate void OnEnterAttackState();
    public OnEnterAttackState enterAttackHandler;

    public delegate void OnExitAttackState();
    public OnExitAttackState exitAttackHandler;
    #endregion Variables

    #region Properties
    public bool IsInAttackState
    {
        get;
        private set;
    }
    #endregion Properties

    #region Unity Methods
    private void Start()
    {
        enterAttackHandler = new OnEnterAttackState(EnterAttackState);
        exitAttackHandler = new OnExitAttackState(ExitAttackState);
    }
    #endregion Unity Methods

    #region Methods
    public void OnStartOfAttackState()
    {
        IsInAttackState = true;
        enterAttackHandler();
    }

    public void OnEndOfAttackState()
    {
        IsInAttackState = false;
        exitAttackHandler();
    }

    private void EnterAttackState()
    {
    }

    private void ExitAttackState()
    {
    }

    public void OnCheckAttackCollider(int attackIndex)
    {
        GetComponent<IAttackable>()?.OnExecuteAttack(attackIndex);
    }

    public void OnCheckSkillAttackCollider()
    {
        GetComponent<IAttackable>()?.OnExecuteSkillAttack();
    }
    #endregion Methods
}
