using UnityEngine;

namespace LudumDare50.Player
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private Node _nextNode;

        public Node NextNode => _nextNode;

        public void OnDrawGizmos()
        {
            if (NextNode != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, NextNode.transform.position);
            }
        }
    }
}