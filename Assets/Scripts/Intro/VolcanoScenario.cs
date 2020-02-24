using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Speech;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Intro
{
    public class VolcanoScenario : MonoBehaviour
    {
        [SerializeField] private float _startDelay = 0.1f;
        [SerializeField] private Image _blackScreen;
        [SerializeField] private SpeechView _speechView;
        [SerializeField] private Transform _inviserSpeechTransform;
        [SerializeField] private Transform _npcSpeechTransform;

        [Space] [Header("Talk1")] [SerializeField]
        private Speech.Speech _speech1;

        [Space] [Header("Upcoming")] 
        [SerializeField] private float _upcomingDuration;
        [SerializeField] private Transform[] _characters;
        private Vector3[] _startPositions;
        [SerializeField] private Vector3[] _upcomingPositions;
        [SerializeField] private Animator[] _animators;
        
        [Space] [Header("RotateNpc")] 
        [SerializeField] private float _rotateDuration;
        [SerializeField] private float _rotateAngle = 180f;
        [SerializeField] private Vector3 _npcStartRotation;
        [SerializeField] private Transform _npcTransform;
        
        [Space] [Header("Talk2")] [SerializeField]
        private Speech.Speech _speech2;
        
        [Space] [Header("Talk3")] 
        [SerializeField] private Speech.Speech _speech3;
        [SerializeField] private Speech.Speech _speech4;
        
        [Space] [Header("Talk4")] 
        [SerializeField] private Speech.Speech _speech5;
        
        [Space] [Header("Talk5")] 
        [SerializeField] private Speech.Speech _speech6;
        
        [Space] [Header("ToBeContinued")]
        // [SerializeField] private Speech.Speech _speech6;
        
        [Space] [Header("CameraMove")] 
        [SerializeField] private Transform _camera;
        [SerializeField] private Vector3 _cameraMove;

        
        private void Awake()
        {
            Init();
            StartCoroutine(Scenario());
        }

        private void Init()
        {
            _startPositions = new Vector3[_characters.Length];
            for (int i = 0; i < _characters.Length; i++)
            {
                _startPositions[i] = _characters[i].transform.position;
            }
        }

        private IEnumerator Scenario()
        {
            Coroutine blackScreen = StartCoroutine(BlackScreen(1f, 0f, 1.2f));
            yield return new WaitForSeconds(_startDelay);


            Coroutine upcoming = StartCoroutine(Upcoming());
            Coroutine talk1 = StartCoroutine(Talk(_speech1, _inviserSpeechTransform));
            yield return blackScreen;
            yield return talk1;

            Coroutine rotate = StartCoroutine(RotateNpc());
            yield return upcoming;
            yield return rotate;
            
            yield return StartCoroutine(Talk(_speech2, _inviserSpeechTransform));
            yield return StartCoroutine(Talk(_speech3, _npcSpeechTransform));
            yield return StartCoroutine(Talk(_speech4, _npcSpeechTransform));
            yield return StartCoroutine(Talk(_speech5, _inviserSpeechTransform));
            Coroutine lastSpeech = StartCoroutine(Talk(_speech6, _npcSpeechTransform));
            Coroutine toBeContinued = StartCoroutine(ToBeContinued());

            yield return lastSpeech;
            yield return toBeContinued;
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

        private IEnumerator Talk(Speech.Speech speech, Transform speechTransform)
        {
            yield return new WaitForSeconds(speech.Delay);
            _speechView.SetActive(true);
            _speechView.SetFollowTransform(speechTransform);
            _speechView.SetText(speech.SpeechText); 
            _speechView.RebuildLayout();
            yield return new WaitForSeconds(speech.Duration);
            _speechView.SetActive(false);
        }
            
        
        private IEnumerator Upcoming()
        {
            foreach (Animator animator in _animators)
            {
                animator.SetFloat("Velocity", 1.0f);
                animator.SetBool("IsGround", true);
            }
            
            float endTs = Time.time + _upcomingDuration;
            float startTs = Time.time;
            float nowTs = Time.time;
            
            while (nowTs < endTs)
            {
                float t = (nowTs - startTs) / (endTs - startTs);
                // IEnumerable<Vector3> positions = 
                //     _startPositions
                //     .Zip(_upcomingPositions, (s, f) => new Tuple<Vector3, Vector3>(s, f))
                //     .Select(x => Vector3.Lerp(x.Item1, x.Item2, t));

                for (int i = 0; i < _characters.Length; i++)
                {
                    Vector3 start = _startPositions[i];
                    Vector3 finish = _upcomingPositions[i];
                    _characters[i].position = Vector3.Lerp(start, finish, t);
                }
                   
                yield return new WaitForEndOfFrame();
                nowTs += Time.deltaTime;
            }
            
            foreach (Animator animator in _animators)
            {
                animator.SetFloat("Velocity", .0f);
            }
        }

        private IEnumerator RotateNpc()
        {
            float endTs = Time.time + _rotateDuration;
            float startTs = Time.time;
            float nowTs = Time.time;
            Vector3 finish = _npcStartRotation + new Vector3(0f, _rotateAngle, 0f);
            
            while (nowTs < endTs)
            {
                _npcTransform.rotation = Quaternion.Euler(Vector3.Lerp(_npcStartRotation, finish, (nowTs - startTs) / (endTs - startTs)));  
                yield return new WaitForEndOfFrame();
                nowTs += Time.deltaTime;
            }
            _npcTransform.rotation = Quaternion.Euler(finish);
        }

        private IEnumerator ToBeContinued()
        {
            yield return new WaitForSeconds(_speech6.Delay);
            
            ContinueMover.Play();
        }
    }
}