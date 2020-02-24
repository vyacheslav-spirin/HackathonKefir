using System;
using UnityEngine;

namespace Speech
{
    [Serializable]
    public struct Speech
    {
        [SerializeField] private Character _charId;
        [SerializeField] private float _delay;
        [SerializeField] private float _duration;
        [SerializeField] private string _speechText;

        public Character CharId => _charId;
        public float Delay => _delay;
        public float Duration => _duration;
        public string SpeechText => _speechText;

        public enum Character
        {
            Inviser = 0,
            Hatman = 1,
            Swapper = 2
        }
    }
}