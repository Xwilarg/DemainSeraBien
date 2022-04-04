using UnityEngine;

namespace LudumDare50.Rendering
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private float _delay;

        private Vector3? _from, _to;
        private float _timer = 0;

        private void Start()
        {
            _from = transform.position;
            _to = transform.position;
        }

        public void MoveTo(Vector3 to)
        {
            _from = _to;
            _to = to;
            _timer = 0;
        }

        private void Update()
        {
            _timer += Time.deltaTime * _delay;
            transform.position = Vector3.Lerp(_from.Value, _to.Value, _timer);
        }
    }
}
