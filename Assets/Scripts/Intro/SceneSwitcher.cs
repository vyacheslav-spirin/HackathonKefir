using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Intro
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private Image _blackScreen;
        [SerializeField] private float _duration;

        private Coroutine _switchCoroutine;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponents<PlayerController>().Length > 0 && _switchCoroutine == null)
            {
                _switchCoroutine = StartCoroutine(SwitchScene());
            }
        }

        IEnumerator SwitchScene()
        {
            float now = Time.time;
            float start = now;
            float end = start + _duration;

            while (now < end)
            {
                float a = (now - start) / (end - start);
                _blackScreen.color = new Color(0, 0, 0, a);
                yield return new WaitForEndOfFrame();
                now += Time.deltaTime;
            }
            _blackScreen.color = Color.black;
            _switchCoroutine = null;
            SceneManager.LoadScene(_sceneName);
        }
    }
}