using System;
using UnityEngine;

public class LightBlink : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _from;
    [SerializeField] private float _to;
    [SerializeField] private float _period;

    private void Update()
    {
        _light.intensity = _to - (_to - _from) * _curve.Evaluate((Time.time % _period) / _period);
    }
}