using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Intro
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponents<PlayerController>().Length > 0)
            {
                SceneManager.LoadScene(_sceneName);
            }
        }
    }
}