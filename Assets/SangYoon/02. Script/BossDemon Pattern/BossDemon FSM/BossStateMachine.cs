using System;
using DemonBoss;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class BossStateMachine : MonoBehaviour
{
    Dictionary<Type, IDemonState> states;
    IDemonState current;

    public float stateTime;
    public float idleTime = 5f;
    public float walkTime = 7f;
    public float attackTime = 1f;

    public float attackDist = 3.5f;

    public Transform target;
    public Animator anim;

    public float moveSpeed = 1f;

    private IDemonState currentState;
    public AttackTrigger attackTrigger;

    public Vector3 fireBallPos;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireBallPos = new Vector3(2.15f, -1.15f, 0f);

        states = new Dictionary<Type, IDemonState> { 
            { typeof(DemonBoss.IdleState),       new DemonBoss.IdleState(this)},
            { typeof(DemonBoss.WalkState),       new DemonBoss.WalkState(this)},
            { typeof(DemonBoss.HitState),        new DemonBoss.HitState(this)},
            { typeof(DemonBoss.DeathState),      new DemonBoss.DeathState(this)},
            { typeof(DemonBoss.AttackState),     new DemonBoss.AttackState(this)},
            { typeof(DemonBoss.FireBreathState), new DemonBoss.FireBreathState(this)},
            { typeof(DemonBoss.CastSpellState),  new DemonBoss.CastSpellState(this)},
            { typeof(DemonBoss.SmashState),      new DemonBoss.SmashState(this)},
        };

        ChangeState<DemonBoss.IdleState>();
    }

    private void Start()
    {
        current?.OnEnter();
    }
    private void Update()
    {
        current?.OnState();
    }


    public void ChangeState<T>() where T : IDemonState
    {
        current?.OnExit();
        current = states[typeof(T)];
        current.OnEnter();
    }

    public void LookTarget()
    {
        Vector3 product = target.position - transform.position;
        Vector3 scale = transform.localScale;
        scale.x = product.x > 0 ? -1 : 1;
        fireBallPos.x = product.x > 0 ? -1 : 1;
        transform.localScale = scale;
    }

    /// <summary>
    /// 애니메이션 클립으로 메서드 추가해서 각 상태의 공격 데미지를 다르게 주기위함
    /// </summary>
    public void AnimationEventTrigger()
    {
        currentState?.OnAnimationEventTrigger();
    }

    public void OnGround()
    {

    }
}
