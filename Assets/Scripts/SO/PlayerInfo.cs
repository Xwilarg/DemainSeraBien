using UnityEngine;

namespace LudumDare50.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Configuration")]

        [Tooltip("Speed of the player")]
        [Range(0f, 10f)]
        public float Speed = 1f;

        public float NeedMultiplicator;

        [Tooltip("Min distance with objective before player go toward next node")]
        [Range(0f, 1f)]
        public float MinDistBetweenNode;

        public float PropulsionForce;

        [Header("Needs")]
        public float FoodPower;
    }
}