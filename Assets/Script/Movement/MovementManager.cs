using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static MovementManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField]
    private List<Vector3> _positionToMove;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private bool _isMoving = false;

    private void Start()
    {
        _positionToMove = new List<Vector3>();
    }

    private void Update()
    {
        if(_player.GetComponent<Player>().IsPlayerDead())
        {
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_isMoving == false)
            {
                StartCoroutine(MovePlayer());
            }
        }
    }

    private IEnumerator MovePlayer()
    {
        _isMoving = true;
        _player.GetComponent<Collider>().enabled = false;
        for (int i = 0; i < _positionToMove.Count; i++)
        {
            Vector3 pos = new Vector3(_positionToMove[i].x, _player.transform.position.y, _positionToMove[i].z);
            float distance = Vector3.Distance(_player.transform.position, pos);
            float time = distance / 40f;
            RotatePlayer(pos - _player.transform.position);
            //_player.transform.DOMove(pos, time);
            _player.GetComponent<Rigidbody>().DOMove(pos, time);
            yield return new WaitForSeconds(time);
        }
        _isMoving = false;
        _player.GetComponent<Collider>().enabled = true;
    }

    public bool IsPlayerMoving()
    {
        return _isMoving;
    }

    public void SetPositionsToMove(List<Vector3> pos)
    {
        _positionToMove = pos;
    }

    private void RotatePlayer(Vector3 dir)
    {
        //_player.transform.DORotateQuaternion(Quaternion.LookRotation(dir), 0.2f);
    }
}
