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
        private Node[] _nodes;

        private List<Node> _locations;

        private void Start()
        {
            _locations = _nodes.ToList();
        }

        public Node GetNextNode(NeedType need)
        {
            var okay = _locations.Where(x => x.GivenNeed == need).ToArray();
            return okay[Random.Range(0, okay.Length)];
        }
    }
}
