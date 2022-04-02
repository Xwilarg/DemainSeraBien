using LudumDare50.Objective;
using LudumDare50.SO;
using UnityEngine;

namespace LudumDare50.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Node _destination;

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
            transform.LookAt(_destination.transform, Vector3.up);

            // Move toward target
            _rb.velocity = new Vector3(transform.forward.x * _info.Speed, _rb.velocity.y, transform.forward.z * _info.Speed);

            if (Vector3.Distance(transform.position, _destination.transform.position) < _info.MinDistBetweenNode)
            {
                _destination = _destination.NextNode;
            }
        }
    }
}
