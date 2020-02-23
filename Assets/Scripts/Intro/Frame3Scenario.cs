using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Animator _rogueAnimator;
        [SerializeField] private float _downDelay;

        [Space] [Header("Cage break")] 
        [SerializeField] private Transform _cage;
        [SerializeField] private Image _blackScreen;
        [SerializeField] private IntroFinisher _finisher;

        private void Awake()
        {
            StartCoroutine(Scenario());
        }

        private IEnumerator Scenario()
        {
            Debug.Log("Start scenario");
            yield return StartCoroutine(BlackScreen(1f, 0f, 1.2f));
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
            Coroutine breakCage = StartCoroutine(BreakCage());
            
            yield return sc;
            yield return fall;
            yield return breakCage;

            yield return StartCoroutine(BlackScreen(0f, 1f, 1.5f));

            _finisher.enabled = true;
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
            yield return new WaitForSeconds(_downDelay);

            _rogueAnimator.SetTrigger("Fall");
            
            yield return new WaitForSeconds(1f);
            
            _rogueAnimator.ResetTrigger("Fall");
        }

        private IEnumerator BreakCage()
        {
            yield return new WaitForSeconds(_shakeDelay);
            
            float endTs = Time.time + 0.5f;
            float startTs = Time.time;
            float nowTs = Time.time;
            
            Vector3 from = Vector3.zero;
            Vector3 to = Vector3.left * 90f;
            
            while (nowTs < endTs)
            {
                _cage.rotation = Quaternion.Euler(Vector3.Lerp(from, to, (nowTs - startTs) / (endTs - startTs)));
                yield return new WaitForEndOfFrame();
                nowTs += Time.deltaTime;
            }
        }
        
        private IEnumerator BlackScreen(float aFrom, float aTo, float duration)
        {
            float endTs = Time.time + duration;
            float startTs = Time.time;
            float nowTs = Time.time;
            
            Color from = new Color(0,0,0,aFrom);
            Color to = new Color(0,0,0,aTo);
            
            while (nowTs < endTs)
            {
                _blackScreen.color = Color.Lerp(from, to, (nowTs - startTs) / (endTs - startTs));
                yield return new WaitForEndOfFrame();
                nowTs += Time.deltaTime;
            }
        }
    }
}