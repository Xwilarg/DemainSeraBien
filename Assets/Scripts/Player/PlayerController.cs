using LudumDare50.SO;
using UnityEngine;
using UnityEngine.AI;

namespace LudumDare50.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Node _startNode;

        [SerializeField]
        private PlayerInfo _info;

        private NavMeshAgent _agent;
        private Node _currNode;

        private void Start()
        {
            _currNode = _startNode;

            _agent = GetComponent<NavMeshAgent>();
            _agent.destination = _currNode.transform.position;
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, _currNode.transform.position) < _info.MinDistBetweenNode)
            {
                _currNode = _currNode.NextNode;
                _agent.destination = _currNode.transform.position;
            }
        }

        public void OnDrawGizmos()
        {
            if (_currNode == null)
                return;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _currNode.transform.position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Food"))
            {
                // Eat food we collide with
                Destroy(collision.gameObject);
            }
            else
            {
                var rb = collision.collider.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    var dir = collision.collider.transform.position - transform.position;
                    dir.y = Mathf.Abs(new Vector2(dir.x, dir.z).magnitude);
                    rb.AddForce(dir * 10, ForceMode.Impulse);
                }
            }
        }
    }
}
