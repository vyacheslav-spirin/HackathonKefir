using System.Collections.Generic;
using UnityEngine;

namespace Intro
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private List<AudioSource> _sounds;

        private int i = 0;

        public void AudioEvent()
        {
            if (_sounds.Count > i)
            {
                _sounds[i++].Play();
            }
        }
    }
}