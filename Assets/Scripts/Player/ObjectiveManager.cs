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
            _locations = _nodes.ToList();
        }

        [SerializeField]
        private Node[] _nodes;

        private List<Node> _locations;

        public Node GetNextNode(NeedType need)
        {
            var okay = _locations.Where(x => x.GivenNeed == need).ToArray();
            return okay[Random.Range(0, okay.Length)];
        }
    }
}
