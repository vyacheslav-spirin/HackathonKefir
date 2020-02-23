using System;
using System.Collections;
using UnityEngine;

namespace Intro
{
    public class Frame3Scenario : MonoBehaviour
    {
        [SerializeField] private float _startDelay = 1f;
        
        [Space] [Header("Oncoming")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Vector3 _cameraStartPos;
        [SerializeField] private Vector3 _cameraEndPos;
        [SerializeField] private float _cameraOncomingTime;

        [Space] [Header("Shake")] 
        [SerializeField] private float _shakeDelay;
        [SerializeField] private AnimationCurve _shakeCurve;
        [SerializeField] private float _cameraYOffset;
        [SerializeField] private float _shakeDuration;
        
        [Space] [Header("Down")]
        [SerializeField] private float _downDelay;
        [SerializeField] private float _downDuration;
        [SerializeField] private Transform _rogueTransform;
        

        private void Awake()
        {
            StartCoroutine(Scenario());
        }

        private IEnumerator Scenario()
        {
            Debug.Log("Start scenario");
            yield return new WaitForSeconds(_startDelay);
    
            yield return StartCoroutine(CameraOncoming());
        }

        private IEnumerator CameraOncoming()
        {
            Debug.Log("Start camera flow");
            float endTs = Time.time + _cameraOncomingTime;
            float startTs = Time.time;
            float nowTs = Time.time;
            while (nowTs < endTs)
            {
                Debug.Log("Update camera position");
                _cameraTransform.position = Vector3.Lerp(_cameraStartPos, _cameraEndPos, (nowTs - startTs) / (endTs - startTs));
                yield return new WaitForEndOfFrame();
                nowTs += Time.deltaTime;
            }

            Coroutine sc = StartCoroutine(CameraShake());
            Coroutine fall = StartCoroutine(FallDown());
            
            yield return sc;
            yield return fall;
        }

        private IEnumerator CameraShake()
        {
            Debug.Log("Start camera shake");
            yield return new WaitForSeconds(_shakeDuration);
            
            float endTs = Time.time + _shakeDuration;
            float startTs = Time.time;
            float nowTs = Time.time;
            
            while (nowTs < endTs)
            {
                Debug.Log("Update camera position");
                float yOffset = _cameraYOffset * _shakeCurve.Evaluate((nowTs - startTs) / (endTs - startTs));
                _cameraTransform.position = _cameraEndPos + new Vector3(0f, yOffset);
                yield return new WaitForEndOfFrame();
                nowTs += Time.deltaTime;
            }
        }

        private IEnumerator FallDown()
        {
            yield return new WaitForSeconds(1f);
        }
    }
}