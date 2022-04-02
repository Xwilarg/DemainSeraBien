using LudumDare50.SO;
using UnityEngine;

namespace LudumDare50.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform _destination;

        [SerializeField]
        private PlayerInfo _info;

        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // Keep looking at target
            transform.LookAt(_destination, Vector3.up);

            // Move toward target
            _rb.velocity = new Vector3(transform.forward.x * _info.Speed, _rb.velocity.y, transform.forward.z * _info.Speed);
        }
    }
}
