using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayerState
{
    Idle,
    Hold,
    Dash,
    Slash,
    Dead
}

public class Player : MonoBehaviour
{

    [SerializeField] private PlayerState _playerState;
    [SerializeField] private GameObject _playerHitBox;
    private bool _isDead = false;

    #region Animation declare

    private Animator _animator;

    private const string IDLE_ANIM = "Idle";
    private const string HOLD_ANIM = "Hold";
    private const string DASH_ANIM = "Dash";
    private const string SLASH_ANIM = "Slash";
    private const string DEAD_ANIM = "Dead";

    #endregion

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerState = PlayerState.Idle;
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }
        HitBoxController();
        PlayerStateManager();
        ChangePlayerBehavior();
    }

    private void HitBoxController()
    {
        if(MovementManager.instance.IsPlayerMoving())
        {
            TurnOnHitBox();
        }
        else
        {
            TurnOffHitBox();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Monster>().ChangeMonsterState(MonsterState.Dead);
        }

    }

    private void TurnOffHitBox()
    {
        if(_playerHitBox.activeInHierarchy)
        {
            _playerHitBox.SetActive(false);
        }
    }

    private void TurnOnHitBox()
    {
        if(!_playerHitBox.activeInHierarchy)
        {
            _playerHitBox.SetActive(true);
        }
    }

    private void PlayerStateManager()
    {
        if (MovementManager.instance.IsPlayerMoving())
        {
            ChangePlayerState(PlayerState.Slash);
            return;
        }
        else
        {
            if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
            {
                ChangePlayerState(PlayerState.Hold);
                return;
            }
            ChangePlayerState(PlayerState.Idle);
            return;
        }

        //else if(Input.GetMouseButtonDown(1))
        //{
        //    ChangePlayerState(PlayerState.Dash);
        //}
        //else if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    ChangePlayerState(PlayerState.Slash);
        //}
        //else if(Input.GetKeyDown(KeyCode.R))
        //{
        //    ChangePlayerState(PlayerState.Dead);
        //}
    }

    public void ChangePlayerState(PlayerState state)
    {
        _playerState = state;
    }

    private void ChangePlayerBehavior()
    {
        switch (_playerState)
        {
            case PlayerState.Idle:
                _animator.Play(IDLE_ANIM);
                break;
            case PlayerState.Hold:
                _animator.Play(HOLD_ANIM);
                break;
            case PlayerState.Dash:
                _animator.Play(DASH_ANIM);
                break;
            case PlayerState.Slash:
                _animator.Play(SLASH_ANIM);
                break;
            case PlayerState.Dead:
                break;
        }
    }

    public IEnumerator Dead()
    {
        _isDead = true;
        yield return new WaitForSeconds(.4f);
        _animator.Play(DEAD_ANIM);
        if (GameManager.instance != null)
        {
            GameManager.instance.Lose();
        }
    }

    public bool IsPlayerDead()
    {
        return _isDead;
    }
}
