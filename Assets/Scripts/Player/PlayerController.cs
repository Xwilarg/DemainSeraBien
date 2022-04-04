using LudumDare50.Prop;
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
        public static PlayerController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private TMP_Text _debugText;

        [SerializeField]
        private PlayerInfo _info;

        [SerializeField]
        private LifespanBar _healthBar;

        [SerializeField]
        private LifespanBar _barFood, _barEntertainment, _barSmoke, _barAlcohol;

        public Launcher FridgeLauncher { set; get; }
        public ParticleSystem SmokeSystem { set; get; }
        
        [SerializeField]
        private Canvas _overlayCanvas;

        [SerializeField]
        private GameObject _fadingTextAnimationPrefab;

        [SerializeField]
        private float _knockDownForce = 10f;

        private Rigidbody _rb;

        private bool _isDisabled = false;

        private float _timerReset = -1f;

        private float _age, _maxAge;
        private readonly Dictionary<NeedType, float> _needs = new()
        {
            { NeedType.Food, .4f },
            { NeedType.Entertainment, .1f },
            { NeedType.Smoke, .1f },
            { NeedType.Alcohol, .1f }
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
            _rb = GetComponent<Rigidbody>();

            _age = _info.MaxAge;
            _maxAge = 0f;
            UpdateDestination();
        }

        private Vector2 NullifyY(Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        private void FixedUpdate()
        {
            // We are close enough to node, we are going to the next one
            if (!_isDisabled && Vector2.Distance(NullifyY(transform.position), NullifyY(_currNode.transform.position)) < _info.MinDistBetweenNode)
            {
                _maxAge += _info.AgeAddedActivity;
                ReduceNeed(_currNode.GivenNeed);
                if (_currNode.GivenNeed == NeedType.Food)
                {
                    FridgeLauncher.Throw();
                }
                else if (_currNode.GivenNeed == NeedType.Smoke)
                {
                    SmokeSystem.Play();
                }
                _isDisabled = true;
                UpdateUI();
                ResetPlayer(2f);
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
            UpdateUI();

            if (_timerReset > 0f)
            {
                _timerReset -= Time.deltaTime;
                if (_timerReset <= 0f)
                {
                    _isDisabled = false;
                    _rb.isKinematic = true;
                    _agent.enabled = true;
                    SmokeSystem.Stop();
                    UpdateDestination();
                }
            }

        }

        private void UpdateDestination()
        {
            _age -= _info.AgeProgression;
            _currNode = ObjectiveManager.Instance.GetNextNode(MostNeeded);
            _agent.destination = _currNode.transform.position;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_debugText != null)
            {
                _debugText.text =
                    $"Age: {_age:0.00}\n" +
                    string.Join("\n", _needs.OrderByDescending(x => x.Value).Select(x => $"{x.Key}: {x.Value:0.00}"));
            }

            _healthBar.SetValue(_age / _info.MaxAge);
            _healthBar.SetOtherValue(_maxAge / _info.MaxAge);

            _barEntertainment.SetValue(_needs[NeedType.Entertainment]);
            _barFood.SetValue(_needs[NeedType.Food]);
            _barSmoke.SetValue(_needs[NeedType.Smoke]);
            _barAlcohol.SetValue(_needs[NeedType.Alcohol]);
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
            var consummable = collision.collider.GetComponent<ItemData>();
            if (consummable != null)
            {                
                // Eat food we collide with
                _needs[consummable.TargetNeed] -= _info.FoodPower;
                if (_needs[consummable.TargetNeed] < 0f)
                {
                    _needs[consummable.TargetNeed] = 0f;
                }
                Destroy(collision.gameObject);
                UpdateDestination();
                FadeNumberAbovePlayer(consummable.IsGood ? 1 : -1);
            }
            else
            {
                var rb = collision.collider.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    if (rb.velocity.magnitude > _knockDownForce)
                    {
                        // Stun player
                        _maxAge += _info.AgeAddedHeadTrauma;
                        _rb.isKinematic = false;
                        _agent.enabled = false;
                        ResetPlayer(3f);
                        var invDir = rb.velocity.normalized;
                        invDir.y = Mathf.Abs(new Vector2(invDir.x, invDir.z).magnitude) / 3f;
                        _rb.AddForce(invDir * _info.PropulsionForce * rb.velocity.magnitude, ForceMode.Impulse);
                        _rb.AddTorque(Random.insideUnitSphere.normalized * 10f);
                    }
                    var dir = collision.collider.transform.position - transform.position;
                    dir.y = Mathf.Abs(new Vector2(dir.x, dir.z).magnitude);
                    rb.AddForce(dir * _info.PropulsionForce, ForceMode.Impulse);
                }
            }
        }

        private void ResetPlayer(float timer)
        {
            _isDisabled = true;
            _timerReset = Mathf.Clamp(_timerReset + timer, 0f, 3f);
        }

        private void FadeNumberAbovePlayer(int n)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            GameObject fadingText = Instantiate(_fadingTextAnimationPrefab, _overlayCanvas.transform);
            fadingText.transform.position = pos;

            TextMeshProUGUI textMesh = fadingText.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            textMesh.text = (n > 0 ? "+" : "") + n;
            textMesh.color = (n > 0) ? Color.green : Color.red;

            Destroy(fadingText, 1f);
        }
    }
}
