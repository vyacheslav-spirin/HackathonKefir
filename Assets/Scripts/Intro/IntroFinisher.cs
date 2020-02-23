using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Intro
{
    public class IntroFinisher : MonoBehaviour
    {
        [SerializeField] private string SceneName;
        private void Awake()
        {
            StartCoroutine(Intro());
        }

        private IEnumerator Intro()
        {
            yield break;
        }

        private void OnEnable()
        {
            SceneManager.LoadScene(SceneName);
            Debug.Log($"SWITCH SCENE: {SceneName}");
        }
    }
}