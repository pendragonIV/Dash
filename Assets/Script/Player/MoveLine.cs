using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MoveLine : MonoBehaviour
{
    [Header("Line Setting")]

    [SerializeField] private LayerMask _layerToInteract;
    [SerializeField] private LayerMask _layerOfChess;
    [SerializeField] private float _lineLength = 2f;
    [SerializeField] private int _reflectionCount = 5;


    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private FloatingJoystick _joystick;
    private RaycastHit _hit;
    private Ray _ray;

    private Vector3 _mouseDownPosition;
    private float _lineMultiplier = 1f;
    private float _maxDistance = 250f;


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (MovementManager.instance.IsPlayerMoving())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            _mouseDownPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            Vector3 mousePosition = Input.mousePosition;
            _lineMultiplier = Vector3.Distance(_mouseDownPosition, mousePosition) / _maxDistance;
            if (_lineMultiplier > 1f)
            {
                _lineMultiplier = 1f;
            }

            ReflectLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _lineRenderer.positionCount = 0;
        }
    }

    private void ReflectLine()
    {
        Vector3 joystickDir = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;
        Vector3 dir = new Vector3(-joystickDir.x, 0, -joystickDir.z).normalized;
        _ray = new Ray(transform.position, dir);
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);

        List<Vector3> pos = new List<Vector3>();

        float remainingLength = _lineLength * _lineMultiplier;

        for (int i = 0; i < _reflectionCount; i++)
        {
            if(Physics.Raycast(_ray, out _hit, remainingLength, _layerOfChess))
            {
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _hit.point);
                pos.Add(_hit.point);
                break;
            }

            if (Physics.Raycast(_ray, out _hit, remainingLength, _layerToInteract))
            {
                remainingLength = remainingLength - Vector3.Distance(_ray.origin, _hit.point) + 2.5f;
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _hit.point);
                _ray = new Ray(_hit.point, Vector3.Reflect(_ray.direction, _hit.normal));
                pos.Add(_hit.point);
            }
            else
            {
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _ray.origin + _ray.direction * remainingLength);
                pos.Add(_ray.origin + _ray.direction * remainingLength);
                break;
            }
        }

        MovementManager.instance.SetPositionsToMove(pos);
    }
}
