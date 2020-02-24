using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Intro
{
    public class SceneStarter : MonoBehaviour
    {
        [SerializeField] private Image _blackScreen;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _fadeOutCurve;

        private void Awake() { StartCoroutine(FadeOut()); }
        
        IEnumerator FadeOut()
        {
            float now = Time.time;
            float start = now;
            float end = start + _duration;

            while (now < end)
            {
                float a = 1 - _fadeOutCurve.Evaluate((now - start) / (end - start));
                _blackScreen.color = new Color(0, 0, 0, a);
                yield return new WaitForEndOfFrame();
                now += Time.deltaTime;
            }
            _blackScreen.color = new Color(0, 0, 0, 0);
        }
    }
}