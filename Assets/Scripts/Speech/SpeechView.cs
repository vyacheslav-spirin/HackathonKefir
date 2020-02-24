using UnityEngine;
using UnityEngine.UI;

namespace Speech
{
    public class SpeechView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;
        [SerializeField] private Text _text;
        [SerializeField] private RectTransform _rectTransform;

        private Transform _followTransform;

        private void LateUpdate()
        {
            if (_followTransform != null)
            {
                transform.position = PositionFromWorldToCanvas(_canvas, _followTransform.position, _camera);
            }
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetFollowTransform(Transform follow)    
        {
            _followTransform = follow;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void RebuildLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }
        
        private static Vector3 PositionFromWorldToCanvas(Canvas canvas, Vector3 position, Camera worldCamera)
        {
            Vector3 pos = Vector3.zero;
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                pos = worldCamera.WorldToScreenPoint(position);
            }
            else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                var tempVector = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, worldCamera.WorldToScreenPoint(position), canvas.worldCamera, out tempVector);
                pos = canvas.transform.TransformPoint(tempVector);
            }

            return pos;
        }
    }
}