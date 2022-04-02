using LudumDare50.SO;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace LudumDare50.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _debugText;

        [SerializeField]
        private PlayerInfo _info;

        private readonly Dictionary<NeedType, float> _needs = new()
        {
            { NeedType.Food, .4f },
            { NeedType.Social, .1f },
            { NeedType.Exercice, .1f }
        };

        private NavMeshAgent _agent;
        private Node _currNode;

        private NeedType MostNeeded => _needs.OrderByDescending(x => x.Value).First().Key;

        private void Start()
        {
            _currNode = ObjectiveManager.Instance.GetNextNode(MostNeeded);
            UpdateDestination();
        }

        private void FixedUpdate()
        {
            // We are close enough to node, we are going to the next one
            if (Vector3.Distance(transform.position, _currNode.transform.position) < _info.MinDistBetweenNode)
            {
                _needs[_currNode.GivenNeed] = 0f;
                UpdateDestination();
            }
        }

        private void Update()
        {
            var keys = _needs.Keys;
            for (int i = keys.Count - 1; i >= 0; i--)
            {
                _needs[keys.ElementAt(i)] += Time.deltaTime * _info.NeedMultiplicator;
                if (_needs[keys.ElementAt(i)] > 1f)
                {
                    _needs[keys.ElementAt(i)] = 1f;
                }
            }
            UpdateDebugText();
        }

        private void UpdateDestination()
        {
            _currNode = ObjectiveManager.Instance.GetNextNode(MostNeeded);
            _agent.destination = _currNode.transform.position;
            UpdateDebugText();
        }

        private void UpdateDebugText()
        {
            if (_debugText != null)
            {
                _debugText.text = string.Join("\n", _needs.OrderByDescending(x => x.Value).Select(x => $"{x.Key}: {x.Value:0.00}"));
            }
        }

        public void OnDrawGizmos()
        {
            if (_currNode == null)
            {
                return;
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _currNode.transform.position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Food"))
            {
                // Eat food we collide with
                _needs[NeedType.Food] -= _info.FoodPower;
                Destroy(collision.gameObject);
                UpdateDestination();
            }
            else
            {
                var rb = collision.collider.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    var dir = collision.collider.transform.position - transform.position;
                    dir.y = Mathf.Abs(new Vector2(dir.x, dir.z).magnitude);
                    rb.AddForce(dir * _info.PropulsionForce, ForceMode.Impulse);
                }
            }
        }
    }
}
