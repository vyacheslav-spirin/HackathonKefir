using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Intro
{
    public class IntroFinisher : MonoBehaviour
    {
        [SerializeField] private string SceneName;

        private void OnEnable()
        {
            SceneManager.LoadScene(SceneName);
            Debug.Log($"SWITCH SCENE: {SceneName}");
        }
    }
}