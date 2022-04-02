using UnityEngine;

namespace LudumDare50.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/FoodInfo", fileName = "FoodInfo")]
    public class FoodInfo : ScriptableObject
    {
        [Tooltip("Name of the food")]
        public string Name;

        [Tooltip("Is the food good or bad to the player")]
        public bool IsGoodFood;
    }
}