using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour
{
    [SerializeField]
    private Color _defaultColor;
    [SerializeField]
    private Color _detectedColor;

    [SerializeField]
    private Image _indicator;

    private void Start()
    {
        _indicator.color = _defaultColor;
    }

    public void ChangeIndicatorColor(bool isDetected)
    {
        if (isDetected)
        {
            _indicator.DOColor(_detectedColor, 0.5f);
        }
        else
        {
            _indicator.DOColor(_defaultColor, 0.5f);
        }
    }
}
