using UnityEngine;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        
        private Transform _target;
        private Vector3 _lastTargetPosition;
        private Vector2 _pos;

        private void Update()
        {
            if (!_target || _lastTargetPosition == _target.position)
                return;
            SetPosition();
        }

        public void Initialize(Transform target)
        {
            _target = target;
        }

        public void SetPosition()
        {
            var camera = Camera.main;
            if (camera == null) return;
            if (!_target) return;
            var position = _target.position;
            _pos = camera.WorldToScreenPoint(position + (Vector3.up * 3f));
            rectTransform.anchoredPosition = _pos;
            _lastTargetPosition = position;
        }
    }
}