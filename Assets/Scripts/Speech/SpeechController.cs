using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Speech
{
    public class SpeechController : MonoBehaviour
    {
        [SerializeField] private SpeechView _view;
        [SerializeField] private List<MonoBehaviour> _speakers;
        private Coroutine _speechCoroutine;
        private void OnTriggerEnter(Collider other)
        {
            SpeechTrigger speechTrigger = other.GetComponent<SpeechTrigger>();
            if (speechTrigger != null && !speechTrigger.WasSeen)
            {
                StopAllCoroutines();
                _speechCoroutine = StartCoroutine(StartSpeech(speechTrigger));
                speechTrigger.WasSeen = true;
            }
        }

        private IEnumerator StartSpeech(SpeechTrigger trigger)
        {
            foreach (Speech speech in trigger.SpeechList)
            {
                yield return new WaitForSeconds(speech.Delay);
                _view.SetText(speech.SpeechText);
                Transform speechTransform = GetSpeechTransform(speech.CharId);
                _view.SetFollowTransform(speechTransform);
                _view.SetActive(speechTransform != null);
                _view.RebuildLayout();
                yield return new WaitForSeconds(speech.Duration);
                _view.SetActive(false);
                _view.SetFollowTransform(null);
            }

            _speechCoroutine = null;
        }

        public void AddSpeaker(Follower follower)
        {
            _speakers.Add(follower);
        }

        private Transform GetSpeechTransform(Speech.Character character)
        {
            int charId = (int) character;
            ISpeaker speaker = _speakers
                .Cast<ISpeaker>()
                .FirstOrDefault(x => x.CharId == charId);

            return speaker?.SpeechTransform;
        }
    }
}