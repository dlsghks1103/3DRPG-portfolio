using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Enemy;
using RPG.CharacterControl;

public class AttackStateController : MonoBehaviour
{
    public delegate void OnEnterAttackState();
    public OnEnterAttackState enterAttackHandler;

    public delegate void OnExitAttackState();
    public OnExitAttackState exitAttackHandler;

    public bool IsInAttackState
    {
        get;
        private set;
    }

    private void Start()
    {
        enterAttackHandler = new OnEnterAttackState(EnterAttackState);
        exitAttackHandler = new OnExitAttackState(ExitAttackState);
    }

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
}
