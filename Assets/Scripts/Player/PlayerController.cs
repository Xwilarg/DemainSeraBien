using LudumDare50.Objective;
using LudumDare50.SO;
using UnityEngine;
using UnityEngine.AI;

namespace LudumDare50.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Node _destination;

        [SerializeField]
        private PlayerInfo _info;

        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.destination = _destination.transform.position;
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, _destination.transform.position) < _info.MinDistBetweenNode)
            {
                Debug.Log(_destination.name);
                _destination = _destination.NextNode;
                _agent.destination = _destination.transform.position;
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _destination.transform.position);
        }
    }
}
