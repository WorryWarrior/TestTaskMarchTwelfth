using UnityEngine;

namespace Content.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam = null;
        [SerializeField] private int transitionFrequency = 4;
        [SerializeField] private float transitionValue = .5f;

        private Transform _followTarget;
        private float _targetViewportScale;
        private float _initialViewportScale;

        private void Awake()
        {
            _initialViewportScale = cam.orthographicSize;
            _targetViewportScale = _initialViewportScale;
        }

        public void SetFollowTarget(Transform followTarget)
        {
            _followTarget = followTarget;
        }

        public void SetViewportScale(int progress)
        {
            _targetViewportScale = _initialViewportScale + (progress / transitionFrequency) * transitionValue;
        }

        private void LateUpdate()
        {
            if (!_followTarget)
                return;

            transform.position = Vector3.Lerp(transform.position, _followTarget.position + Vector3.back * 10f, .5f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, _targetViewportScale, .2f);
        }
    }
}