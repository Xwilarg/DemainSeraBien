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

        private float _age;
        private readonly Dictionary<NeedType, float> _needs = new()
        {
            { NeedType.Food, .4f },
            { NeedType.Entertainment, .1f },
            { NeedType.Sleep, .1f },
            { NeedType.Exercice, .1f }
        };

        private NavMeshAgent _agent;
        private Node _currNode;

        private NeedType MostNeeded => _needs.OrderByDescending(x => x.Value).First().Key;

        public void ReduceNeed(NeedType need)
        {
            _needs[need] = 0f;
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            _age = _info.MaxAge;
            UpdateDestination();
        }

        private void FixedUpdate()
        {
            // We are close enough to node, we are going to the next one
            if (Vector3.Distance(transform.position, _currNode.transform.position) < _info.MinDistBetweenNode)
            {
                ReduceNeed(_currNode.GivenNeed);
                UpdateDestination();
            }
        }

        private void Update()
        {
            _age -= Time.deltaTime * _info.AgeProgression;
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
            _age -= _info.AgeProgression;
            _currNode = ObjectiveManager.Instance.GetNextNode(MostNeeded);
            _agent.destination = _currNode.transform.position;
            UpdateDebugText();
        }

        private void UpdateDebugText()
        {
            if (_debugText != null)
            {
                _debugText.text =
                    $"Age: {_age:0.00}\n" +
                    string.Join("\n", _needs.OrderByDescending(x => x.Value).Select(x => $"{x.Key}: {x.Value:0.00}"));
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
                if (_needs[NeedType.Food] < 0f)
                {
                    _needs[NeedType.Food] = 0f;
                }
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
