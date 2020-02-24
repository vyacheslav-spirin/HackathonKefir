using System.Collections.Generic;
using UnityEngine;

namespace Speech
{
    public class SpeechTrigger : MonoBehaviour
    {
        [SerializeField] private List<Speech> _speechList;
        
        public List<Speech> SpeechList => _speechList;
    }
}