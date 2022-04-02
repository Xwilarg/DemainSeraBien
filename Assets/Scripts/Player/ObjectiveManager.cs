using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LudumDare50.Player
{
    public class ObjectiveManager : MonoBehaviour
    {
        public static ObjectiveManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private Transform[] _nodes;

        private List<Transform> _locations;

        private void Start()
        {
            _locations = _nodes.ToList();
        }

        public Transform GetNextNode()
        {
            var rand = Random.Range(1, _locations.Count);
            var t = _locations[rand];
            _locations.RemoveAt(rand);
            _locations.Insert(0, t);
            return t;
        }
    }
}
