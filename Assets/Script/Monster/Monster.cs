using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Idle,
    Move,
    Attack,
    Dead
}

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterState _monsterState;
    [SerializeField] private Collider _monsterHitBox;
    [SerializeField] private Collider _monsterDeathBox;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _indicator;
    [SerializeField] private Rigidbody _rigidbody;
    private bool _isDead = false;
    private bool _isAttacking = false;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private Vector3 _moveDir;

    private void Start()
    {
        _monsterState = MonsterState.Idle;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        if (_isMoving)
        {
            transform.rotation = Quaternion.LookRotation(-_moveDir);
        }
    }


    private void Update()
    {
        if (_isDead)
        {
            return;
        }
        ChangeMonsterBehavior();
    }

    private void FixedUpdate()
    {
        if (_isDead || _isAttacking)
        {
            return;
        }
        if (_isMoving)
        {
            ChangeMonsterState(MonsterState.Move);
            Move(_moveDir);
        }
    }

    private void Move(Vector3 moveDir)
    {
        if (_isDead || _isAttacking)
        {
            return;
        }
        _animator.Play("Walk");
        _rigidbody.MovePosition(transform.position + moveDir * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isDead || _isAttacking)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            ChangeMonsterState(MonsterState.Attack);
            Attack(other.transform);
            GetComponent<IndicatorController>().ChangeIndicatorColor(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead || _isAttacking)
        {
            return;
        }
        if (collision.collider.CompareTag("Wall"))
        {
            _moveDir = Vector3.Reflect(_moveDir, collision.contacts[0].normal);

            transform.DORotateQuaternion(Quaternion.LookRotation(-_moveDir), 0.3f);
        }
    }

    private void ChangeMonsterBehavior()
    {
        switch (_monsterState)
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Move:
                
                break;
            case MonsterState.Attack:
                break;
            case MonsterState.Dead:
                Dead();
                break;
        }
    }

    private void Idle()
    {
        _animator.Play("Idle");
    }

    private void Attack(Transform target)
    {
        if (_isAttacking || MovementManager.instance.IsPlayerMoving() || _isDead)
        {
            return;
        }
        transform.rotation = Quaternion.LookRotation(transform.position - target.position);
        Vector3 pos = new Vector3(target.position.x, 0, target.position.z);
        _isAttacking = true;
        _animator.Play("Attack");
        transform.DOJump(pos, .5f, 1, 1f).OnComplete(() =>
        {
            ChangeMonsterState(MonsterState.Idle);
            _isAttacking = false;
        });
        target.GetComponent<Player>().ChangePlayerState(PlayerState.Dead);
        StartCoroutine(target.GetComponent<Player>().Dead());
    }

    private void Dead()
    {
        if (_isDead || _isAttacking)
        {
            return;
        }
        _animator.Play("Dead");
        _isDead = true;
        _monsterDeathBox.enabled = false;
        _indicator.SetActive(false);
    }

    public void ChangeMonsterState(MonsterState state)
    {
        _monsterState = state;
    }
}
