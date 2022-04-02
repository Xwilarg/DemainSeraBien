using UnityEngine;

namespace LudumDare50.Player
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private NeedType _givenNeed;

        [SerializeField]
        private string _placeName;

        public NeedType GivenNeed => _givenNeed;
    }
}
